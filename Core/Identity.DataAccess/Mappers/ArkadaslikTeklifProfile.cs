using AutoMapper;
using Identity.DataAccess.Dtos;

namespace Identity.DataAccess.Mappers
{
    public class ArkadaslikTeklifProfile : Profile
    {
        public ArkadaslikTeklifProfile()
        {
            CreateEntityToResourceMap();
            CreateResourceToEntityMap();
        }

        private void CreateEntityToResourceMap()
        {
            CreateMap<ArkadaslikTeklifProfile, ArkadaslarimListeDto>();

        }

        private void CreateResourceToEntityMap()
        {
            CreateMap<ArkadaslarimListeDto, ArkadaslikTeklifProfile>();
        }
    }
}
