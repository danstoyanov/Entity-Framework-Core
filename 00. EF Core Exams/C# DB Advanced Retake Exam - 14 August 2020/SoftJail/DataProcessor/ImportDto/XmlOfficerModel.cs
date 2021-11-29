using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class XmlOfficerModel
    {
        [Required]
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Money")]
        public decimal Salary { get; set; }

        [Required]
        [XmlElement("Position")]
        public string Position { get; set; }

        [Required]
        [XmlElement("Weapon")]
        public string Weapon { get; set; }
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public XmlPrisonerIdModel[] Prisoners { get; set; }
    }
}
