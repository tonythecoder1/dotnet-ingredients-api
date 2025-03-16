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

//var rangosEndpoints = app.MapGroup("/rango");
//var rangosEndpointsWithParam = rangosEndpoints.MapGroup("/{rangoId:int}");


app.MapGet("/", () => ".NET ONLINE");

app.MapGet("/rango/{id}", async (RangoDbContext rangodbcontext,int id, IMapper mapper) =>
{
    var rango = await rangodbcontext.Rangos.FirstOrDefaultAsync(x => x.Id == id);
    
    if( rango == null){
        return Results.NotFound();
    }

    return Results.Ok(mapper.Map<RangoDTO>(rango));
}).WithName("GetRango");

app.MapGet("", async Task<Results<NoContent, Ok<IEnumerable<RangoDTO>>>>
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

app.MapGet("/rango/{rangoId:int}/ingredientes", async (RangoDbContext rangodbcontext, int rangoId, IMapper mapper) =>
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

app.MapPost("", async (RangoDbContext rangoDbContext, 
                             IMapper mapper, [FromBody] RangoForCreationDTO rangoForCreationDTO,
                            LinkGenerator linkGenerator, HttpContext httpContext) => 
{
    var rangoEntity = mapper.Map<Rango>(rangoForCreationDTO);
    rangoDbContext.Add(rangoEntity);
    await rangoDbContext.SaveChangesAsync();

    var rangoToReturn = mapper.Map<RangoDTO>(rangoEntity);
    var linkToReturn = linkGenerator.GetUriByName(httpContext, "GetRango", new {id = rangoToReturn.id});
    return TypedResults.Created(linkToReturn, rangoToReturn);
});

app.MapPut("/rango/{id}", async Task<Results<NotFound, Ok<RangoDTO>>> (
    RangoDbContext rangodbcontext, 
    IMapper mapper, 
    int id,
    [FromBody] RangoForCreationUpdateDTO rangoForCreationUpdateDTO) =>
{
    var rangosEntity = await rangodbcontext.Rangos.FirstOrDefaultAsync(x => x.Id == id);

    if (rangosEntity == null)
        return TypedResults.NotFound();

    mapper.Map(rangoForCreationUpdateDTO, rangosEntity);
    await rangodbcontext.SaveChangesAsync();

    var rangoToReturn = mapper.Map<RangoDTO>(rangosEntity);
    return TypedResults.Ok(rangoToReturn); 
});

app.MapDelete("/rango/{id}", async Task<Results<NotFound, NoContent>> (
    RangoDbContext rangodbcontext, 
    int id) =>
{
    var rangosEntity = await rangodbcontext.Rangos.FirstOrDefaultAsync(x => x.Id == id);

    if (rangosEntity == null){
        return TypedResults.NotFound();
    }

    rangodbcontext.Rangos.Remove(rangosEntity);
    await rangodbcontext.SaveChangesAsync();
    
    return TypedResults.NoContent(); 
});





app.Run();