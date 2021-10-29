using System;

namespace P01_StudentSystem.Models
{
    public class Course
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegisteredOn { get; set; }
        public DateTime Birthday { get; set; }
    }
}
