using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Psg.Api.Extensions;
using Psg.Api.Properties;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;

namespace Psg.Api.Base
{
    public class MTController : Controller
    {
        protected readonly string TEMEL_ENTITY_ADI = "";
        public MTController(string entityAdi)
        {
            TEMEL_ENTITY_ADI = entityAdi;
        }
        protected async Task<IActionResult> HataKontrolluCalistir<R>(Func<Task<IActionResult>> codetoExecute) where R : class
        {
            try
            {
                return await codetoExecute.Invoke();
            }
            catch (ModelStateError)
            {
                return new DortYuzYirmiIki(ModelState);
            }
            catch (Exception hata)
            {
                Sonuc<R> sonuc = Sonuc<R>.Basarisiz(new Exception("Beklenmedik bir hata oluştu", hata));
                return StatusCode(500, sonuc);
            }
        }
        protected async Task<IActionResult> GecerlilikKontrolluCalistir<R>( Func<IActionResult>  codetoExecute) where R : class
        {
            if (!ModelState.IsValid)
                return new DortYuzYirmiIki(ModelState);
            try
            {
                return codetoExecute.Invoke();
            }

            catch (BadRequestError hata)
            {
                return BadRequest(new BadRequestError(hata.Message));
            }
            catch (ModelStateError)
            {
                return new DortYuzYirmiIki(ModelState);
            }
            catch (NotFoundError hata)
            {
                return NotFound(Sonuc.Basarisiz(new Exception("Kayıt bulunamadı!", hata)));
            }
            catch (InternalServerError hata)
            {
                return NotFound(Sonuc.Basarisiz(new Exception("İşlem başarısız. Lütfen daha sonra tekrar deneyiniz!", hata)));
            }
            catch (Exception hata)
            {
                return StatusCode(500, Sonuc.Basarisiz(new Exception(Properties.Resources.IslemGerceklesmedi, hata)));
            }
        }
        
        #region Kapat
        //protected void Calistir(Action codetoExecute)
        //{
        //    try
        //    {
        //        codetoExecute.Invoke();
        //    }
        //    catch (AuthorizationValidationException ex)
        //    {
        //        throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
        //    }
        //    catch (FaultException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new FaultException(ex.Message);
        //    }
        //} 
        #endregion

        #region Yardimcilar
        protected string AnBilgisiAl()
        {

            return $"Kullanıcı:{User.Identity.Name}, Tarih:{DateTime.UtcNow}, IpNo:{GetClientIp()}, MACAddress:{GetClientMAC()}";
        }
        private string GetClientIp()
        {
            return HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();//{Request.HttpContext.Connection.RemoteIpAddress} de kullanilmis orneklerde
        }

        private string GetClientMAC()
        {
            string macAddresses = "";

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }
        #endregion

    }
    public class ModelStateError : ArgumentException
    {

    }
    public class BadRequestError : ArgumentException
    {
        public BadRequestError()
        {
        }

        public BadRequestError(string message) : base(message)
        {
        }
    }
    public class NotFoundError : ArgumentException
    {
        public NotFoundError()
        {
        }

        public NotFoundError(string message) : base(message)
        {
        }
    }
    public class DortYuzYirmiIki : ObjectResult
    {
        public DortYuzYirmiIki(ModelStateDictionary modelState) : base(new SerializableError(modelState))
        {
            if (modelState == null)
                throw new ArgumentNullException(nameof(modelState));
            StatusCode = 422;
        }
    }
    public class Sonuc : Sonuc<object>
    {

    }

