using AutoMapper;
using Core.Base.Helpers;
using Core.EntityFramework;
using Core.EntityFramework.SharedEntity;
using Identity.DataAccess.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.DataAccess.Mappers
{
    public class FotoProfile : Profile
    {
        public FotoProfile()
        {

            //CreateMap(typeof(QueryResult<>), typeof(QueryResultResource<>));
            
            CreateEntityToResourceMap();
            CreateResourceToEntityMap();
        }

        private void CreateEntityToResourceMap()
        {
           
            CreateMap<Foto, FotoDetayDto>();
            CreateMap<Foto, FotoOkuDto>();
        }

        private void CreateResourceToEntityMap()
        {
            CreateMap<FotoDetayDto, Foto>().ForMember(k => k.FotoId, islem => islem.Ignore());
            CreateMap<FotografYazDto, Foto>();
        }
    }
}
