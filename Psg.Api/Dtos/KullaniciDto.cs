using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Psg.Api.Dtos
{
    public class KullaniciListeDto : KullaniciBaseDto
    {
        public string CinsiyetAdi { get; set; }
    }


    public class KullaniciBaseDto
    {
        public int Id { get; set; }
        public string KullaniciAdi { get; set; }
        public string Eposta { get; set; }
        public bool EpostaOnaylandi { get; set; }
        public string TelefonNumarasi { get; set; }
        public bool TelefonOnaylandi { get; set; }
        public string ProfilFotoUrl { get; set; }
        public bool Aktif { get; set; }
        public int Yasi { get; set; }
        public DateTime YaratilmaTarihi { get; set; }
        public DateTime? SonAktifOlma { get; set; }
        public ICollection<FotoDetayDto> Fotograflari { get; set; } = new List<FotoDetayDto>();
        public string TamAdi { get; set; }
    }

    public class KullaniciYazDto : KullaniciBaseDto
    {
        public int KisiNo { get; set; }
        public string Unvan { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string DigerAd { get; set; }
        public int CinsiyetNo { get; set; }
        public DateTime DogumTarihi { get; set; }
    }

    public class KullaniciDetayDto : KullaniciBaseDto
    {

    }
}

