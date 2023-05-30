using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public class TeamMember
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name needed")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname needed")]

        public string Surname { get; set; }

        public int PositionId { get; set; }
        public Position Position { get; set; }
        [Required(ErrorMessage ="Description needed")]
        public string Description { get; set; }
        public string? Image { get; set; } 

        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
