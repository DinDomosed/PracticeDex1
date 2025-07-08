using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography.X509Certificates;

namespace BankSystem.Data.EntityConfigurations
{
    public class CurrencyEntityTypeConfigurations : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("Currencies");

            builder.HasIndex(c => c.Code)
                .IsUnique();
            builder.Property(c => c.Code)
                .IsRequired()
                .HasColumnName("Code")
                .HasMaxLength(3);

            builder.Property(c => c.Symbol)
                .IsRequired()
                .HasColumnName("Symbol")
                .HasMaxLength(1);

            builder.HasKey(c => c.Code);

            builder.HasMany(c => c.Accounts)
                .WithOne(a => a.Currency)
                .HasForeignKey(a => a.CurrencyCode)
                .IsRequired();
                
        }
    }
}
