﻿// <auto-generated />
using System;
using MaxiOrders.Back.Infrastructure.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MaxiOrders.Back.Infrastructure.Model.Migrations
{
    [DbContext(typeof(DBMaxiOrdersContext))]
    [Migration("20190826210513_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MaxiOrders.Back.Domain.Entities.Company", b =>
                {
                    b.Property<long>("IdCompany")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("IdCompany");

                    b.ToTable("Company","Companies");
                });

            modelBuilder.Entity("MaxiOrders.Back.Domain.Entities.Device", b =>
                {
                    b.Property<long>("IdDevice")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BillImage")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("DataSheets")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<long>("IdHeadQuarter");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("InstalationDate")
                        .HasColumnType("date");

                    b.Property<string>("LicenseNumber")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("ManufacturingDate")
                        .HasColumnType("date");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("date");

                    b.Property<string>("Serie")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("State");

                    b.HasKey("IdDevice");

                    b.HasIndex("IdHeadQuarter");

                    b.ToTable("Device","Masters");
                });

            modelBuilder.Entity("MaxiOrders.Back.Domain.Entities.HeadQuarter", b =>
                {
                    b.Property<long>("IdHeadQuarter")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<long>("IdCompany");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("IdHeadQuarter");

                    b.HasIndex("IdCompany");

                    b.ToTable("HeadQuarter","Companies");
                });

            modelBuilder.Entity("MaxiOrders.Back.Domain.Entities.User", b =>
                {
                    b.Property<long>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DigitalSignature")
                        .HasMaxLength(50);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false);

                    b.Property<string>("Identification")
                        .IsRequired()
                        .HasMaxLength(15)
                        .IsUnicode(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(500)
                        .IsUnicode(false);

                    b.Property<string>("Photo")
                        .HasMaxLength(50);

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("IdUser");

                    b.ToTable("User","Users");
                });

            modelBuilder.Entity("MaxiOrders.Back.Domain.Entities.Device", b =>
                {
                    b.HasOne("MaxiOrders.Back.Domain.Entities.HeadQuarter", "IdHeadQuarterNavigation")
                        .WithMany("Device")
                        .HasForeignKey("IdHeadQuarter")
                        .HasConstraintName("FK_Device_HeadQuarter");
                });

            modelBuilder.Entity("MaxiOrders.Back.Domain.Entities.HeadQuarter", b =>
                {
                    b.HasOne("MaxiOrders.Back.Domain.Entities.Company", "IdCompanyNavigation")
                        .WithMany("HeadQuarter")
                        .HasForeignKey("IdCompany")
                        .HasConstraintName("FK_HeadQuarter_Company");
                });
#pragma warning restore 612, 618
        }
    }
}