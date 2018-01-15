using System;
using System.ComponentModel.DataAnnotations;

namespace Psg.Api.Dtos
{
    public class UyelikYaratDto
    {
        
        [StringLength(10, MinimumLength = 0, ErrorMessage = "{0} alanına en az {2} en fazla {1} karakter girebilirsiniz")]
        [Display(Name = "Ünvan")]
        public string Unvan { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} alanına en az {2} en fazla {1} karakter girebilirsiniz")]
        [Display(Name = "Adı")]
        public string Ad { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} alanına en az {2} en fazla {1} karakter girebilirsiniz")]
        [Display(Name = "Soyadı")]
        public string Soyad { get; set; }
        [Required(ErrorMessage = "{0} alanına bilgi girilmesi zorunludur.")]
        [Display(Name = "Cinsiyeti")]
        public int CinsiyetNo { get; set; }
        [Required(ErrorMessage = "{0} alanına bilgi girilmesi zorunludur.")]
        [DataType(DataType.DateTime, ErrorMessage = "{0} alanına doğru bir tarih girilmelidir.")]
        [Display(Name = "Doğum Tarihi")]
        public DateTime DogumTarihi { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{0} alanına en az {2} en fazla {1} karakter girebilirsiniz")]
        [Display(Name = "Kullanıcı adı")]
        public string KullaniciAdi { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "{0} alanına en az {2} en fazla {1} karakter girebilirsiniz")]
        [Display(Name = "Şifre")]
        public string Sifre { get; set; }

        [Required(ErrorMessage = "{0} alanına bilgi girilmesi zorunludur.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "{0} alanına doğru bir eposta adresi girilmelidir.")]
        [Display(Name = "Doğum Tarihi")]

        public string Eposta { get; set; }
        [Required(ErrorMessage = "{0} alanına bilgi girilmesi zorunludur.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "{0} alanına doğru bir telefon numarası girilmelidir.")]
        [Display(Name = "Doğum Tarihi")]
        public string TelefonNumarasi { get; set; }
        public string DigerAd { get; internal set; }
    }
}
