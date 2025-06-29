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
        public Client(string FullName, DateTime birthday, string email, string phoneNumber, string passportNumber, Account? account = null) : base(FullName, birthday)
        {
            Email = email;
            PhoneNumber = phoneNumber;
            PassportNumber = passportNumber;
            RegistrationDate = DateTime.Now;
            if (account != null)
            {
                Accounts.Add(account);
            }
        }
        public Client(Guid Id, string FullName, DateTime birthday, string email, string phoneNumber, string passportNumber, Account? account = null) : base(Id, FullName, birthday)
        {
            Email = email;
            PhoneNumber = phoneNumber;
            PassportNumber = passportNumber;
            RegistrationDate = DateTime.Now;
            if (account != null)
            {
                Accounts.Add(account);
            }
        }
        public override string ToString()
        {
            return $"Имя клииента: {FullName}\n" +
                $"Дата рождения: {Birthday:d}\n" +
                $"Возраст: {Age} лет\n" +
                $"Почта: {Email}\n" +
                $"Номер телефона {PhoneNumber}\n" +
                $"Серия и номер паспорта: {PassportNumber}\n" +
                $"Дата регистрации: {RegistrationDate:d}\n" +
                $"Колличество счетов {Accounts.Count}\n\n" +
                $"ID клиента: {Id}";
        }
        public override bool Equals(object? obj)
        {
            if (obj is not Client client || obj == null)
                return false;

            return FullName == client.FullName && Birthday == client.Birthday &&
                Email == client.Email && PhoneNumber == client.PhoneNumber &&
                PassportNumber == client.PassportNumber && 
                Accounts.Count == client.Accounts.Count &&
                Id == client.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FullName, Birthday, Email, PhoneNumber, PassportNumber, Id);
        }
    }
}
