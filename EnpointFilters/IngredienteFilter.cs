using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dot_net_api.EnpointFilters;
public class IngredienteFilter : IEndpointFilter
{
    public readonly int _lockedid;

    public IngredienteFilter(int lockedID){
        _lockedid = lockedID;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        int rangoId;

        if( context.HttpContext.Request.Method == "PUT"){
            rangoId = context.GetArgument<int> (2);
        } else if (context.HttpContext.Request.Method == "DELETE"){
            rangoId = context.GetArgument<int> (1);
        } else {
            throw new NotSupportedException("This filter is not supported for this scenario");
        }
                if (rangoId == _lockedid){
                    return TypedResults.Problem(new() {
                        Status = 400,
                        Title =" Este ingrediente nao pode ser modificado",
                        Detail = "Nao se pode mdoficar"
                    });
                }
                var result = await next.Invoke(context);
                return result; 
    }
}
