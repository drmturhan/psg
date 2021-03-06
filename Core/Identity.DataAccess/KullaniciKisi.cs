﻿using Core.Base;
using Core.EntityFramework.SharedEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.DataAccess
{
    public class KullaniciKisi : Kisi
    {
        public MedeniHal MedeniHali { get; set; }
        public KisiCinsiyet Cinsiyeti { get; set; }
        public ICollection<Kullanici> Kullanicilari { get; set; } = new List<Kullanici>();
        public ICollection<KisiFoto> Fotograflari{ get; set; } = new List<KisiFoto>();
    }

}
