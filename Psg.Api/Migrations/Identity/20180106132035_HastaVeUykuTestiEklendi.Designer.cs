﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Psg.Api.Data;
using System;

namespace Psg.Api.Migrations.Identity
{
    [DbContext(typeof(DataContext))]
    [Migration("20180106132035_HastaVeUykuTestiEklendi")]
    partial class HastaVeUykuTestiEklendi
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Psg.Api.Models.Hasta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Ad");

                    b.Property<DateTime>("DogumTarihi");

                    b.Property<string>("Soyad");

                    b.HasKey("Id");

                    b.ToTable("Hastalar");
                });

            modelBuilder.Entity("Psg.Api.Models.Kullanici", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("PassswordSalt");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Kullanicilar");
                });

            modelBuilder.Entity("Psg.Api.Models.UykuTest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Ahi");

                    b.Property<int?>("HastaId");

                    b.Property<int>("HastaNo");

                    b.Property<double>("St90");

                    b.Property<DateTime?>("Tarih");

                    b.HasKey("Id");

                    b.HasIndex("HastaId");

                    b.ToTable("UykuTestleri");
                });

            modelBuilder.Entity("Psg.Api.Models.UykuTest", b =>
                {
                    b.HasOne("Psg.Api.Models.Hasta", "Hasta")
                        .WithMany("UykuTestleri")
                        .HasForeignKey("HastaId");
                });
#pragma warning restore 612, 618
        }
    }
}