    public class ListeSonuc<TEntity> : Sonuc<TEntity>
    {
        public IList<TEntity> DonenListe { get; set; }
        public IEnumerable<ExpandoObject> DonenSekillenmisListe { get; set; }
        public int KayitSayisi { get; set; }
        public int SayfaSayisi { get; set; }
        public int SayfaBuyuklugu { get; set; }
        public int Sayfa { get; set; }
        public static Sonuc<TEntity> IslemTamam(SayfaliListe<TEntity> kayitlar)
        {
            var result = Tamam as ListeSonuc<TEntity>;
            result.DonenListe = kayitlar;
            result.Sayfa = kayitlar.SayfaBilgisi.Sayfa;
            result.KayitSayisi = kayitlar.SayfaBilgisi.KayitSayisi;
            result.SayfaBuyuklugu = kayitlar.SayfaBilgisi.SayfaBuyuklugu;
            result.SayfaSayisi = kayitlar.SayfaBilgisi.SayfaSayisi;
            return result;
        }
        public static ListeSonuc<TEntity> IslemTamam(IList<TEntity> kayitlar)
        {
            var result = Tamam as ListeSonuc<TEntity>;
            result.DonenListe = kayitlar;
            return result;
        }
        public static new ListeSonuc<TEntity> Tamam
        {
            get
            {
                return new ListeSonuc<TEntity> { Basarili = true, Mesajlar = new List<string> { SonucMesajlari.Liste[MesajAnahtarlari.IslemBasarili] } };
            }
        }
    }
    public class KayitSonuc<TEntity> : Sonuc<TEntity>
    {
        public TEntity DonenNesne { get; set; }
        public ExpandoObject DonenSekillenmisNesne { get; set; }
        public static Sonuc<TEntity> IslemTamam(TEntity kayit)
        {
            var result = Tamam; result.DonenNesne = kayit;
            return result;
        }
        public static new KayitSonuc<TEntity> Tamam
        {
            get
            {
                return new KayitSonuc<TEntity> { Basarili = true, Mesajlar = new List<string> { SonucMesajlari.Liste[MesajAnahtarlari.IslemBasarili] } };
            }
        }
    }
    public class Sonuc<TEntity>
    {

        private List<Hata> _errors;
        public Sonuc()
        {
            _errors = new List<Hata>();
            Mesajlar = new List<string>();
        }

        public bool Basarili { get; protected set; }
        public IList<Hata> Hatalar => _errors;
        public List<string> Mesajlar { get; set; }
        public static Sonuc<TEntity> Tamam
        {
            get
            {
                return new Sonuc<TEntity> { Basarili = true, Mesajlar = new List<string> { SonucMesajlari.Liste[MesajAnahtarlari.IslemBasarili] } };
            }
        }
        public static Sonuc<TEntity> Basarisiz(params Hata[] errors)
        {
            var result = new Sonuc<TEntity> { Basarili = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }
        public static Sonuc<TEntity> Basarisiz(Exception hata)
        {
            var result = new Sonuc<TEntity> { Basarili = false };
            hata.SonucaYaz(result);
            return result;
        }

        public override string ToString()
        {
            return Basarili ?
                   "Başarılı" :
                   string.Format("{0} : {1}", "Hatalı sonuç", string.Join(",", Hatalar.Select(x => x.Tanim).ToList()));
        }
        public string ToString(bool mesaj = true)
        {
            if (!mesaj || !Basarili) return ToString();

            return
                  string.Format("{0} : {1}", "Mesajlar", string.Join(",", Mesajlar.ToList()));

        }
    }
    public class InternalServerError : InvalidOperationException
    {
        public InternalServerError()
        {
        }

        public InternalServerError(string message) : base(message)
        {
        }
    }
    public enum MesajAnahtarlari
    {
        IslemBasarili,
        KayitBulunamadi,
        SifirdanBuyukDegerGerekli,
        KayitBosOlamaz,
        IslemGerceklesmedi,
        DahaSonraTekrarDeneyiniz
    }

    public class SonucMesajlari : SortedList<MesajAnahtarlari, string>
    {
        private static volatile SonucMesajlari instance;
        private static object syncRoot = new Object();


        protected SonucMesajlari()
        {
            Add(MesajAnahtarlari.IslemBasarili, Resources.IslemBasarili);
            Add(MesajAnahtarlari.KayitBulunamadi, Resources.KayitBulunamadi);
            Add(MesajAnahtarlari.SifirdanBuyukDegerGerekli, Resources.BuyukDegerGerekli);
            Add(MesajAnahtarlari.KayitBosOlamaz, Resources.KayitBosOlamaz);
            Add(MesajAnahtarlari.IslemGerceklesmedi, Resources.IslemGerceklesmesi);
            Add(MesajAnahtarlari.DahaSonraTekrarDeneyiniz, Resources.DahaSonraTekrarDeneyiniz);
        }

