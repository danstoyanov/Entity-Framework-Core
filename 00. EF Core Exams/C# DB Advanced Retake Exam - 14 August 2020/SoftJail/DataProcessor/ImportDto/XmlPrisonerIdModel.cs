using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Prisoner")]
    public class XmlPrisonerIdModel
    {
        [XmlAttribute("id")]
        public int PrisonerId { get; set; }
    }
}
