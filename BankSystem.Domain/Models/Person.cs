using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Domain.Models
{
    public class Person
    {
        public Guid Id { get; private set; }
        public string FullName { get; private set; }
        public DateTime Birthday { get; private set; }
        public int Age
        {
            get
            {
                DateTime dateNow = DateTime.Now;
                int age = dateNow.Year - Birthday.Year;

                if (Birthday.Date > dateNow.AddYears(-age))
                    age--;
                return age;
            }  
        }
        public Person(string FullName, DateTime Birthday)
        {
            this.FullName = FullName;
            this.Birthday = Birthday;
            Id = Guid.NewGuid();
        }      
        
        public Person(Guid Id, string FullName, DateTime Birthday )
        {
            this.Id = Id;
            this.FullName = FullName;
            this.Birthday = Birthday;
        }
    }
}
