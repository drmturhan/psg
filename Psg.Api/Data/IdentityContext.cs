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
        public DbSet<Foto> Fotograflar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Polisomnografi");
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Foto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Kullanici).WithMany(h => h.Fotograflari).HasForeignKey(fk => fk.KullaniciNo).OnDelete(DeleteBehavior.Cascade);

            });
        }
    }
}
