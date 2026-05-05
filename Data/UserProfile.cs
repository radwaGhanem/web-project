using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(AppUser))]
        public int AppUserId { get; set; }

        [MaxLength(250)]
        public string Bio { get; set; } = string.Empty;

        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        public AppUser AppUser { get; set; } = null!;
    }
}
