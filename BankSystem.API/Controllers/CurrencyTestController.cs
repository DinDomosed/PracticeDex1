using BankSystem.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyTestController : ControllerBase
    {
        private readonly CurrencyService _currencyService;
        public CurrencyTestController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("testConvert")]
        public async Task<IActionResult> Convert([FromQuery] string fromCurrencyCode,
            [FromQuery] string toCurrencyCode, [FromQuery] decimal amount)
        {
            var result = await _currencyService.ConvertCurrency(fromCurrencyCode, toCurrencyCode, amount);
            return Ok(result);
        }
    }
}
