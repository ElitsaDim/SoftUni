namespace CarDealer.DTO.ImportDto
{
    using System.Xml.Serialization;

    [XmlType("partId")]
    public class ImportCarPartDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
