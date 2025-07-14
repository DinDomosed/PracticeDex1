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
    public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.Property(c => c.Id)
                .HasColumnName("Id")
                .HasColumnType("uuid")
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
            builder.HasCheckConstraint("CK_Employee_Age", "\"Birthday\" <= CURRENT_DATE - INTERVAL '18' year");

            builder.Property(c => c.Bonus)
                .HasColumnName("Bonus")
                .HasColumnType("decimal");

            builder.Property(c => c.PassportNumber)
                .HasColumnName("Passport_Number")
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(c => c.PassportNumber)
                .IsUnique();


            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.ClientProfile)
                 .WithOne(c => c.EmployeeProfile)
                 .HasForeignKey<Employee>(c => c.ClientId)
                 .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
