using Microsoft.EntityFrameworkCore;
using Psg.Api.Base;
using Psg.Api.Data;
using Psg.Api.Extensions;
using Psg.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            return await db.Kullanicilar.Include(k=>k.Fotograflari).FirstOrDefaultAsync(k=>k.Id==id);
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
            var foto = await db.Fotograflar.Where(p => p.KullaniciNo== kullaniciNo).FirstOrDefaultAsync(p=>p.IlkTercihmi);
            return foto;
        }

        public async Task<bool> Kaydet()
        {
            var sonuc = await db.SaveChangesAsync();
            return sonuc > 0;
        }

        public async Task<IEnumerable<Kullanici>> ListeGetirKullanicilarTumuAsync()
        {
            return await db.Kullanicilar.Include(k=>k.Fotograflari).ToListAsync<Kullanici>();
        }

        public void Sil<T>(T entity) where T : class
        {
            db.Remove<T>(entity);
        }
    }

    public class AuthRepository : IAuthRepository
    {
        private readonly IdentityContext db;

        public AuthRepository(IdentityContext db)
        {
            this.db = db;
        }
      



        public async Task<bool> KullaniciVarAsync(string kullaniciadi)
        {
            return await db.Kullanicilar.CountAsync(kul => kul.KullaniciAdi == kullaniciadi) == 1;
        }

        public async Task<Kullanici> UyeOlAsync(Kullanici kullanici, string sifre)
        {
            byte[] passwordHash, passwordSalt;
            sifre.CreateHashes(out passwordHash, out passwordSalt);
            kullanici.SifreSalt = passwordSalt;
            kullanici.SifreHash = passwordHash;
            await db.Kullanicilar.AddAsync(kullanici);
            await db.SaveChangesAsync();
            return kullanici;
        }

        public async Task<Kullanici> GirisYapAsync(string kullaniciadi, string sifre)
        {

            var kullanici = await db.Kullanicilar.Include(k=>k.Fotograflari).SingleOrDefaultAsync(k => k.KullaniciAdi == kullaniciadi);
            if (kullanici == null) return null;

            bool sonuc = false;

            sifre.VerifyPasswordHash(kullanici.SifreHash, kullanici.SifreSalt, out sonuc);

            if (sonuc) return kullanici;
            else
                return null;
        }

        public async Task<Kullanici> KullaniciBulAsync(int id)
        {
            var kullanici = await db.Kullanicilar.SingleOrDefaultAsync(k => k.Id == id);
            kullanici.SifreHash = null;
            kullanici.SifreSalt = null;
            return kullanici;

        }
    }
    public interface IAuthRepository
    {
        Task<Kullanici> UyeOlAsync(Kullanici kullanici, string sifre);
        Task<Kullanici> GirisYapAsync
            (string kullaniciadi, string sifre);
        Task<bool> KullaniciVarAsync(string kullaniciadi);
        Task<Kullanici> KullaniciBulAsync(int id);
    }
}
