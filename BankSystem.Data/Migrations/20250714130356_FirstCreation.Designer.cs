﻿// <auto-generated />
using System;
using BankSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BankSystem.Data.Migrations
{
    [DbContext(typeof(BankSystemDbContext))]
    [Migration("20250714130356_FirstCreation")]
    partial class FirstCreation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BankSystem.Domain.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("Account_Number");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal")
                        .HasColumnName("Amount");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("character varying(3)");

                    b.Property<Guid>("IdClient")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AccountNumber")
                        .IsUnique();

                    b.HasIndex("CurrencyCode");

                    b.HasIndex("IdClient");

                    b.ToTable("Accounts", (string)null);
                });

            modelBuilder.Entity("BankSystem.Domain.Models.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("date")
                        .HasColumnName("Birthday");

                    b.Property<decimal?>("Bonus")
                        .HasColumnType("decimal")
                        .HasColumnName("Bonus");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("character varying(254)")
                        .HasColumnName("Email");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("Full_Name");

                    b.Property<string>("PassportNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("Passport_Number");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("Phone_Number");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("date")
                        .HasColumnName("Registration_Date");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("PassportNumber")
                        .IsUnique();

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.ToTable("Clients", null, t =>
                        {
                            t.HasCheckConstraint("CK_Client_Age", "\"Birthday\" <= CURRENT_DATE - INTERVAL '18' year");
                        });
                });

            modelBuilder.Entity("BankSystem.Domain.Models.Currency", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("Code");

                    b.Property<char>("Symbol")
                        .HasMaxLength(1)
                        .HasColumnType("character(1)")
                        .HasColumnName("Symbol");

                    b.HasKey("Code");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Currencies", (string)null);
                });

            modelBuilder.Entity("BankSystem.Domain.Models.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("date")
                        .HasColumnName("Birthday");

                    b.Property<decimal?>("Bonus")
                        .HasColumnType("decimal")
                        .HasColumnName("Bonus");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("Full_Name");

                    b.Property<string>("PassportNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("Passport_Number");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.HasIndex("PassportNumber")
                        .IsUnique();

                    b.ToTable("Employees", null, t =>
                        {
                            t.HasCheckConstraint("CK_Employee_Age", "\"Birthday\" <= CURRENT_DATE - INTERVAL '18' year");
                        });
                });

            modelBuilder.Entity("BankSystem.Domain.Models.EmployeeContract", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("EndOfContract")
                        .HasColumnType("date")
                        .HasColumnName("end_of_contract");

                    b.Property<string>("Post")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("post");

                    b.Property<decimal>("Salary")
                        .HasColumnType("decimal(12, 2)")
                        .HasColumnName("salary");

                    b.Property<DateTime>("StartOfWork")
                        .HasColumnType("date")
                        .HasColumnName("start_of_work");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId")
                        .IsUnique();

                    b.ToTable("Employee_Contracts", (string)null);
                });

            modelBuilder.Entity("BankSystem.Domain.Models.Account", b =>
                {
                    b.HasOne("BankSystem.Domain.Models.Currency", "Currency")
                        .WithMany("Accounts")
                        .HasForeignKey("CurrencyCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BankSystem.Domain.Models.Client", "Client")
                        .WithMany("Accounts")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("BankSystem.Domain.Models.Employee", b =>
                {
                    b.HasOne("BankSystem.Domain.Models.Client", "ClientProfile")
                        .WithOne("EmployeeProfile")
                        .HasForeignKey("BankSystem.Domain.Models.Employee", "ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientProfile");
                });

            modelBuilder.Entity("BankSystem.Domain.Models.EmployeeContract", b =>
                {
                    b.HasOne("BankSystem.Domain.Models.Employee", "Employee")
                        .WithOne("ContractEmployee")
                        .HasForeignKey("BankSystem.Domain.Models.EmployeeContract", "EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("BankSystem.Domain.Models.Client", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("EmployeeProfile");
                });

            modelBuilder.Entity("BankSystem.Domain.Models.Currency", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("BankSystem.Domain.Models.Employee", b =>
                {
                    b.Navigation("ContractEmployee")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
