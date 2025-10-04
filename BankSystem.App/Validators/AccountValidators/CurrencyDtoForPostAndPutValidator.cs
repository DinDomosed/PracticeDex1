using BankSystem.App.DTOs.DTOsAccounts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Validators.AccountValidators
{
    public class CurrencyDtoForPostAndPutValidator : AbstractValidator<CurrencyDtoForPost>
    {
        public CurrencyDtoForPostAndPutValidator()
        {
            RuleFor(c => c.Code)
                .MaximumLength(3)
                .WithMessage("Слишком длинный валютный код")
                .NotEmpty()
                .WithMessage("Код валюты обязателен");

            RuleFor(c => c.Symbol)
                .NotEmpty()
                .WithMessage("Символ валюты обязателен")
                .MaximumLength(1).WithMessage("Сликшом длинный символ");
                
        }
    }
}
