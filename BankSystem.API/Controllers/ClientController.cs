
using AutoMapper;
using BankSystem.App.DTOs.DTOsAccounts;
using BankSystem.App.DTOs.DTOsForRequestsToControllers;
using BankSystem.App.Services;
using BankSystem.App.Validators.AccountValidators;
using BankSystem.Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace BankSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IValidator<ClientDtoForPost> _validatorPost;
        private readonly IValidator<ClientDtoForPut> _validatorPut;
        private readonly IValidator<ClientFilterDTO> _validatorFilter;

        private readonly IValidator<AccountDTOForPost> _validatorAccPost;
        private readonly IValidator<AccountDtoForPut> _validatorAccPut;

        private readonly IValidator<CurrencyDtoForPost> _validatorCurrPost;
        private readonly IValidator<CurrencyDtoForPut> _validatorCurrPut;

        private readonly ClientService _clientService;
        private readonly IMapper _mapper;

        public ClientController(ClientService clientService, IMapper mapper,
            IValidator<ClientDtoForPost> validatorPost, IValidator<ClientDtoForPut> validatorPut,
            IValidator<AccountDTOForPost> validatorAccPost, IValidator<CurrencyDtoForPost> validatorCurrPost,
            IValidator<AccountDtoForPut> validatorAccPut, IValidator<ClientFilterDTO> validatorFilter,
            IValidator<CurrencyDtoForPut> validatorCurrPut)
        {
            _validatorCurrPut = validatorCurrPut;
            _validatorFilter = validatorFilter;
            _validatorAccPut = validatorAccPut;
            _validatorAccPost = validatorAccPost;
            _validatorCurrPost = validatorCurrPost;
            _validatorPost = validatorPost;
            _validatorPut = validatorPut;
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient([FromRoute] Guid id)
        {

            var client = await _clientService.GetAsync(id);

            if (client == null)
                return NotFound();

            var clientDto = _mapper.Map<ClientDtoForGet>(client);

            return Ok(clientDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientDtoForPost clientDto)
        {
            var resultValid = _validatorPost.Validate(clientDto);

            if (!resultValid.IsValid)
                return BadRequest(resultValid.Errors);

            var client = _mapper.Map<Client>(clientDto);

            if (client == null)
                return BadRequest();


            var result = await _clientService.AddClientAsync(client);

            if (result == false)
                return BadRequest();

            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, clientDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ClientDtoForPut clientDto)
        {
            var resultValid = _validatorPut.Validate(clientDto);

            if (!resultValid.IsValid)
                return BadRequest(resultValid.Errors);

            bool result = await _clientService.UpdateClientAsync(id, clientDto);

            if (result == false)
                return BadRequest();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _clientService.DeleteClientAsync(id);

            if (result == false)
                return BadRequest();

            return Ok(new { Message = "Клиент успешно удален" });
        }
        [HttpGet("search")]
        public async Task<IActionResult> GetByFilter([FromQuery] ClientFilterDTO filterDto,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (filterDto == null)
                return BadRequest(new { Message = "Некорректный запрос" });

            var resultValid = _validatorFilter.Validate(filterDto);
            if (!resultValid.IsValid)
                return BadRequest(resultValid.Errors);

            if (filterDto == null)
                return BadRequest();

            var resultSearch = await _clientService.GetFilterClientsAsync(filterDto, page, pageSize);

            return Ok(resultSearch);
        }

        //Вложенные методы для счетов

        [HttpPost("{idClient}/accounts")]
        public async Task<IActionResult> CreateAccount([FromRoute] Guid idClient, [FromBody] AccountDTOForPost dtoAccount)
        {
            var resultValidAcc = _validatorAccPost.Validate(dtoAccount);
            var resultValidCurr = _validatorCurrPost.Validate(dtoAccount.Currency);

            if (!resultValidAcc.IsValid)
                return BadRequest(resultValidAcc.Errors);

            if (!resultValidCurr.IsValid)
                return BadRequest(resultValidCurr.Errors);

            var account = _mapper.Map<Account>(dtoAccount);

            if (account == null)
                return BadRequest();

            var result = await _clientService.AddAccountToClientAsync(idClient, account);

            if (result == false)
                return BadRequest();

            return CreatedAtAction(
                actionName: nameof(GetClient),
                routeValues: new { id = idClient },
                value: null);
        }
        //Работает только с полным обновлением
        [HttpPut("{idClient}/accounts/{accountNumber}")]
        public async Task<IActionResult> UpdateAccount([FromRoute] Guid idClient,
            [FromRoute] string accountNumber,
            [FromBody] AccountDtoForPut accountDto)
        {
            var resultValid = _validatorAccPut.Validate(accountDto);
            var resultValidCurr = _validatorCurrPut.Validate(accountDto.Currency);

            if (!resultValid.IsValid)
                return BadRequest(resultValid.Errors);

            if (!resultValidCurr.IsValid)
                return BadRequest(resultValidCurr.Errors);

            var result = await _clientService.UpdateAccountAsync(idClient, accountNumber, accountDto);

            if (result == false)
                return BadRequest();

            return NoContent();
        }
        [HttpDelete("{idClient}/accounts/{accountNumber}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] Guid idClient, [FromRoute] string accountNumber)
        {
            var result = await _clientService.DeleteAccountAsync(idClient, accountNumber);

            if (result == false)
                return BadRequest();

            return Ok(new { Message = "Счет успешно удалён" });
        }
    }
}
