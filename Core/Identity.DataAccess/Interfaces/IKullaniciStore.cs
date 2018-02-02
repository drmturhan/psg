using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.DataAccess
{
    public interface IKullaniciStore<TUser> : IUserStore<TUser> where TUser : Kullanici
    {
        KullaniciKisi KisiyiAl(string ad, string soyad, DateTime dogumTarihi, int cinsiyet);
        bool KisininKullanicisiVar(KullaniciKisi kisi);
        Task<List<Kullanici>> ListeGetirKullanicilarinTumuAsync();
        KullaniciKisi KisiyiAl(int kisiNo);
        Task<KisiFoto> FotoBulAsync(int kisiNo);
        Task<bool> KisiyiKaydetAsync(KullaniciKisi kisi, string hataMesaji);
        Task<List<KisiCinsiyet>> ListeGetirCinsiyetlerAsync();
        Task<List<Kullanici>> ListeGetirEpostayaBenzerAsync(string eposta);
        Task<Kullanici> KullaniciyiGetirEpostayaGoreAsync(string eposta);
        Task<Kullanici> KullaniciyiGetirIdyeGoreAsync(string id);
        Task<bool> KullaniciGetirGuvenlikKodunaGore(string kod);
        Task<Kullanici> KullaniciyiGetirKullaniciAdinaGoreAsync(string userName);
        Task<bool> KullaniciVarAsync(string userName);
    }
}
