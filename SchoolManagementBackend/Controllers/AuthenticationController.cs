using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using SchoolManagementBackend.Dtos;
using SchoolManagementBackend.Models;
using SchoolManagementBackend.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
    
[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly UserRepository _userRepository;
    private readonly IMemoryCache _memoryCache; 
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IConfiguration _configuration;

    public AuthenticationController(
        UserRepository userRepository,
        IMemoryCache memoryCache,
        ILogger<AuthenticationController> logger,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _memoryCache = memoryCache;
        _logger = logger;
        _configuration = configuration;
    }

    // POST: api/authentication/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (registerDto.Password.Length < 8)
            return BadRequest(new { Message = "Password must be at least 8 characters long." });

        if (registerDto.Password != registerDto.ConfirmPassword)
            return BadRequest(new { Message = "Passwords do not match." });

        var validRoles = new[] { "Admin", "Student", "Parent", "Teacher" };
        if (!validRoles.Contains(registerDto.Role, StringComparer.OrdinalIgnoreCase))
            return BadRequest(new { Message = "Invalid role. Allowed roles are: Admin, Student, Parent, Teacher." });

        if (await _userRepository.GetUserByEmailAsync(registerDto.Email) != null)
            return Conflict(new { Message = "A user with this email already exists." });

        if (!_memoryCache.TryGetValue(registerDto.Email, out string? cachedOtp) || cachedOtp != registerDto.Otp)
            return BadRequest(new { Message = "Invalid or expired OTP. Please request a new one." });

        _memoryCache.Remove(registerDto.Email);

        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            Role = registerDto.Role,
            PasswordHash = HashPassword(registerDto.Password)
        };

        await _userRepository.AddUserAsync(user);

        return Ok(new
        {
            Message = "User registered successfully. Go back to login.",
            RedirectUrl = "http://localhost:3000"
        });
    }

    // POST: api/authentication/send-otp
    [HttpPost("send-otp")]
    public IActionResult SendOtp([FromBody] string email)
    {
        if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
            return BadRequest(new { Message = "Invalid email address." });

        if (_userRepository.GetUserByEmailAsync(email).Result != null)
            return Conflict(new { Message = "A user with this email already exists." });

        var otp = GenerateOtp();
        _memoryCache.Set(email, otp, TimeSpan.FromMinutes(10));

        _logger.LogInformation($"Generated OTP for {email}: {otp}");

        return Ok(new { Message = "OTP has been generated and logged to the terminal." });
    }

    // POST: api/authentication/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        // Fetch the user by email or username
        var user = await _userRepository.GetUserByEmailAsync(loginDto.Identifier)
                   ?? await _userRepository.GetUserByUsernameAsync(loginDto.Identifier);

        // Check if user exists and verify the password
        if (user == null || string.IsNullOrEmpty(user.PasswordHash) || !VerifyPassword(loginDto.Password, user.PasswordHash))
            return Unauthorized(new { Message = "Invalid username/email or password." });

        // Generate JWT token
        var token = GenerateJwtToken(user);

        // Check user role and provide an appropriate message or redirect URL
        string? v = user.Role.ToLower() switch
        {
            "student" => "/students-dashboard",
            "teacher" => "/teachers-dashboard",
            "parent" => "/parents-dashboard",
            "admin" => "/admin-dashboard",
            _ => null // Handle unexpected roles
        };
        string? redirectUrl = v;

        if (redirectUrl == null)
        {
            return BadRequest(new { Message = "Unrecognized role. Please contact support." });
        }

        // Respond with the token, user information, and role-specific redirect URL
        return Ok(new
        {
            Message = "Login successful.",
            Token = token,
            RedirectUrl = redirectUrl,
            User = new
            {
                user.UserId,
                user.Username,
                user.Email,
                user.Role
            }
        });
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        return HashPassword(password) == passwordHash;
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("AppSettings:JwtSettings");
        var jwtKey = jwtSettings["SecretKey"];
        var jwtIssuer = jwtSettings["Issuer"];
        var jwtAudience = jwtSettings["Audience"];
        var tokenExpirationMinutesString = jwtSettings["TokenExpirationMinutes"];
        if (string.IsNullOrEmpty(tokenExpirationMinutesString))
        {
            throw new InvalidOperationException("Token expiration minutes setting is not configured properly.");
        }
        var tokenExpirationMinutes = int.Parse(tokenExpirationMinutesString);

        if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            throw new InvalidOperationException("JWT settings are not configured properly.");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(tokenExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
