using BankSystem.App.DTOs.DTosForRequestsToControllersEmployee;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Validators.EmployeeValidators
{
    public class EmploteeDtoForPostValidator : AbstractValidator<EmploteeDtoForPost>
    {
        public EmploteeDtoForPostValidator()
        {
            RuleFor(c => c.FullName)
                .NotNull()
                .WithMessage("ФИО обязательно")
                .MaximumLength(100)
                .WithMessage("ФИО слишком длинное");

            RuleFor(c => c.Birthday)
                .Must(date => date <= DateTime.Today.AddYears(-18))
                .WithMessage("Сотруднику должно быть не менее 18 лет")
                .NotEmpty()
                .WithMessage("Дата рождения обязательна");

            RuleFor(c => c.PassportNumber)
                .MinimumLength(6)
                .WithMessage("Слишком короткий номер паспорта")
                .MaximumLength(20)
                .WithMessage("Слишком длинный номер паспорта")
                .NotEmpty()
                .WithMessage("Серия и номер паспорта обязательны");

            RuleFor(c => c.ContractEmployee)
                .NotNull()
                .WithMessage("Контракт обязателен");
        }
    }
}
