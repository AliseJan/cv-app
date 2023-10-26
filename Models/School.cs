using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVApp.Models
{
    public class School
    {
        [Key]
        public int SchoolId { get; set; }
        [Required]
        public string SchoolName { get; set; }
        [Required]
        public string FacultyName { get; set; }
        [Required]
        public string Degree { get; set; }
        [Required]
        public string FieldOfStudy { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public string StudyStatus { get; set; }

        [ForeignKey("CV")]
        public int CVId { get; set; }
    }
}