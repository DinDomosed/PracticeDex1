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

namespace BankSystem.App.Tests
{
    public class RateUpdaterService_Tests
    {
        [Fact]
        public async Task RateUpdater_Test()
        {
            //Arrange
            using BankSystemDbContext dbContext = new BankSystemDbContext();
            IClientStorage clientStorage = new ClientDbStorage(dbContext);
            IDateTimeProvider dateProvider = new FakeSystemDateTimeProvider(null);

            //Генерируем тесового клиента и счет , добавляем в базу данных
            TestDataGenerator generator = new TestDataGenerator();

            var client = generator.GenerateTestListClients(1).First();

            decimal initialAmount = 30000;
            Account fakeAccount = new Account(client.Id, new Currency("RUB", '₽'), initialAmount);

            await clientStorage.AddAsync(client);
            await clientStorage.AddAccountAsync(client.Id, fakeAccount);
            //Убеждаюсь , что контекст отслеживает все изменения 
            await dbContext.SaveChangesAsync();


            decimal rate = 10m;
            RateUpdaterService rateUpdater = new RateUpdaterService(clientStorage, rate, dateProvider);

            //Токен для остановки 
            using CancellationTokenSource tokenSours = new CancellationTokenSource();

            //Act 
            await rateUpdater.RateUpdaterAsync(tokenSours.Token);

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
