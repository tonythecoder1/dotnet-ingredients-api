using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dot_net_api.entities;
using dot_net_api.Models;

namespace dot_net_api.Migrations.Profiles
{
    public class DotNetApiProfiles : Profile
    {

        public DotNetApiProfiles()
        {
             CreateMap<Rango, RangoDTO>().ReverseMap();
             CreateMap<Ingrediente, IngredientesDTOP>()
                .ForMember(d => d.RangoId, 
                           o => o.MapFrom(s => s.Rangos.Any() ? s.Rangos.First().Id : 0))
                .ForMember(d => d.Nome, 
                           o => o.MapFrom(s => s.Nome));
                     

        }

    }
   
}
