using BankSystem.App.DTOs.DTosForRequestsToControllersEmployee;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Validators.EmployeeValidators
{
    public class EmployeeContractDtoForPostValidator : AbstractValidator<EmployeeContractDtoForPost>
    {
        public EmployeeContractDtoForPostValidator()
        {
            RuleFor(c => c.StartOfWork)
                .Must(date => date <= DateTime.Today)
                .WithMessage("Дата трудоустройства не может быть в будующем");

            RuleFor(c => c.Post)
                .NotEmpty()
                .WithMessage("Должность обязательна")
                .MaximumLength(50)
                .WithMessage("Наименование должности не может быть длиннее 50 символов")
                .MinimumLength(2)
                .WithMessage("Наименование должности не может быть короче 2-х символов");

            RuleFor(c => c.Salary)
                .GreaterThanOrEqualTo(0).WithMessage("Зарплата не может быть отрицательной")
                .LessThanOrEqualTo(10000000).WithMessage("Зарпалата слишком большая");

        }
    }
}
