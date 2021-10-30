using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        public int  ResourceId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string  Url { get; set; }
        public string ResourceType { get; set; }
        public int  CourseId { get; set; }
    }
}
