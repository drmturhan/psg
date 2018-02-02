using System.ComponentModel.DataAnnotations;

namespace Psg.Api.Dtos
{
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



    public class SifreyiSifirlaDto
    {
        [Required]
        [EmailAddress]
        public string Eposta { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string SifreKontrolonfirmPassword { get; set; }

        public string Kod { get; set; }
    }
}
