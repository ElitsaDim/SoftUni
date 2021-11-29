namespace CarDealer.DTO.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("sale")]
    public class ExportSalesWithDiscountDto
    {
        [XmlElement("car")]
        public ExportSalesCarDto Car { get; set; }

        [XmlElement("discount")]
        public string Discount { get; set; }

        [XmlElement("customer-name")]
        public string CustomerName { get; set; }

        [XmlElement("price")]
        public string Price { get; set; }

        [XmlElement("price-with-discount")]
        public string PriceWithDiscount { get; set; }
    }
}
