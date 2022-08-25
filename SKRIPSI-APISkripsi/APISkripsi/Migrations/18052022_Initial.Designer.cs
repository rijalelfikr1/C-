﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using APISkripsi.Function;

namespace APISkripsi.Migrations
{
    [DbContext(typeof(DatabaseAccessLayer))]
    [Migration("18052022_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("VerifyEmailForgotPasswordTutorial.Models.User", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                b.Property<string>("Email")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<byte[]>("PasswordHash")
                    .IsRequired()
                    .HasColumnType("varbinary(max)");

                b.Property<string>("PasswordResetToken")
                    .HasColumnType("nvarchar(max)");

                b.Property<byte[]>("PasswordSalt")
                    .IsRequired()
                    .HasColumnType("varbinary(max)");

                b.Property<DateTime?>("ResetTokenExpires")
                    .HasColumnType("datetime2");

                b.Property<string>("VerificationToken")
                    .HasColumnType("nvarchar(max)");

                b.Property<DateTime?>("VerifiedAt")
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.ToTable("Users");
            });
#pragma warning restore 612, 618
        }
    }
}
