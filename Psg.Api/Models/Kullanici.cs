using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Psg.Api.Models
{

    public class Cinsiyet
    {
        public int Id { get; set; }
        public string CinsiyetAdi { get; set; }
        public ICollection<Kisi> Kisiler { get; set; } = new List<Kisi>();
    }

    public class Kisi
    {
        public int Id { get; set; }
        public string Unvan { get; set; }
        public string Ad { get; set; }
        public string DigerAd { get; set; }
        public string Soyad { get; set; }
        public DateTime DogumTarihi { get; set; }
        public int CinsiyetNo { get; set; }
        public Cinsiyet Cinsiyeti { get; set; }
        [NotMapped]
        public string TamAdi { get { return $"{Unvan} {Ad} {DigerAd} {Soyad}".Trim(); } }
        public ICollection<Kullanici> Kullanicilar { get; set; } = new List<Kullanici>();
        


    }
    public class Kullanici
    {
        public int Id { get; set; }
        public int KisiNo { get; set; }
        public Kisi KisiBilgisi { get; set; }
        public string KullaniciAdi { get; set; }
        public string EPosta { get; set; }
        public bool? EpostaOnaylandi { get; set; }
        public string TelefonNumarasi { get; set; }
        public bool? TelefonOnaylandi { get; set; }
        public bool Aktif { get; set; }
        public byte[] SifreHash { get; set; }
        public byte[] SifreSalt { get; set; }
        public DateTime YaratilmaTarihi { get; set; }
        public DateTime? SonAktifOlma { get; set; }
        public ICollection<Foto> Fotograflari { get; set; } = new List<Foto>();
        public ICollection<ArkadaslikTeklif> YapilanTeklifler { get; set; } = new List<ArkadaslikTeklif>();
        public ICollection<ArkadaslikTeklif> GelenTeklifler { get; set; } = new List<ArkadaslikTeklif>();

    }
    public class UykuTest
    {
        public int Id { get; set; }
        public DateTime? Tarih { get; set; }
        public int HastaNo { get; set; }
        public Hasta Hasta { get; set; }
        public double Ahi { get; set; }
        public double St90 { get; set; }
    }

    public class ArkadaslikTeklif
    {
        public int ArkadaslikIsteyenNo { get; set; }
        public Kullanici ArkadaslikIsteyen { get; set; }
        public int TeklifEdilenNo { get; set; }
        public Kullanici TeklifEdilen { get; set; }
        public DateTime IstekTarihi { get; set; }
        public DateTime? CevapTarihi { get; set; }
        public bool? Karar { get; set; }

    }


}
