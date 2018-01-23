using Microsoft.AspNetCore.Http;
using System;
using System.Text;

namespace Psg.Api.Helpers
{
    public static class ErrorExtensions
    {

        public static void UygulamaHatasiEkle(this HttpResponse response, Exception hata)
        {
            //response.Headers.Add("Application-Error", HataCumlesiniAl(hata));
            //response.Headers.Add("Access-Control-Expose-Headers", "Uygulama-Hatasu");
            //response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        private static string HataCumlesiniAl(Exception hata)
        {
            StringBuilder sb = new StringBuilder();
            HataEkle(sb, hata);
            return sb.ToString();
        }

        private static void HataEkle(StringBuilder sb, Exception hata)
        {
            sb.AppendLine(hata.Message);
            if (hata.InnerException != null)
                HataEkle(sb, hata.InnerException);
        }
    }
}
