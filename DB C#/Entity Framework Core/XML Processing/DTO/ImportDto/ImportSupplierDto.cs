

namespace CarDealer.DTO.ImportDto
{
    using System.Xml.Serialization;

    [XmlType("Supplier")]
    public class ImportSupplierDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("IsImporter")]
        public string IsImporter{ get; set; }

    }
}
