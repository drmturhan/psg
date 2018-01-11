﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psg.Api.Dtos
{
    public class UyeDto
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
