using CVApp.Models;

namespace CVApp.ViewModels
{
    public class CreateCVViewModel
    {
        public CV CV { get; set; }
        public List<School> Schools { get; set; }
        public List<Job> Jobs { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Achievement> Achievements { get; set; }

        public CreateCVViewModel()
        {
            CV = new CV();
            Schools = new List<School>();
            Jobs = new List<Job>();
            Skills = new List<Skill>();
            Achievements = new List<Achievement>();
        }
    }
}