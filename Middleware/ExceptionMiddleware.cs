using System.Net;
using System.Text.Json;
using repassAPI.Exceptions;

namespace repassAPI.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger) 
{
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (InvalidTokenException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (ConflictException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.Conflict, ex.Message);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (BadRequestException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            var result = JsonSerializer.Serialize(new { error = message });
            return context.Response.WriteAsync(result);
        }
    }