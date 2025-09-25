using BankSystem.App.DTOs.DTOsAccounts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Validators.AccountValidators
{
    public class CurrencyDtoForPutValidator : AbstractValidator<CurrencyDtoForPut>
    {
        public CurrencyDtoForPutValidator()
        {
            RuleFor(c => c.Code)
                .MaximumLength(4)
                .WithMessage("Слишком длинный валютный код")
                .When(c => !string.IsNullOrWhiteSpace(c.Code));

            RuleFor(c => c.Symbol)
                .NotEmpty()
                .WithMessage("Символ валюты обязателен")
            .When(c => !string.IsNullOrWhiteSpace(c.Code));
        }
    }
}
