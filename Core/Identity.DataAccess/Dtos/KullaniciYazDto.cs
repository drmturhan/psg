using System;

namespace Identity.DataAccess.Dtos
{
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
}

