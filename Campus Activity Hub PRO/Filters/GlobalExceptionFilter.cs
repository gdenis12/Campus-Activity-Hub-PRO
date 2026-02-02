using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        Console.WriteLine("EXCEPTION OCCURRED");
        Console.WriteLine($"Message: {context.Exception.Message}");
        Console.WriteLine($"Path: {context.HttpContext.Request.Path}");

        context.Result = new ViewResult
        {
            ViewName = "Error"
        };


        context.ExceptionHandled = true;
    }
}