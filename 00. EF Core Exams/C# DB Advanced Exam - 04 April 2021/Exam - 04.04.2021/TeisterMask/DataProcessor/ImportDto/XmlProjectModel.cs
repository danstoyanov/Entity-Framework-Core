using System;
using System.ComponentModel.DataAnnotations;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class XmlProjectModel
    {
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public string OpenDate { get; set; }

        [Required]
        public string? DueDate { get; set; }

        public XmlTaskModel[] Tasks { get; set; }
    }
}
