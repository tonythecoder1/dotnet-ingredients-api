using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dot_net_api.Models;
using MiniValidation;

namespace dot_net_api.EnpointFilters;

    public class ValidateAnnotationFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next){
            var rangoParaCriacaoDTO = context.GetArgument<RangoForCreationDTO>(2);
            if(!MiniValidator.TryValidate(rangoParaCriacaoDTO, out var validationErrors)){
                return TypedResults.ValidationProblem(validationErrors);
            }

            return await next(context);
        }


}
