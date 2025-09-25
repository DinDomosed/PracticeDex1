using BankSystem.App.DTOs.DTosForRequestsToControllersEmployee;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Validators.EmployeeValidators
{
    public class EmployeeFilterDtoValidator : AbstractValidator<EmployeeFilterDTO>
    {
        public EmployeeFilterDtoValidator()
        {
            RuleFor(c => c.FullName)
                .MaximumLength(100).WithMessage("ФИО слишком длинное")
                .When(c => !string.IsNullOrWhiteSpace(c.FullName));

            RuleFor(c => c.PassportNumber)
                .MaximumLength(20).WithMessage("Слишком длинный номер паспорта")
                .When(c => !string.IsNullOrWhiteSpace(c.PassportNumber));

            RuleFor(c => c.SalaryFrom)
                .GreaterThanOrEqualTo(0).WithMessage("Зарплата не может быть отрицательной")
                .LessThanOrEqualTo(10000000).WithMessage("Зарпалата слишком большая")
                .When(c => !string.IsNullOrWhiteSpace(c.SalaryFrom.ToString()));

            RuleFor(c => c.SalaryTo)
                .GreaterThanOrEqualTo(0).WithMessage("Зарплата не может быть отрицательной")
                .LessThanOrEqualTo(10000000).WithMessage("Зарпалата слишком большая")
                .When(c => !string.IsNullOrWhiteSpace(c.SalaryTo.ToString()));

            RuleFor(c => c.Post)
                .MaximumLength(50)
                .WithMessage("Наименование должности не может быть длиннее 50 символов")
                .When(c => !string.IsNullOrWhiteSpace(c.Post));

            RuleFor(c => c.Bonus)
                .GreaterThanOrEqualTo(0).WithMessage("Бонус не может быть отрицательным")
                .LessThanOrEqualTo(1000000).WithMessage("Слишком большой бонус")
                .When(c => !string.IsNullOrWhiteSpace(c.Bonus.ToString()));
        }
    }
}
