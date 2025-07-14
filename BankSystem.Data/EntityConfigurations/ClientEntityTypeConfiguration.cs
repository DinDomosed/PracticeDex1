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
    public class ClientEntityTypeConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            //Указать таблицудля сущности Client
            builder.ToTable("Clients");

            //Указать св ва Persone
            builder.Property(c => c.Id)
                .HasColumnName("Id")
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(c => c.FullName)
                .HasColumnName("Full_Name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Birthday)
                .HasColumnName("Birthday")
                .HasColumnType("date")
                .IsRequired();
            builder.HasCheckConstraint("CK_Client_Age", "\"Birthday\" <= CURRENT_DATE - INTERVAL '18' year");

            builder.Property(b => b.Bonus)
                .HasColumnName("Bonus")
                .HasColumnType("decimal");
            // Указать специфические св ва client
            builder.HasIndex(c => c.Email)
                .IsUnique();
            builder.Property(c => c.Email)
                .HasColumnName("Email")
                .HasMaxLength(254)
                .IsRequired();

            builder.HasIndex(c => c.PhoneNumber)
                .IsUnique();
            builder.Property(c => c.PhoneNumber)
                .HasColumnName("Phone_Number")
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(c => c.PassportNumber)
                .IsUnique();
            builder.Property(c => c.PassportNumber)
                .HasColumnName("Passport_Number")
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(c => c.RegistrationDate)
                .HasColumnName("Registration_Date")
                .IsRequired()
                .HasColumnType("date");

            builder.HasKey(c => c.Id);
            // Указать связь с Account

            builder.HasMany(c => c.Accounts)
                .WithOne(a => a.Client)
                .HasForeignKey(a => a.IdClient)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

        }
    }
}
