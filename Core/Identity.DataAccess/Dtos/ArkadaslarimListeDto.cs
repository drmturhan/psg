using System;

namespace Identity.DataAccess.Dtos
{
    public class ArkadaslarimListeDto
    {
        public ArkadasDto TeklifEden { get; set; }
        public ArkadasDto TeklifEdilen { get; set; }
        public DateTime IstekTarihi { get; set; }
        public DateTime? CevapTarihi { get; set; }
        public bool? Karar { get; set; }
    }
    public class ArkadasDto
    {
        public int Id { get; set; }
        public string Eposta { get; set; }
        public bool EpostaOnaylandi { get; set; }
        public string TelefonNumarasi { get; set; }
        public bool TelefonOnaylandi { get; set; }
        public string ProfilFotoUrl { get; set; }
        public string CinsiyetAdi { get; set; }
        public int Yasi { get; set; }
        public string TamAdi { get; set; }

    }
}

