using dot_net_api.Data;
using dot_net_api.entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using AutoMapper;
using dot_net_api.Models;
using dot_net_api.EndpointHandler;
using dot_net_api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("RangoDbConStr"))
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.MapGet("/", () => ".NET ONLINE");

app.RegisterRangoEndpoints();
app.RegisterIngredientesEndPoint();

//var rangosEndpoints = app.MapGroup("/rango");
//var rangosEndpointsWithParam = rangosEndpoints.MapGroup("/{rangoId:int}");

app.MapGet("/db", async (RangoDbContext rangodbcontext) =>
{
    return await rangodbcontext.Rangos.ToListAsync();
}); 

app.Run();