using AutoMapper;
using BankSystem.App.DTOs.DTOsAccounts;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Mappings
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<CurrencyDtoForPost, Currency>()
                .ConstructUsing(c => new Currency(c.Code, c.Symbol[0]));

            CreateMap<CurrencyDtoForPut, Currency>()
               .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) =>
               {
                   if (srcMember is string s && string.IsNullOrWhiteSpace(s))
                       return false;
                   if (srcMember != null && srcMember.Equals(destMember))
                       return false;
                   if (srcMember == null)
                       return false;
                   if (srcMember is char c && c.Equals(default(char)))
                       return false;
                   


                   return true;
               }));

            CreateMap<AccountDTOForPost, Account>()
                .ConstructUsing((src, ctx) => new Account(
                    src.IdClient,
                    ctx.Mapper.Map<Currency>(src.Currency),
                    src.Amount ?? 0
                    ));

            CreateMap<AccountDtoForPut, Account>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) =>
                {
                    if (srcMember is string s && string.IsNullOrWhiteSpace(s))
                        return false;

                    if (srcMember != null && srcMember.Equals(destMember))
                        return false;

                    if (srcMember == null)
                        return false;

                    return true;
                }));

            CreateMap<Account, AccountDtoForGet>()
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) =>
                {
                    if (srcMember is string s && string.IsNullOrWhiteSpace(s))
                        return false;

                    if (srcMember != null && srcMember.Equals(destMember))
                        return false;

                    if (srcMember == null)
                        return false;

                    return true;
                }));

        }
    }
}
