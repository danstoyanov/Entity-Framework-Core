﻿using CarDealer.Models;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CarDealer.Dto.Export
{
    [XmlType("suplier")]
    public class ExportSuppliersDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("parts-count")]
        public int PartsCount { get; set; }
    }
}