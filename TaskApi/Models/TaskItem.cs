using System;

namespace TaskApi.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public int Priority { get; set; }
        public string Status { get; set; } = null!;

        public string UserId { get; set; } = null!; // Agora é string
        public User User { get; set; } = null!;
    }
}
