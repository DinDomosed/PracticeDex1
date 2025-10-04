using BankSystem.App.DTOs;
using BankSystem.App.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankSystem.App.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly CurrencyService_ApiSettiing _setting;
        public CurrencyService(HttpClient httpClient, IOptions<CurrencyService_ApiSettiing> options)
        {
            _httpClient = httpClient;
            _setting = options.Value;
        }
        public async Task<decimal> ConvertCurrency(string fromСurrencyCode, string toCurrencyCode, decimal currentAmount)
        {
            if (fromСurrencyCode.Length != 3 || toCurrencyCode.Length != 3)
                throw new InvalidDataException("Неверный код валюты");
            if (currentAmount <= 0)
                throw new InvalidDataException("Сумма конвертации не может быть меньше или равна 0");


            string url = $"{_setting.BaseUrl}?api_key={_setting.ApiKey}&from={fromСurrencyCode}&to={toCurrencyCode}&amount={currentAmount}";

            HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);

            responseMessage.EnsureSuccessStatusCode();
            var response = await responseMessage.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<CurrencyApiResultDto>(response, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result == null)
                throw new InvalidOperationException("Не удалось десериализовать ответ API");

            if (result.error != 0)
                throw new InvalidOperationException($"Ошибка API {result.error_message}");

            return result.amount;
        }
    }
}
