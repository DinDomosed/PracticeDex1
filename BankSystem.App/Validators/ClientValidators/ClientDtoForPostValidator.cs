using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.App.DTOs.DTOsForRequestsToControllers;
using FluentValidation;

namespace BankSystem.App.Validators.ClientValidators
{
    public class ClientDtoForPostValidator : AbstractValidator<ClientDtoForPost>
    {
        public ClientDtoForPostValidator()
        {
            RuleFor(c => c.FullName)
                .NotEmpty()
                .WithMessage("ФИО обязательно")
                .MaximumLength(100)
                .WithMessage("ФИО слишком длинное");

            RuleFor(c => c.Birthday)
                .NotEmpty()
                .WithMessage("Дата рождения обязательна")

                .Must(date => date <= DateTime.Today)
                .WithMessage("Дата рождения не может быть в будующем")

                .Must(date => date <= DateTime.Today.AddYears(-18))
                .WithMessage("Клиенту должно быть не менее 18 лет");

            RuleFor(c => c.Email)
                .NotEmpty()
                .WithMessage("Email обязателен")

                .EmailAddress()
                .WithMessage("Email некорректный")

                .MaximumLength(254)
                .WithMessage("Email слишком длинный");

            RuleFor(c => c.PhoneNumber)
                .NotEmpty()
                .WithMessage("Номер телефона обязателен");

            RuleFor(c => c.PassportNumber)
                .MinimumLength(6)
                .WithMessage("Слишком короткий номер паспорта")
                .MaximumLength(20)
                .WithMessage("Слишком длинный номер паспорта")
                .NotEmpty()
                .WithMessage("Серия и номер паспорта обязательны");
        }
    }
}
