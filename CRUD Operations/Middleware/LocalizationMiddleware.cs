using CRUD_Operations.Services;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;


public class ApiResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly LocalizationService _localization;

    public ApiResponseMiddleware ( RequestDelegate next , LocalizationService localization )
    {
        _next = next;
        _localization = localization;
    }

    public async Task Invoke ( HttpContext context )
    {
        try
        {
            await _next ( context );
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync ( context , ex );
        }
    }

    private Task HandleExceptionAsync ( HttpContext context , Exception exception )
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case ValidationException validationException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize ( new
                {
                    errors = validationException.Errors.Select ( e =>
                        _localization.GetLocalizedString ( e.Key , e.Value ) )
                } );
                break;
            case KeyNotFoundException _:
                code = HttpStatusCode.NotFound;
                result = _localization.GetLocalizedString ( "NotFound" );
                break;
            default:
                result = _localization.GetLocalizedString ( "ServerError" );
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        if (string.IsNullOrEmpty ( result ))
        {
            result = JsonSerializer.Serialize ( new { error = exception.Message } );
        }

        return context.Response.WriteAsync ( result );
    }
}