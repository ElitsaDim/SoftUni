
namespace Theatre.DataProcessor.ExportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Play")]
    public class Plays
    {
        [XmlAttribute("Title")]
        [Required]
        [MinLength(4)]
        [MaxLength(50)]
        public string Title { get; set; }

        [XmlElement("Duration")]
        [Required]
        public string Duration { get; set; }

        [XmlElement("Rating")]
        public string Rating { get; set; }

        [XmlElement("Genre")]
        public string Genre { get; set; }

        [XmlArray("Actors")]
        public Actors[] Actors{ get; set; }
    }
}
