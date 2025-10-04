using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.DTOs.DTOsForRequestsToControllers;
using FluentValidation;

namespace BankSystem.App.Validators.ClientValidators
{
    public class ClientDtoForPutValidator : AbstractValidator<ClientDtoForPut>
    {
        public ClientDtoForPutValidator()
        {
            RuleFor(c => c.FullName)
                .NotEmpty()
                .WithMessage("ФИО обязательно")
                .MaximumLength(100)
                .WithMessage("ФИО не может превышать 100 символов")
                .When(c => !string.IsNullOrWhiteSpace(c.FullName));

            RuleFor(c => c.Email)
                .NotEmpty()
                .WithMessage("Email обязателен")
                .EmailAddress()
                .WithMessage("Email некорректный")
                .MaximumLength(254).WithMessage("Email слишком длинный")
                .When(c => !string.IsNullOrWhiteSpace(c.Email));

            RuleFor(c => c.PhoneNumber)
                .NotEmpty()
                .WithMessage("Номер телефона обязателен")
                .Matches(@"^\+?\d[\d\s\(\)-]{8,16}\d$")
                .WithMessage("Номер телефона введен некорректно")
                .When(c => !string.IsNullOrWhiteSpace(c.PhoneNumber));

            RuleFor(c => c.PassportNumber)
                .MinimumLength(6)
                .WithMessage("Слишком короткий номер паспорта")
                .MaximumLength(20)
                .WithMessage("Слишком длинный номер паспорта")
                .NotEmpty()
                .When(c => !string.IsNullOrWhiteSpace(c.PassportNumber));
        }
    }
}
