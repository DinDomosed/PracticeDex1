using AutoMapper;
using BankSystem.App.Common;
using BankSystem.App.DTOs.DTOsForRequestsToControllers;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.App.Mappings
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientDtoForGet>();

            CreateMap<ClientDtoForPost, Client>()
                .ConstructUsing(src => new Client(
                    src.FullName,
                    src.Birthday,
                    src.Email,
                    src.PhoneNumber,
                    src.PassportNumber,
                    null));

            CreateMap<ClientDtoForPut, Client>()
                .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember, destMember) =>
                {
                    if (srcMember == null)
                        return false;

                    if (srcMember is string s && string.IsNullOrWhiteSpace(s))
                        return false;

                    if (srcMember != null && srcMember.Equals(destMember))
                        return false;

                    return true;

                }));

            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));
        }
    }
}
