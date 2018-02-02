using System;

namespace Identity.DataAccess.Dtos
{
    public class ArkadaslarimListeDto
    {
        public KullaniciListeDto ArkadaslikIsteyen { get; set; }
        public KullaniciListeDto TeklifEdilen { get; set; }
        public DateTime IstekTarihi { get; set; }
        public DateTime? CevapTarihi { get; set; }
        public bool? Karar { get; set; }
    }
}

