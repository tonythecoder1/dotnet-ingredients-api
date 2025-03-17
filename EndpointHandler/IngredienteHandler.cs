using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper; // Adicionado para mapear DTOs
using dot_net_api.Data;
using dot_net_api.Models; // Para acesso aos DTOs
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Para acessar o EF Core

namespace dot_net_api.EndpointHandler;

public class IngredienteHandler
{
    public static async Task<IResult> BuscarIngredientesDeRango(
        RangoDbContext rangodbcontext,
        int rangoId,
        IMapper mapper)
    {
        var rangoComIngredientes = await rangodbcontext.Rangos
                                     .Include(rango => rango.Ingredientes) // Eager loading
                                     .FirstOrDefaultAsync(rango => rango.Id == rangoId);

        if (rangoComIngredientes == null)
        {
            return Results.NotFound($"Rango com ID {rangoId} não encontrado.");
        }

        // Mapeando os ingredientes para DTO
        var ingredientesDto = mapper.Map<IEnumerable<IngredientesDTOP>>(rangoComIngredientes.Ingredientes);

        return Results.Ok(ingredientesDto);
    }
}
