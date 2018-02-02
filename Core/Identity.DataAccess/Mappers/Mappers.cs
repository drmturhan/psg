using AutoMapper;
using Core.EntityFramework;
using Core.EntityFramework.SharedEntity;
using Identity.DataAccess.Dtos;

namespace Identity.DataAccess.Mappers
{
    public static class KullaniciMappers
    {
        internal static IMapper Mapper { get; }
        static KullaniciMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<KullaniciProfile>()).CreateMapper();
        }

        public static KullaniciDetayDto ToKullaniciDetayDto(this Kullanici entity)

        {
            return entity == null ? null : Mapper.Map<KullaniciDetayDto>(entity);
        }
        public static KullaniciYazDto ToDto(this Kullanici entity)

        {
            return entity == null ? null : Mapper.Map<KullaniciYazDto>(entity);
        }

        
       

        public static Kullanici ToEntity(this KullaniciDetayDto resource)

        {
            return resource == null ? null : Mapper.Map<Kullanici>(resource);
        }

        public static Kullanici ToEntity(this KullaniciYazDto resource)

        {
            return resource == null ? null : Mapper.Map<Kullanici>(resource);
        }
        public static Kullanici ToEntity(this UyelikYaratDto resource)

        {
            return resource == null ? null : Mapper.Map<Kullanici>(resource);
        }
        public static ListeSonuc<KullaniciListeDto> ToKullaniciDetayDto(this ListeSonuc<Kullanici> entitySonuc)
        {
            return entitySonuc == null ? null : Mapper.Map<ListeSonuc<KullaniciListeDto>>(entitySonuc);
        }
        public static ListeSonuc<Kullanici> ToEntity(this ListeSonuc<KullaniciListeDto> entitySonuc)
        {
            return entitySonuc == null ? null : Mapper.Map<ListeSonuc<Kullanici>>(entitySonuc);
        }
        public static void Kopyala(KullaniciYazDto yazDto, Kullanici entity)
        {
            Mapper.Map(yazDto, entity);
        }
    }


    public static class ArkadaslikTeklifMappers
    {
        internal static IMapper Mapper { get; }
        static ArkadaslikTeklifMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ArkadaslikTeklifProfile>()).CreateMapper();
        }

        public static ArkadaslarimListeDto ToDto(this ArkadaslikTeklif entity)
        {
            return entity == null ? null : Mapper.Map<ArkadaslarimListeDto>(entity);
        }
       

        public static ListeSonuc<ArkadaslarimListeDto> ToDto(this ListeSonuc<ArkadaslikTeklif> entitySonuc)
        {
            return entitySonuc == null ? null : Mapper.Map<ListeSonuc<ArkadaslarimListeDto>>(entitySonuc);
        }
        public static ListeSonuc<ArkadaslikTeklif> ToEntity(this ListeSonuc<ArkadaslarimListeDto> dtoSonuc)
        {
            return dtoSonuc == null ? null : Mapper.Map<ListeSonuc<ArkadaslikTeklif>>(dtoSonuc);
        }
    }
    public static class KisiFotoMappers
    {
        internal static IMapper Mapper { get; }
        static KisiFotoMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<FotoProfile>()).CreateMapper();
        }

        public static FotoOkuDto ToFotoOkuDto(this Foto entity)
        {
            return entity == null ? null : Mapper.Map<FotoOkuDto>(entity);
        }

        public static KisiFoto ToEntity(this FotografYazDto dto)
        {
            return dto == null ? null : Mapper.Map<KisiFoto>(dto);
        }
        
    }
}
