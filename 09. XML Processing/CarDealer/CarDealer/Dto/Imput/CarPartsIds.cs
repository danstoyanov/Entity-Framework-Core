using System.Xml.Serialization;

namespace CarDealer.Dto.Imput
{
    [XmlType("partId")]
    public class CarPartsIds
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
