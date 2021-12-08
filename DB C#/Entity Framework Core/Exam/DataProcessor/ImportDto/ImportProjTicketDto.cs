using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportProjTicketDto
    {
        [Range(typeof(decimal), "1", "101")]
        [Required]
        public decimal Price { get; set; }

        [Required]
        [Range(typeof(sbyte), "1", "10")]
        public sbyte RowNumber { get; set; }
        
        public int PlayId { get; set; }
    }
}
