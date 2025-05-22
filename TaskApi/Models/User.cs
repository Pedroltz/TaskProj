using TaskApi.Models;

public class User
{
    public string Id { get; set; } = null!; // FirebaseUid agora é o ID
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
