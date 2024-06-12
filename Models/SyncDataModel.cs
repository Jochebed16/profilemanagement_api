using System.ComponentModel.DataAnnotations;

namespace PM_API.Models
{
	public class SyncDataModel
	{
		 
		public int Id { get; set; }
		public int OperationId { get; set; }
		public string SerializedData { get; set; } = null!;
		public DateTime CreatedAt { get; set; }
	}
}
