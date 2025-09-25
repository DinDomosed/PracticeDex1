using BankSystem.App.DTOs.DTosForRequestsToControllersEmployee;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Validators.EmployeeValidators
{
    public class EmployeeClientProfileDtoForPostValidator : AbstractValidator<EmployeeClientProfileDtoForPost>
    {
        public EmployeeClientProfileDtoForPostValidator()
        {
            RuleFor(c => c.Currency)
                .NotEmpty()
                .WithMessage("Валюта для счета обязательна");

            RuleFor(c => c.Email)
                .EmailAddress()
                .WithMessage("Некорректный формат email")
                .NotEmpty()
                .WithMessage("Email обязателен");

            RuleFor(c => c.PhoneNumber)
                .NotEmpty()
                .WithMessage("Номер телефона обязателен");
        }
    }
}
