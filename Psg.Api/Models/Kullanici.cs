using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psg.Api.Models
{
    public class Kullanici
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PassswordSalt { get; set; }
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
