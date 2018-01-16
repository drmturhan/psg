using System;

namespace Psg.Api.Models
{
    public class Foto
    {
        public int Id { get; set; }
        public string DosyaAdi { get; set; }
        public string Url { get; set; }
        public string Aciklama { get; set; }
        public DateTime EklenmeTarihi { get; set; }
        public bool ProfilFotografi { get; set; }
        public string PublicId { get; set; }
        public int KullaniciNo { get; set; }
        public Kullanici Kullanici { get; set; }
    }
}