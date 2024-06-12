using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PM_API.Models.Data.PMDbContext;

namespace PM_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProfileController : ControllerBase
	{
		private readonly PmdbContext _context;
		public ProfileController(PmdbContext context)
		{
			_context = context;
		}

		// To get the list of profiles.
		[HttpGet]
		public async Task<ActionResult<List<Profile>>> GetProfiles()
		{
			var profiles = await _context.Profiles.ToListAsync();
			//var profiles = await _context.Database.ExecuteSqlAsync($"Exec selectProfile");
			return Ok(profiles);
		}

		//To get a particular profile
		[HttpGet("{id}")]
		public async Task<ActionResult<Profile>> GetProfile(int id)
		{
			if (id == 0)
			{
				return BadRequest();
			}

			var profile = await _context.Profiles.FindAsync(id);

			if (profile == null)
			{
				return NotFound();
			}

			return Ok(profile);

		}


		//To add a profile
		[HttpPost]
		public async Task<ActionResult<Profile>> AddProfile([FromBody] Profile profile)
		{
			//await _context.Profiles.AddAsync(profile);
			//await _context.SaveChangesAsync();

			//Implementing stored procedure
			await _context.Database.ExecuteSqlAsync($"Exec createProfile {profile.Name},{profile.Contact},{profile.Email}");
			return Ok(profile);
		}

		//To update 
		[HttpPut("{id}")]
		public async Task<ActionResult> EditProfile(int id, [FromBody] Profile profile)
		{
			if (profile != null)
			{
				//_context.Update(profile);
				//await _context.SaveChangesAsync();

				//Implementing stored procedure
				await _context.Database.ExecuteSqlAsync($"Exec updateProfile {id},{profile.Name},{profile.Contact},{profile.Email}");
				return Ok(profile);
			}
			else
			{

				return BadRequest();
			}
		}

		//To delete
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteProfile(int id)
		{
			if (id == 0)
			{
				return BadRequest();

			}
			else
			{
				var profile = await _context.Profiles.FindAsync(id);

				if (profile == null)
				{
					return NotFound();
				}
				else
				{
					//_context.Remove(profile);
					//await _context.SaveChangesAsync();

					//Implementing stored procedures
					await _context.Database.ExecuteSqlAsync($"Exec deleteProfile {id}");
						return Ok();
				}
			}

		}
	}
}
