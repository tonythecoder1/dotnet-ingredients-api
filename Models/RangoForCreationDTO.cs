using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dot_net_api.Models
{
    public class RangoForCreationDTO
    {   
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public required string Nome { get; set; }
    }
}