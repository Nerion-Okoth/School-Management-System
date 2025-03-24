using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementBackend.Models;
using SchoolManagementBackend.Repositories;

namespace SchoolManagementBackend.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public ProfileController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/profile
        [HttpGet]
        public async Task<IActionResult> GetAllProfiles()
        {
            var profiles = await _userRepository.GetAllProfilesAsync();
            return Ok(profiles);
        }

        // GET: api/profile/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfileById(int id)
        {
            var profile = await _userRepository.GetProfileByIdAsync(id);
            if (profile == null)
            {
                return NotFound("Profile not found.");
            }
            return Ok(profile);
        }

        // POST: api/profile
        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] Profile profile)
        {
            if (profile == null)
            {
                return BadRequest("Profile data is null.");
            }

            var createdProfile = await _userRepository.AddProfileAsync(profile);
            return CreatedAtAction(nameof(GetProfileById), new { id = createdProfile.ProfileId }, createdProfile);
        }

        // PUT: api/profile/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] Profile updatedProfile)
        {
            if (updatedProfile == null || id != updatedProfile.ProfileId)
            {
                return BadRequest("Profile data is invalid.");
            }

            var existingProfile = await _userRepository.GetProfileByIdAsync(id);
            if (existingProfile == null)
            {
                return NotFound("Profile not found.");
            }

            await _userRepository.UpdateProfileAsync(updatedProfile);
            return NoContent();
        }

        // DELETE: api/profile/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            var profile = await _userRepository.GetProfileByIdAsync(id);
            if (profile == null)
            {
                return NotFound("Profile not found.");
            }

            await _userRepository.DeleteProfileAsync(id);
            return NoContent();
        }
    }
}
