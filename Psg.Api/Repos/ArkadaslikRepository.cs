using Microsoft.EntityFrameworkCore;
using Psg.Api.Base;
using Psg.Api.Data;
using Psg.Api.Dtos;
using Psg.Api.Extensions;
using Psg.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psg.Api.Repos
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
        private readonly IdentityContext db;
        private readonly IPropertyMappingService propertyMappingService;

        public ArkadaslikRepository(IdentityContext db, IPropertyMappingService propertyMappingService)
        {
            this.db = db;
            this.propertyMappingService = propertyMappingService;
            propertyMappingService.AddMap<ArkadaslarimListeDto, ArkadaslikTeklif>(ArkadaslikTeklifPropertyMap.Values);

            sorgu = db.ArkadaslikTeklifleri
                .Include(t => t.ArkadaslikIsteyen).ThenInclude(k => k.KisiBilgisi).ThenInclude(kisi => kisi.Cinsiyeti)
                .Include(t => t.TeklifEdilen).ThenInclude(k => k.KisiBilgisi).ThenInclude(kisi => kisi.Cinsiyeti).AsQueryable();

        }
        public async Task EkleAsync<T>(T entity) where T : class
        {
            await db.AddAsync(entity);
        }

        public async Task<Foto> FotografBulAsync(int fotografNo)
        {
            return await db.Fotograflar.FirstOrDefaultAsync(f => f.Id == fotografNo);
        }

        public async Task<bool> KaydetAsync()
        {
            var sonuc = await db.SaveChangesAsync();
            return sonuc > 0;
        }

        public async Task<Kullanici> KullaniciBulAsync(int kullaniciNo)
        {
            return await db.Kullanicilar.SingleOrDefaultAsync(k => k.Id == kullaniciNo);
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
            return await db.Kullanicilar.ToListAsync();
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
            return await db.Fotograflar.Where(f => f.KullaniciNo == kullaniciNo).FirstOrDefaultAsync(p => p.ProfilFotografi);
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

