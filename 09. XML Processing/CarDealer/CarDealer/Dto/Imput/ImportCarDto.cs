using System.Xml.Serialization;
using System.Collections.Generic;

using CarDealer.Models;

namespace CarDealer.Dto.Imput
{
    [XmlType("Car")]
    public class ImportCarDto
    {
        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement("TraveledDistance")]
        public long TraveledDistance { get; set; }

        [XmlArray("parts")]
        public CarPartsIds[] CarPartsIds { get; set; }
    }
}
