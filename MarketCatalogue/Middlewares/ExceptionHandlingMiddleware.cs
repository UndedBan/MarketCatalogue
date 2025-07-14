using Microsoft.AspNetCore.Mvc;

namespace MarketCatalogue.Presentation.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly LinkGenerator _linkGenerator;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        LinkGenerator linkGenerator)
    {
        _next = next;
        _logger = logger;
        _linkGenerator = linkGenerator;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            var errorPath = _linkGenerator.GetPathByAction("Error", "Home", new { area = "" });
            context.Response.Redirect(errorPath ?? "/Home/Error");
        }
    }
}

