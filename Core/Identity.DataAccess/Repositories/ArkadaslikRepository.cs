using Core.EntityFramework;
using Core.EntityFramework.SharedEntity;
using Identity.DataAccess.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.DataAccess.Repositories
{
    public enum ArkadaslikListeTipleri
    {

        Tumu,
        SadeceKabulEdilenler,
        SadeceReddedilenler,
        SadeceCevapBeklenenler,
        SadeceCevapVerilenler
    }

    public class ArkadaslikSorgusu : SorguBase
    {
        public int? TeklifEdenKullaniciNo { get; set; }
        public int? TeklifEdilenKullaniciNo { get; set; }
        public ArkadaslikListeTipleri ListeTipi { get; set; }
    }
    public interface IArkadaslikRepository : IRepository
    {
        Task<List<Kullanici>> ListeGetirKullanicilarAsync();
        Task<SayfaliListe<ArkadaslikTeklif>> ListeGetirTekliflerAsync(ArkadaslikSorgusu sorgu);
        Task<Kullanici> KullaniciBulAsync(int kullaniciNo);
        Task<Foto> FotografBulAsync(int fotografNo);
        Task<Foto> ProfilFotografiniAlAsync(int kullaniciNo);
        Task<ArkadaslikTeklif> TeklifiBulAsync(int isteyenKullaniciNo, int cevaplayanKullaniciNo);

    }
    public class ArkadaslikRepository : IArkadaslikRepository
    {
        private readonly MTIdentityDbContext db;
        private readonly IPropertyMappingService propertyMappingService;

        public ArkadaslikRepository(MTIdentityDbContext db, IPropertyMappingService propertyMappingService)
        {
            this.db = db;
            this.propertyMappingService = propertyMappingService;
            propertyMappingService.AddMap<ArkadaslarimListeDto, ArkadaslikTeklif>(ArkadaslikTeklifPropertyMap.Values);

            sorgu = db.ArkadaslikTeklifleri
                .Include(t => t.ArkadaslikIsteyen).ThenInclude(k => k.Kisi).ThenInclude(kisi => kisi.Cinsiyeti)
                .Include(t => t.TeklifEdilen).ThenInclude(k => k.Kisi).ThenInclude(kisi => kisi.Cinsiyeti).AsQueryable();

        }
        public async Task EkleAsync<T>(T entity) where T : class
        {
            await db.AddAsync(entity);
        }

        public async Task<Foto> FotografBulAsync(int fotografNo)
        {
            return await db.KisiFotograflari.FirstOrDefaultAsync(f => f.FotoId == fotografNo);
        }

        public async Task<bool> KaydetAsync()
        {
            var sonuc = await db.SaveChangesAsync();
            return sonuc > 0;
        }

        public async Task<Kullanici> KullaniciBulAsync(int kullaniciNo)
        {
            return await db.Users.SingleOrDefaultAsync(k => k.Id == kullaniciNo);
        }

        IQueryable<ArkadaslikTeklif> sorgu;

        public async Task<SayfaliListe<ArkadaslikTeklif>> ListeGetirTekliflerAsync(ArkadaslikSorgusu sorguNesnesi)
        {
            var siralamaBilgisi = propertyMappingService.GetPropertyMapping<ArkadaslarimListeDto, ArkadaslikTeklif>();
            var siralanmisSorgu = sorgu.SiralamayiAyarla(sorguNesnesi.SiralamaCumlesi, siralamaBilgisi);

            if (sorguNesnesi.TeklifEdenKullaniciNo.HasValue)
                sorgu = sorgu.Where(teklif => teklif.ArkadaslikIsteyenNo == sorguNesnesi.TeklifEdenKullaniciNo.Value);
            if (sorguNesnesi.TeklifEdilenKullaniciNo.HasValue)
                sorgu = sorgu.Where(teklif => teklif.TeklifEdilenNo == sorguNesnesi.TeklifEdilenKullaniciNo.Value);
            if (sorguNesnesi.ListeTipi != ArkadaslikListeTipleri.Tumu)
                sorgu = ListetipiniBelirle(sorgu, sorguNesnesi);
            var sonuc = await SayfaliListe<ArkadaslikTeklif>.SayfaListesiYarat(siralanmisSorgu, sorguNesnesi.Sayfa, sorguNesnesi.SayfaBuyuklugu);
            return sonuc;
        }
        public async Task<List<Kullanici>> ListeGetirKullanicilarAsync()
        {
            return await db.Users.ToListAsync();
        }

        private IQueryable<ArkadaslikTeklif> ListetipiniBelirle(IQueryable<ArkadaslikTeklif> query, ArkadaslikSorgusu sorgu)
        {

            switch (sorgu.ListeTipi)
            {

                case ArkadaslikListeTipleri.SadeceKabulEdilenler:
                    return query.Where(t => t.Karar.Value);

                case ArkadaslikListeTipleri.SadeceReddedilenler:
                    return query.Where(t => !t.Karar.Value);

                case ArkadaslikListeTipleri.SadeceCevapBeklenenler:
                    return query.Where(t => t.Karar == null);
                case ArkadaslikListeTipleri.SadeceCevapVerilenler:
                    return query.Where(t => t.Karar != null);

            }
            return query;
        }

        public async Task<Foto> ProfilFotografiniAlAsync(int kullaniciNo)
        {
            return await db.KisiFotograflari.Where(f => f.Kisi.Kullanicilari.Count(k=>k.Id==kullaniciNo)==1).FirstOrDefaultAsync(p => p.ProfilFotografi);
        }

        public void Sil<T>(T entity) where T : class
        {
            db.Remove(entity);
        }

        public async Task<ArkadaslikTeklif> TeklifiBulAsync(int isteyenKullaniciNo, int cevaplayanKullaniciNo)
        {
            return await db.ArkadaslikTeklifleri.FirstOrDefaultAsync(a => a.ArkadaslikIsteyenNo == isteyenKullaniciNo && a.TeklifEdilenNo == cevaplayanKullaniciNo);
        }


    }
    public class ArkadaslikTeklifPropertyMap
    {

        public static Dictionary<string, PropertyMappingValue> Values = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            { "Id",new PropertyMappingValue(new List<string>{"Id" })}
        };

    }
}

