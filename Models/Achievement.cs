using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVApp.Models
{
    public class Achievement
    {
        [Key]
        public int AchievementId { get; set; }
        [Required]
        public string Name { get; set; }

        [ForeignKey("Job")]
        public int JobId { get; set; }
    }
}