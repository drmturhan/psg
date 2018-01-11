﻿using System.ComponentModel.DataAnnotations;

namespace Psg.Api.Dtos
{
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
}
