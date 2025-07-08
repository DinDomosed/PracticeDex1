using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Data.EntityConfigurations
{
    public class EmployeeContractEntityTypeConfigurations : IEntityTypeConfiguration<EmployeeContract>
    {
        public void Configure(EntityTypeBuilder<EmployeeContract> builder)
        {
            builder.ToTable("Employee_Contracts");
            builder.Property(c => c.Id)
                .HasColumnName("Id")
                .HasColumnType("uuid")
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(c => c.StartOfWork)
                .HasColumnName("start_of_work")
                .HasColumnType("date")
                .IsRequired();

            builder.Property(c => c.EndOfContract)
                .HasColumnName("end_of_contract")
                .HasColumnType("date")
                .IsRequired(false);

            builder.Property(c => c.Salary)
                .HasColumnName("salary")
                .HasColumnType("decimal(12, 2)")
                .IsRequired();

            builder.Property(c => c.Post)
                 .HasColumnName("post")
                .HasMaxLength(50)
                .IsRequired();

            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Employee)
                .WithOne(e => e.ContractEmployee)
                .HasForeignKey<EmployeeContract>(c => c.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(); 
        }
    }
}
