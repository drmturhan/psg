
using Core.Base;
using System;
using System.Collections.Generic;

namespace Core.EntityFramework
{
    public static class ExceptionExtensions
    {
        public static void SonucaYaz<T>(this Exception hata, Sonuc<T> sonuc) 
        {
            if (sonuc == null) return;
            List<Hata> hatalar = new List<Hata> { new Hata { Kod = "", Tanim = hata.Message } };
            hatalar.ForEach(h => sonuc.Hatalar.Add(h));
            if (hata.InnerException != null)
                hata.InnerException.SonucaYaz<T>(sonuc);
        }
    }
}
