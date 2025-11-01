using BankSystem.App.Interfaces;
using BankSystem.Data;
using BankSystem.Data.Storages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.Services;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using BankSystem.Data.Providers;
using BankSystem.App.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace BankSystem.App.Tests
{
    public class RateUpdaterService_Tests
    {
        [Fact]
        public async Task RateUpdater_Test()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<BankSystemDbContext>()
                .UseInMemoryDatabase("TestDb_Rate")
                .Options;

            using BankSystemDbContext dbContext = new BankSystemDbContext(options);
            IClientStorage clientStorage = new ClientDbStorage(dbContext);
            

            //Генерируем тесового клиента и счет , добавляем в базу данных
            TestDataGenerator generator = new TestDataGenerator();

            var client = generator.GenerateTestListClients(1).First();

            decimal initialAmount = 30000;
            Account fakeAccount = new Account(client.Id, new Currency("RUB", '₽'), initialAmount);

            await clientStorage.AddAsync(client);
            await clientStorage.AddAccountAsync(client.Id, fakeAccount);
            //Убеждаюсь , что контекст отслеживает все изменения 
            await dbContext.SaveChangesAsync();

            var rateOptions = new RateOptions
            {
                Rate = 10m
            };
            IOptions<RateOptions> iOptions = Options.Create(rateOptions);

            IRateProvider rateProvider = new ConfigRateProvider(iOptions);


            decimal rate = rateProvider.GetRate();

            //создаю контейнер для хранения зависимостей и использую 1 уже созданный clientStorage весь тест
            var services = new ServiceCollection();
            services.AddSingleton<IClientStorage>(clientStorage);
            var servicePrivader = services.BuildServiceProvider();

            RateUpdaterService rateUpdater = new RateUpdaterService(servicePrivader, rateProvider);

            //Токен для остановки 
            using CancellationTokenSource tokenSours = new CancellationTokenSource();

            //Act 
            await rateUpdater.RateUpdaterAsync(clientStorage, tokenSours.Token);

            //Получаем обработонного клиента из бд для дальнейшего сравнения соотвествия 
            Client foundClient = await clientStorage.GetAsync(client.Id);

            //Расчитываем ожидаемую сумму
            int maxPercentage = 100;
            decimal totalAmount = initialAmount + ((initialAmount * rate) / maxPercentage);

            //Assert

            Assert.Equal(totalAmount, foundClient.Accounts.FirstOrDefault(c => c.AccountNumber == fakeAccount.AccountNumber).Amount);
        }
    }
}
