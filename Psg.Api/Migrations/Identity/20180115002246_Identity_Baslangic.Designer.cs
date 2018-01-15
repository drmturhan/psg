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
    [DbContext(typeof(IdentityContext))]
    [Migration("20180115002246_Identity_Baslangic")]
    partial class Identity_Baslangic
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Polisomnografi")
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Psg.Api.Models.Cinsiyet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CinsiyetAdi");

                    b.HasKey("Id");

                    b.ToTable("Cinsiyetler");
                });

            modelBuilder.Entity("Psg.Api.Models.Foto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Aciklama");

                    b.Property<DateTime>("EklenmeTarihi");

                    b.Property<bool>("IlkTercihmi");

                    b.Property<int>("KullaniciNo");

                    b.Property<string>("PublicId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("KullaniciNo");

                    b.ToTable("KullaniciFotograflari");
                });

            modelBuilder.Entity("Psg.Api.Models.Kisi", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Ad");

                    b.Property<int>("CinsiyetNo");

                    b.Property<string>("DigerAd");

                    b.Property<DateTime>("DogumTarihi");

                    b.Property<string>("Soyad");

                    b.Property<string>("Unvan");

                    b.HasKey("Id");

                    b.HasIndex("CinsiyetNo");

                    b.ToTable("Kisiler");
                });

            modelBuilder.Entity("Psg.Api.Models.Kullanici", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Aktif");

                    b.Property<string>("EPosta");

                    b.Property<bool?>("EpostaOnaylandi");

                    b.Property<int>("KisiNo");

                    b.Property<string>("KullaniciAdi");

                    b.Property<byte[]>("SifreHash");

                    b.Property<byte[]>("SifreSalt");

                    b.Property<DateTime?>("SonAktifOlma");

                    b.Property<string>("TelefonNumarasi");

                    b.Property<bool?>("TelefonOnaylandi");

                    b.Property<DateTime>("YaratilmaTarihi");

                    b.HasKey("Id");

                    b.HasIndex("KisiNo");

                    b.ToTable("Kullanicilar");
                });

            modelBuilder.Entity("Psg.Api.Models.Foto", b =>
                {
                    b.HasOne("Psg.Api.Models.Kullanici", "Kullanici")
                        .WithMany("Fotograflari")
                        .HasForeignKey("KullaniciNo")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Psg.Api.Models.Kisi", b =>
                {
                    b.HasOne("Psg.Api.Models.Cinsiyet", "Cinsiyeti")
                        .WithMany("Kisiler")
                        .HasForeignKey("CinsiyetNo")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Psg.Api.Models.Kullanici", b =>
                {
                    b.HasOne("Psg.Api.Models.Kisi", "KisiBilgisi")
                        .WithMany("Kullanicilar")
                        .HasForeignKey("KisiNo")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}