using CarDealer.Models;
using System.Xml.Serialization;

namespace CarDealer.Dto.Export
{
    [XmlType("car")]
    public class ExportCarsPartsDto
    {
        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }

        [XmlArray("parts")]
        public CarPartsExportDto[] PartsCar { get; set; }
    }
}