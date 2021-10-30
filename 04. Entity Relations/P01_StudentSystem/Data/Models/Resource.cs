using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        public int  ResourceId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string  Url { get; set; }
        public ResourceType ResourceType { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set;  }
    }
}
