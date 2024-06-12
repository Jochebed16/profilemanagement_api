using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PM_API.Models;
using PM_API.Models.Data.PMDbContext;
using System.Data.Common;
using System.Text.Json;

namespace PM_API.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class SyncController : ControllerBase
	{
		private readonly PmdbContext _context;
        public SyncController(PmdbContext context)
        {
			_context = context;
        }

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] List<SyncDataModel> model)
		{
			if (model.Count == 0) return BadRequest();

			

			foreach (var syncData in model)
			{
				await ExecuteOperationsAsync(syncData);
			}

			return Ok();
		}

		private async Task ExecuteOperationsAsync(SyncDataModel model)
		{
			var profile = JsonSerializer.Deserialize<Profile>(model.SerializedData);
			if (model.OperationId == 1) //Add
			{
				await _context.Database.ExecuteSqlAsync(
			   $"Exec createProfile {profile.Name},{profile.Contact},{profile.Email}"
			   );

			}
			else if (model.OperationId == 2) //Delete
			{
				await _context.Database.ExecuteSqlAsync(
				   $"Exec deleteProfile {profile.Id}"
				   );
			}
			else
			{
				await _context.Database.ExecuteSqlAsync(
				  $"Exec updateProfile {profile.Id}, {profile.Name},{profile.Contact},{profile.Email}"
				  );

			}
		}


		private async Task<int> ExecuteSqlAsync(string sql)
		{
			int i = 0;
			using DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
			cmd.CommandText = sql;
			await _context.Database.OpenConnectionAsync();
			i = await cmd.ExecuteNonQueryAsync();
			await _context.Database.CloseConnectionAsync();
			return i;
		}

	}
}
