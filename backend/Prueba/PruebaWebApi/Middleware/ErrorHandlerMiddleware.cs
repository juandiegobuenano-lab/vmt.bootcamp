using Prueba.Application.Helpers;
using Prueba.Shared.Constants;
using PruebaDomain.Exceptions;

namespace PruebaWebApi.Middleware
{
    public class ErrorHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(
                    ResponseHelper.Fail<string>(ex.Message)
                );
            }
            catch (BadRequestException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(
                    ResponseHelper.Fail<string>(ex.Message)
                );
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(
                    ResponseHelper.Fail<string>(ResponseConstants.ERROR_UNEXPECTED)
                );
            }
        }
    }
}