using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        Console.WriteLine("❌ EXCEPTION:");
        Console.WriteLine(context.Exception.Message);

        context.Result = new ViewResult
        {
            ViewName = "Error"
        };
    }
}
