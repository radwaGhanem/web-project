using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class TaskCreateDto
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = null!;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime DueDate { get; set; }

        public int? AssignedUserId { get; set; }

        public List<int> TagIds { get; set; } = new();
    }

    public class TaskUpdateDto
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = null!;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime DueDate { get; set; }

        public bool IsDone { get; set; }

        public int? AssignedUserId { get; set; }

        public List<int> TagIds { get; set; } = new();
    }

    public class TaskAssignDto
    {
        [Required]
        public int AssignedUserId { get; set; }
    }

    public class TaskReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public bool IsDone { get; set; }
        public int? AssignedUserId { get; set; }
        public string AssignedUserName { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
    }
}
