using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dot_net_api.Models
{
    public class IngredientesDTOP
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public int RangoId{ get; set; }
    }
}