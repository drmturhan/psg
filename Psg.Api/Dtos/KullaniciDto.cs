using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Psg.Api.Dtos
{
    public class KullaniciListeDto
    {
        public int Id { get; set; }
        public string KullaniciAdi { get; set; }
        public string TamAdi { get; set; }
        public string Cinsiyeti { get; set; }
        public int Yasi { get; set; }
        public string Eposta { get; set; }
        public string TelefonNumarasi { get; set; }
        public string AsilFotoUrl { get; set; }
        public DateTime YaratilmaTarihi { get; set; }
        public DateTime? SonAktifOlma { get; set; }
    }


    public class KullaniciBaseDto
    {
        public int Id { get; set; }
        public string KullaniciAdi { get; set; }
        public string Cinsiyeti { get; set; }
        public string Eposta { get; set; }
        public string TelefonNumarasi { get; set; }
        public string AsilFotoUrl { get; set; }
        public DateTime YaratilmaTarihi { get; set; }
        public DateTime? SonAktifOlma { get; set; }
        public ICollection<FotoDetayDto> Fotograflari { get; set; } = new List<FotoDetayDto>();
    }

    public class KullaniciYazDto:KullaniciBaseDto
    {
        public string Unvan { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string DigerAd { get; set; }
        public DateTime DogumTarihi { get; set; }
        public bool? TelefonOnaylandi { get; set; }
        public bool? EpostaOnaylandi { get; set; }
        public int Aktif { get; set; }
        
    }
    public class KullaniciDetayDto:KullaniciBaseDto
    {
        public int Yasi { get; set; }
    }
    public class FotoDetayDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Aciklama { get; set; }
        public DateTime EklenmeTarihi { get; set; }
        public bool IlkTercihmi { get; set; }

    }
}
