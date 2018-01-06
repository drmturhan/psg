using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Psg.Api.Dtos
{

    public class GirisDto
    {
        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{0} alanına en az {2} en fazla {1} karakter girebilirsiniz")]
        [Display(Name = "Kullanıcı adı")]
        public string KullaniciAdi { get; set; }

        [Required]

        [StringLength(8, MinimumLength = 4, ErrorMessage = "{0} alanına en az {2} en fazla {1} karakter girebilirsiniz")]
        [Display(Name = "Şifre")]
        public string Sifre { get; set; }
    }
    public class UyeYazDto
    {
        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{0} alanına en az {2} en fazla {1} karakter girebilirsiniz")]
        [Display(Name = "Kullanıcı adı")]
        public string KullaniciAdi { get; set; }

        [Required]

        [StringLength(8, MinimumLength = 4, ErrorMessage = "{0} alanına en az {2} en fazla {1} karakter girebilirsiniz")]
        [Display(Name = "Şifre")]
        public string Sifre { get; set; }
    }

    public class UyeOkuDto
    {
        public int Id { get; set; }
        public string KullaniciAdi { get; set; }
    }

    public class UykuTestOkuDto
    {
        public int Id { get; set; }
        public int HastaNo { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public int Yas { get; set; }
        public double Ahi { get; set; }
        public double St90 { get; set; }
    }
    public class UykuTestYazDto
    {
        public int Id { get; set; }
        public int HastaNo { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public DateTime DogumTarihi { get; set; }
        public double Ahi { get; set; }
        public double St90 { get; set; }
    }
}
