using System;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class XmlProjectModel
    {
        public string Name { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime DueDate { get; set; }
        public XmlTaskModel[] Tasks { get; set; }
    }
}
