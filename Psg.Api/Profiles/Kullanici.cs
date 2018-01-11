using AutoMapper;
using Psg.Api.Dtos;
using Psg.Api.Extensions;
using Psg.Api.Models;
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
                .ForMember(dto => dto.Yasi, islem => islem.ResolveUsing(e => e.DogumTarihi.YasHesapla()))
                .ForMember(dto => dto.AsilFotoUrl, islem => islem.ResolveUsing(e => e.AsilFotografUrlGetir()));
            CreateMap<Kullanici, KullaniciDetayDto>()
                .ForMember(dto => dto.Yasi, islem => islem.ResolveUsing(e => e.DogumTarihi.YasHesapla()))
                .ForMember(dto => dto.AsilFotoUrl, islem => islem.ResolveUsing(e => e.AsilFotografUrlGetir()));

            CreateMap<Foto, FotoDetayDto>();

        }



        private void CreateResourceToEntityMap()
        {
            CreateMap<UyeYazDto, Kullanici>()
                .ForMember(kaynak => kaynak.KullaniciAdi, islem => islem.MapFrom(source => source.KullaniciAdi));

            CreateMap<UyeOkuDto, Kullanici>()
                .ForMember(kaynak => kaynak.KullaniciAdi, islem => islem.MapFrom(source => source.KullaniciAdi));
        }
    }
}

