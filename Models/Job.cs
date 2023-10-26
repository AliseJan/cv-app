using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVApp.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string JobTitle { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public string Workload { get; set; }
        [Required]
        public string Responsibilities { get; set; }
        [Required]
        public List<Skill> Skills { get; set; }
        [Required]
        public List<Achievement> Achievements { get; set; }

        [ForeignKey("CV")]
        public int CVId { get; set; }

        public Job()
        {
            Skills = new List<Skill>();
            Achievements = new List<Achievement>();
        }
    }
}