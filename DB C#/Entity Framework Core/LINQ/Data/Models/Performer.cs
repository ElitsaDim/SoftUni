

using MusicHub.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models
{
    public class Performer
    {
        public Performer()
        {
            PerformerSongs = new HashSet<SongPerformer>();
        }     

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.PERFORMER_NAME_MAX_LENGTH)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(ValidationConstants.PERFORMER_NAME_MAX_LENGTH)]
        public string LastName { get; set; }
        
        public int Age { get; set; }

        public decimal NetWorth  { get; set; }

        public ICollection<SongPerformer> PerformerSongs  { get; set; }
    }
}
