using Newtonsoft.Json;
using Psg.Api.Data;
using Psg.Api.Dtos;
using Psg.Api.Extensions;
using Psg.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Psg.Api.Seeds
{
    public interface ISeeder
    {
        int Oncelik { get; }
        Task Seed();
    }

    public class UserSeeder : ISeeder
    {
        private readonly IdentityContext idDb;
        public UserSeeder()
        {
            idDb = new IndetityContextFactory().CreateDbContext(null);
        }

        public UserSeeder(IdentityContext idDb)
        {
            this.idDb = idDb;
        }

        public int Oncelik { get { return 10; } }

        public void GetContext() { }
        public async Task Seed()
        {
            try
            {
                
                if (idDb.Kullanicilar.Any()) return;
                var dosyaAdi = "Seeds/Veriler/kullanicilar.json";
                var userJson = await File.ReadAllTextAsync(dosyaAdi);
                var users = JsonConvert.DeserializeObject<List<Kullanici>>(userJson);
                foreach (var user in users)
                {
                    byte[] sifreHash, sifreSalt;
                    string sifre = "akd3463";
                    sifre.CreateHashes(out sifreHash, out sifreSalt);
                    user.SifreHash = sifreHash;
                    user.SifreSalt = sifreSalt;
                    idDb.Kullanicilar.Add(user);
                }
                idDb.SaveChanges();
            }
            catch (Exception hata)
            {

            }

        }
    }
}
