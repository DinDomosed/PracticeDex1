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

        public decimal? Bonus { get; private set; }
        public virtual void AddBonus(decimal amount)
        {
            Bonus += amount;
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

        public override bool Equals(object? obj)
        {
            if(obj == null) 
                return false;
            if(obj is not Person person) 
                return false;

            return  Id == person.Id && FullName == person.FullName && Birthday == person.Birthday;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FullName, Birthday);
        }
    }
}
