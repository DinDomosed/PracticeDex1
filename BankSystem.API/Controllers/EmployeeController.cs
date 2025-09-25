using AutoMapper;
using BankSystem.App.DTOs.DTOsAccounts;
using BankSystem.App.DTOs.DTosForRequestsToControllersEmployee;
using BankSystem.App.Services;
using BankSystem.Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.CompilerServices;
namespace BankSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IValidator<EmploteeDtoForPost> _validatorPost;
        private readonly IValidator<EmployeeContractDtoForPost> _validatorContrPost;
        private readonly IValidator<EmployeeDtoForPut> _validatorPut;
        private readonly IValidator<EmployeeContractDtoForPut> _validatorContrPut;
        private readonly IValidator<EmployeeFilterDTO> _validatorFilter;

        private readonly IValidator<EmployeeClientProfileDtoForPost> _validatorClientProfile;
        private readonly IValidator<CurrencyDtoForPost> _validatorCurrPost;

        private readonly IMapper _mapper;
        private readonly EmployeeService _service;

        public EmployeeController(EmployeeService employeeService, IMapper mapper,
            IValidator<EmploteeDtoForPost> validatorPost, IValidator<EmployeeContractDtoForPost> validatorContrPost,
            IValidator<EmployeeDtoForPut> validatorPut, IValidator<EmployeeContractDtoForPut> validatorContrPut,
            IValidator<EmployeeFilterDTO> validatorFilter, IValidator<EmployeeClientProfileDtoForPost> validatorClientProfile,
            IValidator<CurrencyDtoForPost> validatorCurrPost)
        {
            _validatorCurrPost = validatorCurrPost;
            _validatorClientProfile = validatorClientProfile;
            _validatorFilter = validatorFilter;
            _validatorContrPut = validatorContrPut;
            _validatorPut = validatorPut;
            _validatorContrPost = validatorContrPost;
            _validatorPost = validatorPost;
            _service = employeeService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid id)
        {
            var employee = await _service.GetAsync(id);

            if (employee == null)
                return NotFound();

            var employeeDto = _mapper.Map<EmployeeDtoForGet>(employee);

            return Ok(employeeDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmploteeDtoForPost dtoEmplyee)
        {
            var resultValidEmployee = _validatorPost.Validate(dtoEmplyee);
            var resultValidContract = _validatorContrPost.Validate(dtoEmplyee.ContractEmployee);

            if (!resultValidEmployee.IsValid)
                return BadRequest(resultValidEmployee.Errors);

            if (!resultValidContract.IsValid)
                return BadRequest(resultValidContract.Errors);

            var employee = _mapper.Map<Employee>(dtoEmplyee);

            if (employee == null)
                return BadRequest();

            var result = await _service.AddEmployeeAsync(employee);

            if (result == false)
                return BadRequest();

            var employeeGet = _mapper.Map<EmployeeDtoForGet>(employee);

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employeeGet);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, [FromBody] EmployeeDtoForPut dtoEmployee)
        {
            var resultValid = _validatorPut.Validate(dtoEmployee);

            if (!resultValid.IsValid)
                return BadRequest(resultValid.Errors);

            var result = await _service.UpdateEmployeeAsync(id, dtoEmployee);

            if (result == false)
                return BadRequest();

            return NoContent();
        }

        [HttpPut("{employeeId}/contract")]
        public async Task<IActionResult> UpdateContract([FromRoute] Guid employeeId, [FromBody] EmployeeContractDtoForPut dtoContract)
        {
            var resultValid = _validatorContrPut.Validate(dtoContract);

            if (!resultValid.IsValid)
                return BadRequest(resultValid.Errors);

            var result = await _service.UpdateEmployeeContractAsync(employeeId, dtoContract);

            if (result == false)
                return BadRequest();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _service.DeleteEmployeeAsync(id);

            if (result == false)
                return BadRequest();

            return Ok();
        }
        [HttpGet("search")]
        public async Task<IActionResult> GetByFilter([FromQuery] EmployeeFilterDTO filter,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var resultValid = _validatorFilter.Validate(filter);

            if (!resultValid.IsValid)
                return BadRequest(resultValid.Errors);

            var result = await _service.GetFilterEmployeeAsync(filter, page, pageSize);

            return Ok(result);

        }
        [HttpPost("{employeeId}/clientProfile")]
        public async Task<IActionResult> CreateClientProfile([FromRoute] Guid employeeId,
            [FromBody] EmployeeClientProfileDtoForPost dtoProfile)
        {
            var resultValidCurr = _validatorCurrPost.Validate(dtoProfile.Currency);
            var resultValid = _validatorClientProfile.Validate(dtoProfile);

            if (!resultValidCurr.IsValid)
                return BadRequest(resultValidCurr.Errors);

            if (!resultValid.IsValid)
                return BadRequest(resultValid.Errors);

            var currency = _mapper.Map<Currency>(dtoProfile.Currency);
            if (currency == null)
                return BadRequest(new { Message = "Валюта не может быть пустой" });

            var result = await _service.CreateAccountProfileAsync(employeeId, currency, dtoProfile.Email, dtoProfile.PhoneNumber);

            if (result == false)
                return BadRequest(new { Message = "Не удалось создать профиль" });

            var employee = await _service.GetAsync(employeeId);
            var profileId = employee.ClientProfile.Id;

            return CreatedAtAction(
                actionName: nameof(ClientController.GetClient),
                controllerName: "Client",
                routeValues: new { id = profileId },
                value: null);

        }
    }
}
