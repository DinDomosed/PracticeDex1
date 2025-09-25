using BankSystem.App.DTOs.DTosForRequestsToControllersEmployee;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Validators.EmployeeValidators
{
    public class EmployeeDtoForPutValidation : AbstractValidator<EmployeeDtoForPut>
    {
        public EmployeeDtoForPutValidation()
        {
            RuleFor(c => c.FullName)
                .MaximumLength(100)
                .WithMessage("ФИО слишком длинное")
                .When(c => !string.IsNullOrWhiteSpace(c.FullName));

            RuleFor(c => c.PassportNumber)
                .MinimumLength(6)
                .WithMessage("Слишком короткий номер паспорта")
                .MaximumLength(20)
                .WithMessage("Слишком длинный номер паспорта")
                .When(c => !string.IsNullOrWhiteSpace(c.PassportNumber));
        }
    }
}
