using BankSystem.App.DTOs.DTOsAccounts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Validators.AccountValidators
{
    public class AccountDtoForPutValidator : AbstractValidator<AccountDtoForPut>
    {
        public AccountDtoForPutValidator()
        {
            RuleFor(c => c.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("Сумма на счету не может быть отрицательной")
                .When(c => c.Amount.HasValue);
        }
    }
}
