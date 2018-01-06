using AutoMapper;
using Psg.Api.Dtos;
using Psg.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                .ForMember(des => des.KullaniciAdi, islem => islem.MapFrom(source => source.Username))
                .ForMember(des => des.Sifre, islem => islem.Ignore());
            CreateMap<Kullanici, UyeOkuDto>()
            .ForMember(des => des.KullaniciAdi, islem => islem.MapFrom(source => source.Username));

        }
        private void CreateResourceToEntityMap()
        {
            CreateMap<UyeYazDto, Kullanici>()
                .ForMember(kaynak => kaynak.Username, islem => islem.MapFrom(source => source.KullaniciAdi));

            CreateMap<UyeOkuDto, Kullanici>()
                .ForMember(kaynak => kaynak.Username, islem => islem.MapFrom(source => source.KullaniciAdi));
        }
    }
}

