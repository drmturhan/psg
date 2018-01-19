using AutoMapper;
using Psg.Api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psg.Api.Profiles
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