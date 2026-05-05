using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class TagCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
    }

    public class TagReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
