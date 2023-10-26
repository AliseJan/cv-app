using System.ComponentModel.DataAnnotations;

namespace CVApp.Models
{
    public class CV
    {
        [Key]
        public int CVId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public Address Address { get; set; }
        [Required]
        public List<School> Education { get; set; }
        [Required]
        public List<Job> Experience { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now.Date;

        public CV()
        {
            Education = new List<School>();
            Experience = new List<Job>();
        }
    }
}