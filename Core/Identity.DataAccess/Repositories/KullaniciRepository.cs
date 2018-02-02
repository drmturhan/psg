﻿using Core.EntityFramework;
using Core.EntityFramework.SharedEntity;
using Identity.DataAccess.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.DataAccess.Repositories
{


 
    public interface IKullaniciRepository : IRepository
    {
        Task<bool> KullaniciVarAsync(string kullaniciAdi);
        Task<Kullanici> BulAsync(int id);
        Task<SayfaliListe<Kullanici>> ListeGetirKullanicilarTumuAsync(KullaniciSorgu sorgu);
        Task<Foto> FotografBulAsync(int id);
        Task<Foto> KullanicininAsilFotosunuGetirAsync(int kullaniciNo);
        void KisileriniSil(KullaniciKisi entity);

    }
    public class KullaniciSorgu : SorguBase
    {

    }

    public class KullaniciRepository : IKullaniciRepository
    {
        private readonly MTIdentityDbContext db;
        private readonly IPropertyMappingService propertyMappingService;

        public KullaniciRepository(MTIdentityDbContext db, IPropertyMappingService propertyMappingService)
        {
            this.db = db;
            this.propertyMappingService = propertyMappingService;
            propertyMappingService.AddMap<KullaniciListeDto, Kullanici>(KullaniciPropertyMap.Values);
        }

        public async Task<Kullanici> BulAsync(int id)
        {
            return await db.Users
                .Include(k => k.Kisi).ThenInclude(kisi => kisi.Cinsiyeti)
                .Include(k => k.Kisi).ThenInclude(k => k.Fotograflari).FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task EkleAsync<T>(T entity) where T : class
        {
            await db.AddAsync<T>(entity);
        }

        public async Task<Foto> FotografBulAsync(int id)
        {
            var foto = await db.KisiFotograflari.FirstOrDefaultAsync(p => p.FotoId == id);
            return foto;
        }
        public async Task<Foto> KullanicininAsilFotosunuGetirAsync(int kullaniciNo)
        {
            var foto = await db.KisiFotograflari.Where(f => f.Kisi.Kullanicilari.Count(k => k.Id == kullaniciNo) == 1).FirstOrDefaultAsync(p => p.ProfilFotografi);
            return foto;
        }

        public async Task<bool> KaydetAsync()
        {
            var sonuc = await db.SaveChangesAsync();
            return sonuc > 0;
        }
        public IQueryable<Kullanici> Sorgu
        {
            get
            {
                return db.Users.Include(kul => kul.Kisi)
                    .ThenInclude(k => k.Cinsiyeti)
                    .Include(kul => kul.Kisi).ThenInclude(kul => kul.Fotograflari);
            }
        }
        public async Task<SayfaliListe<Kullanici>> ListeGetirKullanicilarTumuAsync(KullaniciSorgu sorguNesnesi)
        {
            var siralamaBilgisi = propertyMappingService.GetPropertyMapping<KullaniciListeDto, Kullanici>();
            var siralanmisSorgu = Sorgu.SiralamayiAyarla(sorguNesnesi.SiralamaCumlesi, siralamaBilgisi);
            var sonuc = await SayfaliListe<Kullanici>.SayfaListesiYarat(siralanmisSorgu, sorguNesnesi.Sayfa, sorguNesnesi.SayfaBuyuklugu);
            return sonuc;
        }
        public void Sil<T>(T entity) where T : class
        {
            db.Remove<T>(entity);
        }
        public void KisileriniSil(KullaniciKisi entity)
        {
            db.Kisiler.Remove(entity);
        }

        public Task<bool> KullaniciVarAsync(string kullaniciAdi)
        {
            return db.Users.AnyAsync(k => k.UserName== kullaniciAdi);
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