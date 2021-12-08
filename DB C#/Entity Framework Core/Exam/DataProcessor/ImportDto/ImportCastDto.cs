

namespace Theatre.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Cast")]
    public class ImportCastDto
    {
        [XmlElement("FullName")]
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string FullName { get; set; }

        [XmlElement("IsMainCharacter")]
        [Required]
        public bool IsMainCharacter { get; set; }

        [Required]
        [XmlElement("PhoneNumber")]
        [RegularExpression(@"^\+(44-)\d{2}\-\d{3}\-\d{4}$")]
        public string PhoneNumber { get; set; }

        [Required]
        [XmlElement("PlayId")]
        public int PlayId { get; set; }
    }
}
