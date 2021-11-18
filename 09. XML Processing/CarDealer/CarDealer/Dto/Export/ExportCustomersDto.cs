using System.Xml.Serialization;

namespace CarDealer.Dto.Export
{
    [XmlType("customer")]
    public class ExportCustomersDto
    {
        [XmlAttribute("full-name")]
        public string FullName { get; set; }

        [XmlAttribute("bought-cars")]
        public int BoughtCars { get; set; }

        [XmlAttribute("spent-money")]
        public decimal SpendMoney { get; set; }
    }
}