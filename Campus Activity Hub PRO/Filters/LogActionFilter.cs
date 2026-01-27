using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

public class LogActionFilter : IActionFilter
{
    private Stopwatch _timer;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _timer = Stopwatch.StartNew();

        var user = context.HttpContext.User.Identity?.Name ?? "Guest";

        Console.WriteLine("----- REQUEST START -----");
        Console.WriteLine($"URL: {context.HttpContext.Request.Path}");
        Console.WriteLine($"Method: {context.HttpContext.Request.Method}");
        Console.WriteLine($"User: {user}");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _timer.Stop();
        Console.WriteLine($"Execution time: {_timer.ElapsedMilliseconds} ms");
        Console.WriteLine("----- REQUEST END -----");
    }
}
