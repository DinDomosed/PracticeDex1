using BankSystem.App;
using BankSystem.App.DTOs;
using BankSystem.Domain.Models;
using CsvHelper;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExportTool.Maps
{
    public class ClientCsvDtoMap : ClassMap<ClientCsvDto>
    {
        public ClientCsvDtoMap()
        {
            Map(c => c.Id).Name("ID").Index(0);
            Map(c => c.FullName).Name("ФИО").Index(1);
            Map(c => c.Birthday).Name("Дата рождения").Index(2);
            Map(c => c.Email).Name("Почта").Index(3);
            Map(c => c.PhoneNumber).Name("Номер телефона").Index(4);
            Map(c => c.RegistrationDate).Name("Дата регистрации").Index(5);
            Map(c => c.Bonus).Name("Бонусы").Index(6);
            Map(c => c.PassportNumber).Name("Серия и номер паспорта").Index(7);
        }
    }
}
