using AutoMapper;
using Psg.Api.Dtos;
using Psg.Api.Models;
using System;

namespace Psg.Api.Profiles
{
    public class UykuTestProfile : Profile
    {
        public UykuTestProfile()
        {
            CreateEntityToResourceMap();
            CreateResourceToEntityMap();
        }

        private void CreateResourceToEntityMap()
        {
            CreateMap<UykuTestYazDto, UykuTest>()
                .AfterMap((d, e) => {
                    if (string.IsNullOrWhiteSpace(d.Ad) && string.IsNullOrWhiteSpace(d.Soyad))
                        e.Hasta = null;
                    else
                    {
                        e.Hasta = new Hasta();
                        e.Hasta.Ad = d.Ad;
                        e.Hasta.Soyad = d.Soyad;
                        e.Hasta.DogumTarihi = d.DogumTarihi;
                    }
                });
        }

        private void CreateEntityToResourceMap()
        {
            CreateMap<UykuTest, UykuTestOkuDto>()
                .ForMember(r => r.Ad, islem => islem.MapFrom(e => e.Hasta.Ad))
                .ForMember(r => r.Soyad, islem => islem.MapFrom(e => e.Hasta.Soyad))
                .ForMember(r => r.Yas, islem => islem.ResolveUsing(e => DateTime.Today.Year - e.Hasta.DogumTarihi.Year))
                .ForMember(r => r.St90, islem => islem.MapFrom(e => e.St90))
                .ForMember(r => r.Ahi, islem => islem.MapFrom(e => e.Ahi));
        }
    }
}

