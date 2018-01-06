using Microsoft.EntityFrameworkCore;
using Psg.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psg.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Hasta> Hastalar { get; set; }
        public DbSet<UykuTest> UykuTestleri { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UykuTest>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Hasta).WithMany(h => h.UykuTestleri).HasForeignKey(fk => fk.HastaNo);

            });
        }
    }
}
