using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dot_net_api.EndpointHandler;
using dot_net_api.EnpointFilters;

namespace dot_net_api.Extensions;

public static class EndpointsCreator
{
    public static void RegisterRangoEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/rango", RangoHandler.PostRango)
             .AddEndpointFilter(new ValidateAnnotationFilter());

        endpointRouteBuilder.MapPut("/rango/{id}", RangoHandler.PutRango)
            .AddEndpointFilter(new IngredienteFilter(12)) // Mantém o filtro corrigido
            .AddEndpointFilter(new IngredienteFilter(3));

        endpointRouteBuilder.MapDelete("/rango/{id}", RangoHandler.DeleteRango)
            .AddEndpointFilter(new IngredienteFilter(12))
            .AddEndpointFilter(new IngredienteFilter(3));


        endpointRouteBuilder.MapGet("/rango", RangoHandler.GetRangosAsync);
        endpointRouteBuilder.MapGet("/rango/{id}", RangoHandler.GetRango)
            .WithName("GetRango");
    }

    public static void RegisterIngredientesEndPoint(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/rango/{rangoId:int}/ingredientes", IngredienteHandler.BuscarIngredientesDeRango);
    }
}
