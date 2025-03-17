using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dot_net_api.Data;
using dot_net_api.entities;
using dot_net_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dot_net_api.EndpointHandler;

public class RangoHandler
{
    public static async Task<Results<NoContent, Ok<IEnumerable<RangoDTO>>>> GetRangosAsync(
        RangoDbContext rangodbcontext, 
        [FromQuery] string? rangoNome,
        IMapper mapper)
    {
        if (string.IsNullOrWhiteSpace(rangoNome))
            return TypedResults.NoContent();

        var rangosEntity = await rangodbcontext.Rangos
                                    .Where(x => x.Nome.ToLower().Contains(rangoNome.ToLower()))
                                    .ToListAsync();

        if (!rangosEntity.Any())
            return TypedResults.NoContent();

        return TypedResults.Ok(mapper.Map<IEnumerable<RangoDTO>>(rangosEntity));
    }

    public static async Task<IResult> PostRango(
        RangoDbContext rangoDbContext,
        IMapper mapper,
        [FromBody] RangoForCreationDTO rangoForCreationDTO,
        LinkGenerator linkGenerator,
        HttpContext httpContext)
    {
        var rangoEntity = mapper.Map<Rango>(rangoForCreationDTO);
        rangoDbContext.Add(rangoEntity);
        await rangoDbContext.SaveChangesAsync();

        var rangoToReturn = mapper.Map<RangoDTO>(rangoEntity);
        var linkToReturn = linkGenerator.GetUriByName(httpContext, "GetRango", new { id = rangoToReturn.id });

        return Results.Created(linkToReturn ?? string.Empty, rangoToReturn);
    }

    public static async Task<Results<NotFound, Ok<RangoDTO>>> PutRango (
    RangoDbContext rangodbcontext, 
    IMapper mapper, 
    int id,
    [FromBody] RangoForCreationUpdateDTO rangoForCreationUpdateDTO)
{
    var rangosEntity = await rangodbcontext.Rangos.FirstOrDefaultAsync(x => x.Id == id);

    if (rangosEntity == null)
        return TypedResults.NotFound();

    mapper.Map(rangoForCreationUpdateDTO, rangosEntity);
    await rangodbcontext.SaveChangesAsync();

    var rangoToReturn = mapper.Map<RangoDTO>(rangosEntity);
    return TypedResults.Ok(rangoToReturn); 
}

    public static async Task<Results<NotFound, NoContent>> DeleteRango (
    RangoDbContext rangodbcontext, 
    int id) 
{
    var rangosEntity = await rangodbcontext.Rangos.FirstOrDefaultAsync(x => x.Id == id);

    if (rangosEntity == null){
        return TypedResults.NotFound();
    }

    rangodbcontext.Rangos.Remove(rangosEntity);
    await rangodbcontext.SaveChangesAsync();
    
    return TypedResults.NoContent(); 
}

   public static async Task<IResult> GetRango(RangoDbContext rangodbcontext, int id, IMapper mapper) 
    {
    var rango = await rangodbcontext.Rangos.FirstOrDefaultAsync(x => x.Id == id);

    if (rango == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(mapper.Map<RangoDTO>(rango));
    }


    
}