        public static SonucMesajlari Liste
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SonucMesajlari();
                    }
                }
                return instance;
            }
        }
    }
    public class Hata
    {
        public string Kod { get; set; }
        public string Tanim { get; set; }
    }
    public enum ValidatorTipleri
    {
        Yaratma
    }
    public class SayfaBilgi
    {
        public int KayitSayisi { get; set; }
        public int SayfaSayisi { get; set; }
        public int SayfaBuyuklugu { get; set; }
        public int Sayfa { get; set; }
        [JsonIgnore]
        public bool OncesiVar => Sayfa > 1;
        [JsonIgnore]
        public bool SonrasiVar
        {
            get
            {
                return Sayfa < SayfaSayisi;
            }
        }
    }
    public class SayfaliListe<T> : List<T>
    {
        public SayfaliListe()
        {
        }
        public SayfaBilgi SayfaBilgisi { get; set; } = new SayfaBilgi();
        public SayfaliListe(List<T> kayitlar, int kayitSayisi, int sayfa, int sayfaBuyuklugu)
        {
            AddRange(kayitlar);
            SayfaBilgisi.KayitSayisi = kayitSayisi;
            SayfaBilgisi.Sayfa = sayfa;
            SayfaBilgisi.SayfaBuyuklugu = sayfaBuyuklugu;
            SayfaBilgisi.SayfaSayisi = (int)Math.Ceiling(kayitSayisi / (double)sayfaBuyuklugu);
        }

        public static async Task<SayfaliListe<T>> SayfaListesiYarat(IQueryable<T> kaynakSorgu, int sayfa, int sayfaBuyuklugu)
        {
            var count = await kaynakSorgu.CountAsync();
            var items = await kaynakSorgu.Skip((sayfa - 1) * sayfaBuyuklugu).Take(sayfaBuyuklugu).ToListAsync();
            return new SayfaliListe<T>(items, count, sayfa, sayfaBuyuklugu);
        }
    }
    public interface IPropertyMappingService
    {
        bool ValidMappingsExistsFor<TSource, TDestinatin>(string fields);
        void AddMap<TSource, TDestination>(Dictionary<string, PropertyMappingValue> mappingValues);
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
    public interface IPropertyMapping
    {

    }
    public class PropertyMapping<TSource, TDictionary> : IPropertyMapping
    {
        public Dictionary<string, PropertyMappingValue> mappingDictionary { get; private set; }
        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            this.mappingDictionary = mappingDictionary;
        }

    }
    public class PropertyMappingService : IPropertyMappingService
    {


        protected IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();
        public PropertyMappingService()
        {

        }
        public void AddMap<TSource, TDestination>(Dictionary<string, PropertyMappingValue> mappingValues)
        {
            var mathingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();
            if (mathingMapping.Count() == 0)
                propertyMappings.Add(new PropertyMapping<TSource, TDestination>(mappingValues));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var mathingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();
            if (mathingMapping.Count() == 1)
            {
                return mathingMapping.First().mappingDictionary;
            }
            throw new Exception($"Cannot find exact property mapping instance for<{typeof(TSource)}>");
        }
        public bool ValidMappingsExistsFor<TSource, TDestinatin>(string fields)
        {
            var propertyMappig = GetPropertyMapping<TSource, TDestinatin>();
            if (string.IsNullOrWhiteSpace(fields))
                return true;
            var fieldAfterSplit = fields.Split(',');
            foreach (var field in fieldAfterSplit)
            {
                var trimmedField = field.Trim();
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ? trimmedField : trimmedField.Remove(indexOfFirstSpace);
                if (!propertyMappig.ContainsKey(propertyName))
                    return false;
            }
            return true;
        }
    }
    
    public interface ITypeHelperService
    {
        bool TryHastProperties<T>(string fields);
    }
    public class TypeHelperService : ITypeHelperService
    {
        public bool TryHastProperties<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
                return true;
            var fieldsAfterSplit = fields.Split(',');
            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();
                var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                    return false;
            }
            return true;
        }
    }
    public abstract class SorguBase
    {
        public string AramaCumlesi { get; set; }
        public string Alanlar { get; set; }
        public string SiralamaCumlesi { get; set; }
        public int Sayfa { get; set; } = 1;
        public int SayfaBuyuklugu { get; set; } = 10;

    }
}

