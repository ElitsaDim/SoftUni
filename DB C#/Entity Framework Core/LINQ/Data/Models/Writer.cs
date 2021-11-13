

using MusicHub.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models
{
    public class Writer
    {
        public Writer()
        {
            Songs = new HashSet<Song>();
        }
        
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.WRITER_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        public string Pseudonym  { get; set; }

        public virtual ICollection<Song> Songs { get; set; }

    }
}
