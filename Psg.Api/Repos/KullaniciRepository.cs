using Microsoft.EntityFrameworkCore;
using Psg.Api.Base;
using Psg.Api.Controllers;
using Psg.Api.Data;
using Psg.Api.Dtos;
using Psg.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Psg.Api.Extensions;
namespace Psg.Api.Repos
{
    public interface IKullaniciRepository : IRepository
    {
        Task<bool> KullaniciVarAsync(string kullaniciAdi);
        Task<Kullanici> BulAsync(int id);
        Task<SayfaliListe<Kullanici>> ListeGetirKullanicilarTumuAsync(KullaniciSorgu  sorgu);
        Task<Foto> FotografBulAsync(int id);
        Task<Foto> KullanicininAsilFotosunuGetirAsync(int kullaniciNo);
        void KisileriniSil(Kisi entity);
        
    }

    public class KullaniciRepository : IKullaniciRepository
    {
        private readonly IdentityContext db;
        private readonly IPropertyMappingService propertyMappingService;

        public KullaniciRepository(IdentityContext db, IPropertyMappingService propertyMappingService)
        {
            this.db = db;
            this.propertyMappingService = propertyMappingService;
            propertyMappingService.AddMap<KullaniciListeDto, Kullanici>(KullaniciPropertyMap.Values);
        }

        public async Task<Kullanici> BulAsync(int id)
        {
            return await db.Kullanicilar
                .Include(k => k.KisiBilgisi).ThenInclude(kisi=>kisi.Cinsiyeti)
                .Include(k=>k.Fotograflari).FirstOrDefaultAsync(k=>k.Id==id);
        }

        public async Task EkleAsync<T>(T entity) where T : class
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

        public async Task<bool> KaydetAsync()
        {
            var sonuc = await db.SaveChangesAsync();
            return sonuc > 0;
        }
        public IQueryable<Kullanici> Sorgu { get { return db.Kullanicilar.Include(kul => kul.KisiBilgisi).ThenInclude(k => k.Cinsiyeti).Include(kul=>kul.Fotograflari); } }
        public async Task<SayfaliListe<Kullanici>> ListeGetirKullanicilarTumuAsync(KullaniciSorgu sorguNesnesi)
        {
            var siralamaBilgisi = propertyMappingService.GetPropertyMapping<KullaniciListeDto, Kullanici>();
            var siralanmisSorgu = Sorgu.SiralamayiAyarla(sorguNesnesi.SiralamaCumlesi, siralamaBilgisi);
            var sonuc = await SayfaliListe<Kullanici>.SayfaListesiYarat(siralanmisSorgu, sorguNesnesi.Sayfa, sorguNesnesi.SayfaBuyuklugu);
            return sonuc;
        }
        public void Sil<T>(T entity) where T:class
        {
            db.Remove<T>(entity);
        }
        public void KisileriniSil(Kisi entity)
        {
            db.Kisiler.Remove(entity);
        }

        public Task<bool> KullaniciVarAsync(string kullaniciAdi)
        {
            return db.Kullanicilar.AnyAsync(k => k.KullaniciAdi == kullaniciAdi);
        }
    }
    public class KullaniciPropertyMap
    {

        public static Dictionary<string, PropertyMappingValue> Values = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            { "Id",new PropertyMappingValue(new List<string>{"Id" })},
            { "AdSoyad",new PropertyMappingValue(new List<string>{"Ad","Soyad"})}
        };

    }
}
