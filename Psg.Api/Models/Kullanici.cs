using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Psg.Api.Models
{
    public class Kullanici
    {
        public int Id { get; set; }
        public string KullaniciAdi { get; set; }
        public string EPosta { get; set; }
        public bool? EpostaOnaylandi { get; set; }
        public string TelefonNumarasi { get; set; }
        public bool? TelefonOnaylandi { get; set; }
        public bool Aktif { get; set; }
        public string Unvan { get; set; }
        public string Ad { get; set; }
        public string DigerAd { get; set; }
        public string Soyad { get; set; }
        public byte[] SifreHash { get; set; }
        public byte[] SifreSalt { get; set; }
        public DateTime DogumTarihi { get; set; }
        public string Cinsiyeti { get; set; }
        public DateTime YaratilmaTarihi { get; set; }
        public DateTime? SonAktifOlma { get; set; }
        [NotMapped]
        public string TamAdi { get { return $"{Unvan} {Ad} {DigerAd} {Soyad}".Trim(); } }
        public ICollection<Foto> Fotograflari { get; set; } = new List<Foto>();

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

}
