using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVApp.Models
{
    public class Skill
    {
        [Key]
        public int SkillId { get; set; }
        [Required]
        public string Name { get; set; }

        [ForeignKey("Job")]
        public int JobId { get; set; }
    }
}