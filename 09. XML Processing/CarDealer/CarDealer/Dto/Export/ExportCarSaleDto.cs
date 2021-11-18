﻿using System.Xml.Serialization;

namespace CarDealer.Dto.Export
{
    [XmlType("car")]
    public class ExportCarSaleDto
    {
        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }
    }
}
