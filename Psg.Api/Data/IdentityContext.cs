using Microsoft.EntityFrameworkCore;
using Psg.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psg.Api.Data
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        { }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Kisi> Kisiler { get; set; }
        public DbSet<Cinsiyet> Cinsiyetler { get; set; }
        public DbSet<Foto> Fotograflar { get; set; }
        public DbSet<ArkadaslikTeklif> ArkadaslikTeklifleri { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Polisomnografi");
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Kullanici>(entity =>
            {
                entity.ToTable("Kullanicilar");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.KisiBilgisi).WithMany(h => h.Kullanicilar).HasForeignKey(fk => fk.KisiNo);

            });
            modelBuilder.Entity<Foto>(entity =>
            {
                entity.ToTable("KullaniciFotograflari");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Kullanici).WithMany(h => h.Fotograflari).HasForeignKey(fk => fk.KullaniciNo);

            });
            modelBuilder.Entity<Kisi>(entity =>
            {
                entity.ToTable("Kisiler");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Cinsiyeti).WithMany(h => h.Kisiler).HasForeignKey(fk => fk.CinsiyetNo);

            });
            modelBuilder.Entity<Cinsiyet>(entity =>
            {
                entity.ToTable("Cinsiyetler");
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<ArkadaslikTeklif>(entity =>
            {
                entity.ToTable("ArkadaslikTeklifleri");
                entity.HasKey(e => new { e.ArkadaslikIsteyenNo, e.TeklifEdilenNo });
                entity.HasOne(e => e.ArkadaslikIsteyen).WithMany(m => m.GelenTeklifler).HasForeignKey(fk => fk.TeklifEdilenNo).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.TeklifEdilen).WithMany(m => m.YapilanTeklifler).HasForeignKey(fk => fk.ArkadaslikIsteyenNo).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
