using BankSystem.App.DTOs.DTosForRequestsToControllersEmployee;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Validators.EmployeeValidators
{
    public class EmployeeContractDtoForPutValidator : AbstractValidator<EmployeeContractDtoForPut>
    {
        public EmployeeContractDtoForPutValidator()
        {
            RuleFor(c => c.StartOfWork)
                .Must(date => date <= DateTime.Today)
                .WithMessage("Дата начала работы не может быть в будующем")
                .When(c => !c.Equals(default(DateTime)) || !string.IsNullOrWhiteSpace(c.StartOfWork.ToString()));

            RuleFor(c => c.Salary)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Зарплата не может быть отрицательной")
                .LessThanOrEqualTo(10000000).WithMessage("Зарпалата слишком большая")
                .When(c => !string.IsNullOrWhiteSpace(c.Salary.ToString()));

            RuleFor(c => c.Post)
                .MaximumLength(50)
                .WithMessage("Наименование должности не может быть длиннее 50 символов")
                .MinimumLength(2)
                .WithMessage("Наименование должности не может быть короче 2-х символов")
                .When(c => !string.IsNullOrWhiteSpace(c.Post));
        }
    }
}
