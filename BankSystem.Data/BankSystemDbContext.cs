using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

using BankSystem.Data.EntityConfigurations;

namespace BankSystem.Data
{
    public class BankSystemDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<EmployeeContract> EmployeeContracts { get; set; } = null!;
        public DbSet<Currency> Currencies { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder dbOptionsBuilder)
        {
            dbOptionsBuilder.UseNpgsql(
                "Host=localhost;" +
                "Port=5432;" +
                "Database=dbBankSystem;" +
                "Username=postgres;" +
                "Password=Diana123");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AccountEntityTypeConfigurations());
            modelBuilder.ApplyConfiguration(new EmployeeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeContractEntityTypeConfigurations());
            modelBuilder.ApplyConfiguration(new CurrencyEntityTypeConfigurations());
        }
    }
}
