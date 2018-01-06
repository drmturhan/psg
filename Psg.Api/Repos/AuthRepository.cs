using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Psg.Api.Data;
using Psg.Api.Models;
using System.Text;
using System.Threading.Tasks;

namespace Psg.Api.Repos
{

    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext db;

        public AuthRepository(DataContext db)
        {
            this.db = db;
        }
        public async Task<Kullanici> GirisYap(string kullaniciadi, string sifre)
        {
            var kullanici = await db.Kullanicilar.FirstOrDefaultAsync(k => k.Username == kullaniciadi);
            if (kullanici == null)
                return null;
            if (!VerifyPasswordHash(sifre, kullanici.PasswordHash, kullanici.PassswordSalt))  return null;
            return kullanici;
        }

        private bool VerifyPasswordHash(string sifre, byte[] passwordHash, byte[] passswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(sifre));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        public async Task<bool> KullaniciVarAsync(string kullaniciadi)
        {
            return await db.Kullanicilar.CountAsync(kul => kul.Username == kullaniciadi) == 1;
        }

        public async Task<Kullanici> UyeOlAsync(Kullanici kullanici, string sifre)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(sifre, out passwordHash, out passwordSalt);
            kullanici.PassswordSalt = passwordSalt;
            kullanici.PasswordHash = passwordHash;
            await db.Kullanicilar.AddAsync(kullanici);
            await db.SaveChangesAsync();
            return kullanici;
        }

        private void CreatePasswordHash(string sifre, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(sifre));
            }
        }

        public async Task<Kullanici> GirisYapAsync(string kullaniciadi, string sifre)
        {

            var kullanici = await db.Kullanicilar.SingleOrDefaultAsync(k => k.Username == kullaniciadi);
            if (kullanici == null) return null;
            if (!VerifyPasswordHash(sifre, kullanici.PasswordHash, kullanici.PassswordSalt))
                return null;
            return kullanici;
        }

        public async Task<Kullanici> KullaniciBulAsync(int id)
        {
            var kullanici = await db.Kullanicilar.SingleOrDefaultAsync(k => k.Id == id);
            kullanici.PasswordHash = null;
            kullanici.PassswordSalt = null;
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
