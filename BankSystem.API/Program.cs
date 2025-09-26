using AutoMapper;
using BankSystem.API.Midldeware;
using BankSystem.App.DTOs.DTOsAccounts;
using BankSystem.App.DTOs.DTOsForRequestsToControllers;
using BankSystem.App.DTOs.DTosForRequestsToControllersEmployee;
using BankSystem.App.Interfaces;
using BankSystem.App.Mappings;
using BankSystem.App.Services;
using BankSystem.App.Validators.AccountValidators;
using BankSystem.App.Validators.ClientValidators;
using BankSystem.App.Validators.EmployeeValidators;
using BankSystem.Data;
using BankSystem.Data.Storages;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.OpenApi.Models;

namespace BankSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            //добавить для бд строку подключения
            builder.Services.AddDbContext<BankSystemDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


            //Добавление swagger  в DI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "BankSystem API",
                    Version = "v1"
                });
            });

            //Регистрация валидаторов
            builder.Services.AddTransient<IValidator<ClientDtoForPost>, ClientDtoForPostValidator>();
            builder.Services.AddTransient<IValidator<ClientDtoForPut>, ClientDtoForPutValidator>();
            builder.Services.AddTransient<IValidator<ClientFilterDTO>, ClientFilterDtoValidator>();

            builder.Services.AddTransient<IValidator<EmploteeDtoForPost>, EmploteeDtoForPostValidator>();
            builder.Services.AddTransient<IValidator<EmployeeContractDtoForPost>, EmployeeContractDtoForPostValidator>();
            builder.Services.AddTransient<IValidator<EmployeeDtoForPut>, EmployeeDtoForPutValidation>();
            builder.Services.AddTransient<IValidator<EmployeeContractDtoForPut>, EmployeeContractDtoForPutValidator>();
            builder.Services.AddTransient<IValidator<EmployeeFilterDTO>, EmployeeFilterDtoValidator>();

            builder.Services.AddTransient<IValidator<AccountDTOForPost>, AccountDTOForPostValidator>();
            builder.Services.AddTransient<IValidator<AccountDtoForPut>, AccountDtoForPutValidator>();

            builder.Services.AddTransient<IValidator<CurrencyDtoForPost>, CurrencyDtoForPostAndPutValidator>();
            builder.Services.AddTransient<IValidator<CurrencyDtoForPut>, CurrencyDtoForPutValidator>();

            builder.Services.AddTransient<IValidator<EmployeeClientProfileDtoForPost>, EmployeeClientProfileDtoForPostValidator>();


            //Добавление автомаппера в DI
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<ClientProfile>();
            configExpression.AddProfile<EmployeeProfile>();
            configExpression.AddProfile<AccountProfile>();

            var configMapps = new MapperConfiguration(configExpression, new NullLoggerFactory());

            IMapper mapper = configMapps.CreateMapper();

            builder.Services.AddSingleton(mapper);

            //Регистирую хранилище с конкретной реализацией (1 экземпляр на 1 запрос)
            builder.Services.AddScoped<IClientStorage, ClientDbStorage>();
            builder.Services.AddScoped<IEmployeeStorage, EmployeeDbStorage>();

            builder.Services.AddScoped<ClientService>();
            builder.Services.AddScoped<EmployeeService>();

            var app = builder.Build();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();
            app.MapControllers();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BankSystem API V1");
                });
            }

            app.Run();
        }
    }
}
