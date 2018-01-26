using System;
using System.ComponentModel.DataAnnotations;

namespace Psg.Api.Dtos
{

    

    public class UyelikYaratDto
    {

        [StringLength(10, MinimumLength = 0, ErrorMessageResourceName = "MetinUzunlugu", ErrorMessageResourceType = typeof(Psg.Api.Properties.Resources), ErrorMessage = "")]
        [Display(Name = "Ünvan")]
        public string Unvan { get; set; }

        [Required(ErrorMessageResourceName = "AlanaVeriGirilmesiZorunlu", ErrorMessageResourceType = typeof(Psg.Api.Properties.Resources), ErrorMessage = "")]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceName = "MetinUzunlugu", ErrorMessageResourceType = typeof(Psg.Api.Properties.Resources), ErrorMessage = "")]
        [Display(Name = "Adı")]
        public string Ad { get; set; }

        [Required(ErrorMessageResourceName = "AlanaVeriGirilmesiZorunlu", ErrorMessageResourceType = typeof(Psg.Api.Properties.Resources), ErrorMessage = "")]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceName = "MetinUzunlugu", ErrorMessageResourceType = typeof(Psg.Api.Properties.Resources), ErrorMessage = "")]
        [Display(Name = "Soyadı")]
        public string Soyad { get; set; }

        [Required(ErrorMessageResourceName = "AlanaVeriGirilmesiZorunlu", ErrorMessageResourceType = typeof(Psg.Api.Properties.Resources), ErrorMessage = "")]
        [Display(Name = "Cinsiyeti")]
        public int CinsiyetNo { get; set; }

        [Required(ErrorMessageResourceName = "AlanaVeriGirilmesiZorunlu", ErrorMessageResourceType = typeof(Psg.Api.Properties.Resources), ErrorMessage = "")]
        [DataType(DataType.DateTime, ErrorMessageResourceName = "YanlisTarih", ErrorMessageResourceType = typeof(Psg.Api.Properties.Resources), ErrorMessage = "")]
        [Display(Name = "Doğum Tarihi")]
        public DateTime? DogumTarihi { get; set; }

        [Required(ErrorMessageResourceName = "AlanaVeriGirilmesiZorunlu", ErrorMessageResourceType = typeof(Psg.Api.Properties.Resources), ErrorMessage = "")]
        [StringLength(20, MinimumLength = 4, ErrorMessageResourceName = "MetinUzunlugu", ErrorMessageResourceType = typeof(Psg.Api.Properties.Resources), ErrorMessage = "")]
        [Display(Name = "Kullanıcı adı")]
        public string KullaniciAdi { get; set; }
        [Required(ErrorMessageResourceName = "AlanaVeriGirilmesiZorunlu", ErrorMessageResourceType = typeof(Psg.Api.Properties.Resources), ErrorMessage = "")]
        [StringLength(8, MinimumLength = 4, ErrorMessageResourceName = "MetinUzunlugu", ErrorMessageResourceType = typeof(Psg.Api.Properties.Resources), ErrorMessage = "")]
        [Display(Name = "Şifre")]
        public string Sifre { get; set; }

        [Required(ErrorMessageResourceName = "AlanaVeriGirilmesiZorunlu", ErrorMessageResourceType = typeof(Psg.Api.Properties.Resources), ErrorMessage = "")]
        [DataType(DataType.EmailAddress, ErrorMessage = "{0} alanına doğru bir eposta adresi girilmelidir.")]
        [Display(Name = "Eposta")]
        public string EPosta { get; set; }

        [Required(ErrorMessageResourceName = "AlanaVeriGirilmesiZorunlu", ErrorMessageResourceType = typeof(Psg.Api.Properties.Resources), ErrorMessage = "")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "{0} alanına doğru bir telefon numarası girilmelidir.")]
        [Display(Name = "Telefon numarası")]
        public string TelefonNumarasi { get; set; }

        public string DigerAd { get; internal set; }
    }
}
