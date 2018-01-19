using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Psg.Api.Repos;
using System;
using Psg.Api.Base;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Dynamic;
using System.Reflection;
using System.Collections.Generic;

namespace Psg.Api.Helpers
{
    public class KullaniciAktiviteleriniTakipEt : IAsyncActionFilter
    {


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            var kullaniciClaim = resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (kullaniciClaim != null)
            {
                var kullaniciNo = int.Parse(kullaniciClaim.Value);
                if (kullaniciNo <= 0) return;
                var repo = resultContext.HttpContext.RequestServices.GetService<IKullaniciRepository>();
                var kullanici = await repo.BulAsync(kullaniciNo);
                if (kullanici == null) return;
                kullanici.SonAktifOlma = DateTime.Now;
                await repo.KaydetAsync();
            }
        }
    }
    public static class SayfaliListesiExtensions
    {
        public static string SayfalamaMetaDataYarat<T>(this SayfaliListe<T> kayitlar, ISayfaBilgiYaratici sayfaBilgiYaratici)
        {
            if (sayfaBilgiYaratici == null)
                throw new Exception("Sayfa bilgi yaratıcı yok!");

            var sayfalamaMetadatasi = new
            {
                kayitSayisi = kayitlar.SayfaBilgisi.KayitSayisi,
                sayfaBuyuklugu = kayitlar.SayfaBilgisi.SayfaBuyuklugu,
                sayfa = kayitlar.SayfaBilgisi.Sayfa,
                sayfaSayisi = kayitlar.SayfaBilgisi.SayfaSayisi,
                oncekiSayfa = kayitlar.SayfaBilgisi.OncesiVar ? sayfaBilgiYaratici.UriYarat(ResourceUriType.OncekiSayfa) : null,
                sonrakiSayfa = kayitlar.SayfaBilgisi.SonrasiVar ? sayfaBilgiYaratici.UriYarat(ResourceUriType.SonrakiSayfa) : null,
            };
            return JsonConvert.SerializeObject(sayfalamaMetadatasi);
        }

    }

    public interface ISayfaBilgiYaratici
    {
        string UriYarat(ResourceUriType type);
    }
    public class StandartSayfaBilgiYaratici : ISayfaBilgiYaratici
    {

        private readonly string kaynakMetodAdi;
        private readonly IUrlHelper urlHelper;
        private readonly SorguBase sorgu;

        public StandartSayfaBilgiYaratici(SorguBase sorgu, string kaynakMetodAdi, IUrlHelper urlHelper)
        {
            this.kaynakMetodAdi = kaynakMetodAdi;
            this.urlHelper = urlHelper;
            this.sorgu = sorgu;
        }


        public string UriYarat(ResourceUriType type)
        {
            int sayfa = 0;
            switch (type)
            {
                case ResourceUriType.OncekiSayfa:
                    sayfa = sorgu.Sayfa - 1;
                    break;
                case ResourceUriType.SonrakiSayfa:
                    sayfa = sorgu.Sayfa + 1;
                    break;
                default:
                    sayfa = sorgu.Sayfa;
                    break;
            }
            string uri = UriUret(sayfa);
            return uri;
        }

        protected virtual string UriUret(int sayfa)
        {
            return urlHelper.Link(kaynakMetodAdi,
                new
                {
                    siralamaAlani = sorgu.SiralamaCumlesi,
                    aramaCumlesi = sorgu.AramaCumlesi,
                    sayfa = sayfa,
                    sayfaBuyuklugu = sorgu.SayfaBuyuklugu
                });
        }
    }
    public enum ResourceUriType
    {
        OncekiSayfa,
        SonrakiSayfa
    }
    public static class ListSonucExtensions
    {
        public static ListeSonuc<TSource> ShapeData<TSource>(
            this ListeSonuc<TSource> source,
                string fields)
        {
            if (source == null)
                throw new ArgumentException("kaynak boş olamaz!");
            var expandoObjectList = new List<ExpandoObject>();

            var propertyInfoList = new List<PropertyInfo>();
            if (string.IsNullOrWhiteSpace(fields))
            {
                return source;
                //var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                //propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var fieldsAfterSplit = fields.Split(',');
                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();
                    var propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo == null)
                        throw new Exception($"Proeprty {propertyName} wasn't found on {typeof(TSource)}.");
                    propertyInfoList.Add(propertyInfo);
                }
            }
            foreach (TSource sourceObject in source.DonenListe)
            {
                var dataShapedObject = new ExpandoObject();

                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);
                    dataShapedObject.TryAdd(propertyInfo.Name, propertyValue);
                }
                expandoObjectList.Add(dataShapedObject);
            }
            source.DonenSekillenmisListe = expandoObjectList;
            source.DonenListe = null;
            return source;
        }
    }
    public static class SonucExtensions
    {

        public static KayitSonuc<TSource> ShapeData<TSource>(this KayitSonuc<TSource> source, string fields)
        {
            if (source == null)
                throw new ArgumentNullException("kaynak boş olamaz1");
            var dataShapedObject = new ExpandoObject();
            if (string.IsNullOrWhiteSpace(fields))
            {
                return source;
            }
            var propertyInfoList = new List<PropertyInfo>();
            var fieldsAfterSplit = fields.Split(',');
            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();
                var propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                    throw new Exception($"Proeprty {propertyName} wasn't found on {typeof(TSource)}.");
                propertyInfoList.Add(propertyInfo);
            }

            foreach (var propertyInfo in propertyInfoList)
            {
                var propertyValue = propertyInfo.GetValue(source.DonenNesne);
                dataShapedObject.TryAdd(propertyInfo.Name, propertyValue);
            }
            source.DonenNesne = default(TSource);
            source.DonenSekillenmisNesne = dataShapedObject;
            return source;
        }
    }
}
