using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Data.EntityConfigurations
{
    public class AccountEntityTypeConfigurations : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");

            builder.Property(c => c.Id)
                .HasColumnName("Id")
                .HasColumnType("uuid")
                .IsRequired()
                .ValueGeneratedNever();

            builder.HasIndex(c => c.AccountNumber)
                .IsUnique();
            builder.Property(c => c.AccountNumber)
                .HasColumnName("Account_Number")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(c => c.Amount)
                .HasColumnName("Amount")
                .HasColumnType("decimal")
                .IsRequired();

            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Client)
                .WithMany(c => c.Accounts)
                .HasForeignKey(c => c.IdClient)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(c => c.Currency)
                .WithMany(c => c.Accounts)
                .HasForeignKey(c => c.CurrencyCode)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

        }
    }
}
