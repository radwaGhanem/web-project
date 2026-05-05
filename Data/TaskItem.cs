using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = null!;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        public DateTime DueDate { get; set; }

        public bool IsDone { get; set; }

        public int? AssignedUserId { get; set; }

        public AppUser? AssignedUser { get; set; }

        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}
