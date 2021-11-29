namespace CarDealer.DTO.ImportDto
{
    using System.Xml.Serialization;

    [XmlType("Sale")]
    public class ImportSaleDto
    {
        [XmlElement("carId")]
        public int CarId { get; set; }

        [XmlElement("customerId")]
        public int CustomerId { get; set; }

        [XmlElement("discount")]
        public int Discount { get; set; }
    }
}
