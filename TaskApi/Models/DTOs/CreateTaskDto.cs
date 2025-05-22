using System;

namespace TaskApi.Dtos
{
	public class CreateTaskDto
	{
		public string Title { get; set; } = null!;
		public string? Description { get; set; }
		public DateTime? DueDate { get; set; }
		public int Priority { get; set; }
		public string Status { get; set; } = "pending";
	}
}
