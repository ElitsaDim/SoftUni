using System;
using System.Collections.Generic;
using System.Text;

namespace Theatre.DataProcessor.ExportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Actor")]
    public class Actors
    {
        [XmlAttribute("FullName")]
        [MinLength(4)]
        [MaxLength(30)]
        [Required]
        public string FullName { get; set; }

        [XmlAttribute("MainCharacter")]
        public string MainCharacter { get; set; }
    }
}
