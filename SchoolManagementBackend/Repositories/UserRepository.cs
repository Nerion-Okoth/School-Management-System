using Microsoft.EntityFrameworkCore;
using SchoolManagementBackend.Data;
using SchoolManagementBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagementBackend.Repositories
{
    public class UserRepository
    {
        private readonly SchoolDbContext _context;

        public UserRepository(SchoolDbContext context)
        {
            _context = context;
        }

        // Add a new user
        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // Get a user by their username
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Profile) // Include Profile for navigation properties
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        // Get a user by their email
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Profile) // Include Profile for navigation properties
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        // Get a user by their ID
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        // Get all users
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Profile) // Include related Profile data
                .ToListAsync();
        }

        // Update an existing user
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        // Delete a user by ID
        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        // Profile Methods

        // Get all profiles
        public async Task<IEnumerable<Profile>> GetAllProfilesAsync()
        {
            return await _context.Profiles.ToListAsync();
        }

        // Get a profile by ID
        public async Task<Profile?> GetProfileByIdAsync(int id)
        {
            return await _context.Profiles.FindAsync(id);
        }

        // Add a new profile
        public async Task<Profile> AddProfileAsync(Profile profile)
        {
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        // Update an existing profile
        public async Task UpdateProfileAsync(Profile profile)
        {
            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync();
        }

        // Delete a profile by ID
        public async Task DeleteProfileAsync(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile != null)
            {
                _context.Profiles.Remove(profile);
                await _context.SaveChangesAsync();
            }
        }

        // Additional helper methods

        // Check if a user exists by username
        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        // Check if a user exists by email
        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        // Check if a profile exists by user ID
        public async Task<bool> ProfileExistsAsync(int userId)
        {
            return await _context.Profiles.AnyAsync(p => p.UserId == userId);
        }
    }
}
