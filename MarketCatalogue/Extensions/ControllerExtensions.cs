using Microsoft.AspNetCore.Mvc;

namespace MarketCatalogue.Presentation.Extensions;

public static class ControllerExtensions
{
    public static IActionResult RedirectToPreviousPage(this Controller controller, string fallbackUrl = "/")
    {
        var referer = controller.HttpContext.Request.Headers["Referer"].ToString();

        if (!string.IsNullOrEmpty(referer))
            return controller.Redirect(referer);

        return controller.Redirect(fallbackUrl);
    }
}
