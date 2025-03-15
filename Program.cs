using dot_net_api.Data;
using dot_net_api.entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using AutoMapper;
using dot_net_api.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Registra o `DbContext` corretamente
builder.Services.AddDbContext<RangoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("RangoDbConStr"))
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

app.MapGet("/", () => ".NET ONLINE");

app.MapGet("/rango/{id}", async (RangoDbContext rangodbcontext,int id) =>
{
    return await rangodbcontext.Rangos.FirstOrDefaultAsync(x => x.Id == id);
});

app.MapGet("/rangos/", async Task<Results<NoContent, Ok<IEnumerable<RangoDTO>>>>
    (RangoDbContext rangodbcontext, [FromQuery] string? rangoNome,
    IMapper mapper) =>
{
    // Se não houver `rangoNome`, retornar 204 
    if (string.IsNullOrWhiteSpace(rangoNome))
        return TypedResults.NoContent();

    // Busca por registros que contenham o nome fornecido
    var rangosEntity = await rangodbcontext.Rangos
                               .Where(x => x.Nome.ToLower().Contains(rangoNome.ToLower()))
                               .ToListAsync();

    // retorna 204 (No Content)
    if (!rangosEntity.Any())
        return TypedResults.NoContent();

    // Se houver resultados, retornar 200 (Ok)
    return TypedResults.Ok(mapper.Map<IEnumerable<RangoDTO>>(rangosEntity));
});

app.MapGet("/rangos/{rangoId:int}/ingredientes", async (RangoDbContext rangodbcontext, int rangoId, IMapper mapper) =>
{
    var rangoComIngredientes = await rangodbcontext.Rangos
                                 .Include(rango => rango.Ingredientes)
                                 .FirstOrDefaultAsync(rango => rango.Id == rangoId);

    if (rangoComIngredientes == null)
    {
        return Results.NotFound($"Rango com ID {rangoId} não encontrado.");
    }

    // Mapeando os ingredientes para DTO
    var ingredientesDto = mapper.Map<IEnumerable<IngredientesDTOP>>(rangoComIngredientes.Ingredientes);

    return Results.Ok(ingredientesDto);
});



app.MapGet("/db", async (RangoDbContext rangodbcontext) =>
{
    return await rangodbcontext.Rangos.ToListAsync();
}); 

app.Run();