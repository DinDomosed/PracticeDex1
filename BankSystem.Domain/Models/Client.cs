using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class Client : Person
    {
        public List<Account> Accounts { get; private set; } = new List<Account>();
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string PassportNumber { get; private set; }
        public DateTime RegistrationDate { get; private set; }
        public Client(string FullName, DateTime birthday, string email, string phoneNumber, string passportNumber) : base(FullName, birthday)
        {
            Email = email;
            PhoneNumber = phoneNumber;
            PassportNumber = passportNumber;
            RegistrationDate = DateTime.Now;
        }
        public Client(Guid Id, string FullName, DateTime birthday, string email, string phoneNumber, string passportNumber) : base(Id, FullName, birthday)
        {
            Email = email;
            PhoneNumber = phoneNumber;
            PassportNumber = passportNumber;
            RegistrationDate = DateTime.Now;
        }
        public override string ToString()
        {
            return $"Имя клииента: {FullName}\n" +
                $"Дата рождения: {Birthday:d}\n" +
                $"Возраст: {Age} лет\n" +
                $"Почта: {Email}\n" +
                $"Номер телефона {PhoneNumber}\n" +
                $"Серия и номер паспорта: {PassportNumber}\n" +
                $"Дата регистрации: {RegistrationDate:d}\n\n" +
                $"ID клиента: {Id}";
        }
    }
}
