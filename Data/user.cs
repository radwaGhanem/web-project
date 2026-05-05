using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required]
        public string Role { get; set; } = "User";

        public UserProfile Profile { get; set; } = null!;
        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    }
}
