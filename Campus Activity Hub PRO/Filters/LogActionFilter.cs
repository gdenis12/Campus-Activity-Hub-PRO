using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

public class LogActionFilter : IActionFilter
{
    private Stopwatch _timer;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _timer = Stopwatch.StartNew();

        var user = context.HttpContext.User.Identity?.IsAuthenticated == true
            ? context.HttpContext.User.Identity.Name
            : "Guest";

        var request = context.HttpContext.Request;

        Console.WriteLine("----- REQUEST START -----");
        Console.WriteLine($"URL: {request.Path}");
        Console.WriteLine($"Method: {request.Method}");
        Console.WriteLine($"User: {user}");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _timer.Stop();

        Console.WriteLine($"Execution time: {_timer.ElapsedMilliseconds} ms");
        Console.WriteLine("----- REQUEST END -----");
    }
}