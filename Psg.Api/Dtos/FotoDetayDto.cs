using Microsoft.AspNetCore.Http;
using System;

namespace Psg.Api.Dtos
{
    public class FotoDetayDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Aciklama { get; set; }
        public DateTime EklenmeTarihi { get; set; }
        public bool ProfilFotografi { get; set; }
    }

    public class FotoOkuDto
    {
        public int Id { get; set; }
        public string DosyaAdi { get; set; }
        public string Url { get; set; }
        public string Aciklama { get; set; }
        public DateTime EklenmeTarihi { get; set; }
        public bool ProfilFotografi { get; set; }
        public string PublicId { get; set; }
    }


    public class FotografYazDto
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Aciklama { get; set; }
        public DateTime EklenmeTarihi { get; set; }
        public string PublicId { get; set; }
        public bool ProfilFotografi { get; set; }
        
        public FotografYazDto()
        {
            EklenmeTarihi = DateTime.Now;
        }
    }
}
