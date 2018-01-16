using Microsoft.EntityFrameworkCore;
using Psg.Api.Base;
using Psg.Api.Data;
using Psg.Api.Extensions;
using Psg.Api.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Psg.Api.Repos
{


    public interface IAuthRepository
    {
        Task<Kullanici> UyeOlAsync(Kullanici kullanici, string sifre);
        Task<Kullanici> GirisYapAsync
            (string kullaniciadi, string sifre);
        Task<bool> KullaniciVarAsync(string kullaniciadi);
        Task<Kullanici> KullaniciBulAsync(int id);
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

            var kullanici = await db.Kullanicilar
                .Include(k => k.KisiBilgisi).ThenInclude(kisi=>kisi.Cinsiyeti)
                .Include(k=>k.Fotograflari).SingleOrDefaultAsync(k => k.KullaniciAdi == kullaniciadi);
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

}
