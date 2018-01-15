using AutoMapper;
using Psg.Api.Dtos;
using Psg.Api.Extensions;
using Psg.Api.Models;
using System;
using System.Linq;

namespace Psg.Api.Profiles
{
    public class KullaniciProfile : Profile
    {
        public KullaniciProfile()
        {
            CreateEntityToResourceMap();
            CreateResourceToEntityMap();
        }

        private void CreateEntityToResourceMap()
        {
            CreateMap<Kullanici, UyeYazDto>()
                .ForMember(des => des.KullaniciAdi, islem => islem.MapFrom(source => source.KullaniciAdi))
                .ForMember(des => des.Sifre, islem => islem.Ignore());
            CreateMap<Kullanici, UyeOkuDto>()
            .ForMember(des => des.KullaniciAdi, islem => islem.MapFrom(source => source.KullaniciAdi));

            CreateMap<Kullanici, KullaniciListeDto>()
                .ForMember(dto => dto.Yasi, islem => islem.ResolveUsing(e => e.KisiBilgisi.DogumTarihi.YasHesapla()))
                .ForMember(dto => dto.AsilFotoUrl, islem => islem.ResolveUsing(e => e.AsilFotografUrlGetir()));
            CreateMap<Kullanici, KullaniciYazDto>()
                .ForMember(dto => dto.AsilFotoUrl, islem => islem.ResolveUsing(e => e.AsilFotografUrlGetir()));
            CreateMap<Kullanici, KullaniciDetayDto>()
                .ForMember(dto => dto.Yasi, islem => islem.ResolveUsing(e => e.KisiBilgisi.DogumTarihi.YasHesapla()))
                .ForMember(dto => dto.AsilFotoUrl, islem => islem.ResolveUsing(e => e.AsilFotografUrlGetir()));

            CreateMap<Foto, FotoDetayDto>();
            CreateMap<Foto, FotoOkuDto>();

        }



        private void CreateResourceToEntityMap()
        {
            CreateMap<UyeYazDto, Kullanici>()
                .ForMember(kaynak => kaynak.KullaniciAdi, islem => islem.MapFrom(source => source.KullaniciAdi));

            CreateMap<UyeOkuDto, Kullanici>()
                .ForMember(kaynak => kaynak.KullaniciAdi, islem => islem.MapFrom(source => source.KullaniciAdi));

            CreateMap<KullaniciBaseDto, Kullanici>()
                .ForMember(k => k.SifreHash, islem => islem.Ignore())
                .ForMember(k => k.SifreSalt, islem => islem.Ignore())
                .AfterMap((d, e) =>
                {
                    foreach (var eFoto in e.Fotograflari)
                    {
                        eFoto.KullaniciNo = d.Id;
                    }
                });

            CreateMap<KullaniciYazDto, Kullanici>()
                .ForMember(k => k.Id, islem => islem.Ignore())
                .ForMember(k => k.SifreHash, islem => islem.Ignore())
                .ForMember(k => k.SifreSalt, islem => islem.Ignore())
                .AfterMap((d, e) =>
                {
                    foreach (var eFoto in e.Fotograflari)
                    {
                        eFoto.KullaniciNo = d.Id;
                    }
                });
            CreateMap<FotoDetayDto, Foto>().ForMember(k => k.Id, islem => islem.Ignore());
            CreateMap<FotografYazDto, Foto>();
            CreateMap<UyelikYaratDto, Kullanici>()
                .ForPath(d => d.KisiBilgisi.Unvan, opt => opt.MapFrom(s => s.Unvan))
                .ForPath(d => d.KisiBilgisi.Ad, opt => opt.MapFrom(s => s.Ad))
                .ForPath(d => d.KisiBilgisi.DigerAd, opt => opt.MapFrom(s => s.DigerAd))
                .ForPath(d => d.KisiBilgisi.Soyad, opt => opt.MapFrom(s => s.Soyad))
                .ForPath(d => d.KisiBilgisi.CinsiyetNo, opt => opt.MapFrom(s => s.CinsiyetNo))
                .ForPath(d => d.KisiBilgisi.DogumTarihi, opt => opt.MapFrom(s => s.DogumTarihi));

        }
    }
}

