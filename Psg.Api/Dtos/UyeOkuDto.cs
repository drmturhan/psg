using System;

namespace Psg.Api.Dtos
{
    public class UyeOkuDto
    {
        public int Id { get; set; }
        public string KullaniciAdi { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public DateTime DogumTarihi { get; set; }
    }
}
