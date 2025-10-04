using BankSystem.App.Exceptions;
using System.Net;
using Newtonsoft.Json;

namespace BankSystem.API.Midldeware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //Передают дальше по стеку вызовов контекс запроса
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        public async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //Настариваю тип контекста для отправки ответа
            context.Response.ContentType = "application/json";

            switch (ex)
            {
                case AccountNotFoundException:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    }
                case ClientNotFoundException:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    }
                case EmployeeNotFoundException:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    }
                case InvalidClientAgeException:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    }
                case InvalidEmployeeAgeException:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    }
                case PassportNumberNullOrWhiteSpaceException:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    }
                case InvalidDataException:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    }
                case InvalidOperationException:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                    }
                case ArgumentNullException:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    }
                case ArgumentException:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    }
                case Exception:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                    }
            }

            //анонимный объект для создания ответа
            var respone = new
            {
                error = ex.GetType().Name,
                message = ex.Message,
                stackTrace = _env.IsDevelopment() ? ex.StackTrace : null
            };

            var json = JsonConvert.SerializeObject(respone);

            await context.Response.WriteAsync(json);
        }
    }
}

