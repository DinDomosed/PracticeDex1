using AutoMapper;
using BankSystem.App.Common;
using BankSystem.App.DTOs;
using BankSystem.App.DTOs.DTosForRequestsToControllersEmployee;
using BankSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankSystem.App.Mappings
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDtoForGet>()
                .ForMember(c => c.HasAClientProfile, opt => opt.MapFrom(src => src.ClientProfile != null));

            CreateMap<EmployeeContract, EmployeeContractDtoForGet>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => srcMember != null && srcMember != string.Empty));

            CreateMap<EmployeeContractDtoForPost, EmployeeContract>()
                .ConstructUsing(src => new EmployeeContract
                (src.StartOfWork,
                src.EndOfContract,
                src.Salary,
                src.Post));

            CreateMap<EmploteeDtoForPost, Employee>()
                .ConstructUsing((src, ctx) => new Employee(
                    src.FullName,
                    src.Birthday,
                    ctx.Mapper.Map<EmployeeContract>(src.ContractEmployee),
                    src.PassportNumber));

            CreateMap<EmployeeDtoForPut, Employee>()
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

            CreateMap<EmployeeContractDtoForPut, EmployeeContract>()
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

            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));
        }
    }
}
