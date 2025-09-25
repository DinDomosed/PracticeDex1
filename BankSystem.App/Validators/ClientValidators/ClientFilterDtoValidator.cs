using BankSystem.App.DTOs.DTOsForRequestsToControllers;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Validators.ClientValidators
{
    public class ClientFilterDtoValidator : AbstractValidator<ClientFilterDTO>
    {
        public ClientFilterDtoValidator()
        {
            RuleFor(c => c.FullName)
                .MaximumLength(100).WithMessage("ФИО слишком длинное")
                .When(c => !string.IsNullOrWhiteSpace(c.FullName));

            RuleFor(c => c.Bonus)
                .GreaterThanOrEqualTo(0).WithMessage("Бонус не может быть отрицательным")
                .LessThanOrEqualTo(1000000).WithMessage("Слишком большой бонус")
                .When(c => !string.IsNullOrWhiteSpace(c.Bonus.ToString()));

            RuleFor(c => c.CountAccounts)
                .GreaterThanOrEqualTo(0).WithMessage("Колличество счетов не может быть отрицательным")
                .LessThanOrEqualTo(1000).WithMessage("Сликшом большое колличество счетов")
                .When(c => !string.IsNullOrWhiteSpace(c.CountAccounts.ToString()));

            RuleFor(c => c.Email)
                .MaximumLength(254).WithMessage("Email слишком длинный")
                .When(c => !string.IsNullOrWhiteSpace(c.Email));


            RuleFor(c => c.PassportNumber)
               .MinimumLength(6)
               .WithMessage("Слишком короткий номер паспорта")
               .MaximumLength(20)
               .WithMessage("Слишком длинный номер паспорта")
               .When(c => !string.IsNullOrWhiteSpace(c.PassportNumber));
        }
    }
}
