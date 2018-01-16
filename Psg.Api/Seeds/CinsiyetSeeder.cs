using Newtonsoft.Json;
using Psg.Api.Data;
using Psg.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Psg.Api.Seeds
{
    public class CinsiyetSeeder : ISeeder
    {
        private readonly IdentityContext idDb;
        public CinsiyetSeeder()
        {
            idDb = new IndetityContextFactory().CreateDbContext(null);
        }

        public CinsiyetSeeder(IdentityContext idDb)
        {
            this.idDb = idDb;
        }

        public int Oncelik { get { return 5; } }

        public void GetContext() { }
        public async Task Seed()
        {
            try
            {
                if (idDb.Cinsiyetler.Any()) return;
                var dosyaAdi = "Seeds/Veriler/cinsiyetler.json";
                var veriJson = await File.ReadAllTextAsync(dosyaAdi);
                var veriler = JsonConvert.DeserializeObject<List<Cinsiyet>>(veriJson);
                foreach (var veri in veriler)
                {
                    idDb.Cinsiyetler.Add(veri);
                }
                idDb.SaveChanges();
            }
            catch (Exception hata)
            {

            }

        }
    }
}
