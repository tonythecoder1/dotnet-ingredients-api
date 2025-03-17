using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dot_net_api.EndpointHandler;

namespace dot_net_api.Extensions;

    public static class EndpointsCreator
    {
        public static void RegisterRangoEndpoints(this IEndpointRouteBuilder endpointRouteBuilder){
            endpointRouteBuilder.MapPost("", RangoHandler.PostRango);
            endpointRouteBuilder.MapPut("/rango/{id}", RangoHandler.PutRango);
            endpointRouteBuilder.MapDelete("/rango/{id}", RangoHandler.DeleteRango);
            endpointRouteBuilder.MapGet("/rango", RangoHandler.GetRangosAsync);
            endpointRouteBuilder.MapGet("/rango/{id}",RangoHandler.GetRango).WithName("GetRango");
        }

        public static void RegisterIngredientesEndPoint(this IEndpointRouteBuilder endpointRouteBuilder){
            endpointRouteBuilder.MapGet("/rango/{rangoId:int}/ingredientes", IngredienteHandler.BuscarIngredientesDeRango);
        }
    }
