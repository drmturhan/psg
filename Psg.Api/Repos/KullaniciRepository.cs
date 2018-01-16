using Microsoft.EntityFrameworkCore;
using Psg.Api.Base;
using Psg.Api.Data;
using Psg.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psg.Api.Repos
{
    public interface IKullaniciRepository : IRepository
    {
        Task<Kullanici> BulAsync(int id);
        Task<IEnumerable<Kullanici>> ListeGetirKullanicilarTumuAsync();
        Task<Foto> FotografBulAsync(int id);
        Task<Foto> KullanicininAsilFotosunuGetirAsync(int kullaniciNo);
    }

    public class KullaniciRepository : IKullaniciRepository
    {
        private readonly IdentityContext db;

        public KullaniciRepository(IdentityContext db)
        {
            this.db = db;
        }

        public async Task<Kullanici> BulAsync(int id)
        {
            return await db.Kullanicilar
                .Include(k => k.KisiBilgisi).ThenInclude(kisi=>kisi.Cinsiyeti)
                .Include(k=>k.Fotograflari).FirstOrDefaultAsync(k=>k.Id==id);
        }

        public async Task Ekle<T>(T entity) where T : class
        {
            await db.AddAsync<T>(entity);
        }

        public async Task<Foto> FotografBulAsync(int id)
        {
            var foto = await db.Fotograflar.FirstOrDefaultAsync(p => p.Id == id);
            return foto;
        }
        public async Task<Foto> KullanicininAsilFotosunuGetirAsync(int kullaniciNo)
        {
            var foto = await db.Fotograflar.Where(p => p.KullaniciNo== kullaniciNo).FirstOrDefaultAsync(p=>p.ProfilFotografi);
            return foto;
        }

        public async Task<bool> Kaydet()
        {
            var sonuc = await db.SaveChangesAsync();
            return sonuc > 0;
        }

        public async Task<IEnumerable<Kullanici>> ListeGetirKullanicilarTumuAsync()
        {
            return await db.Kullanicilar
                .Include(k=>k.KisiBilgisi).ThenInclude(kisi=>kisi.Cinsiyeti)
                .Include(k=>k.Fotograflari).ToListAsync<Kullanici>();
        }

        public void Sil<T>(T entity) where T : class
        {
            db.Remove<T>(entity);
        }
    }
}
