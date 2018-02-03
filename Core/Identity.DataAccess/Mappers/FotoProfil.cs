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

            CreateMap<KisiFoto, FotoDetayDto>()
                .ForMember(dt => dt.Id, isl => isl.ResolveUsing(ent => ent.FotoId));

            CreateMap<KisiFoto, FotoOkuDto>()
                .ForMember(dt => dt.Id, isl => isl.ResolveUsing(ent => ent.FotoId));

        }

        private void CreateResourceToEntityMap()
        {
            CreateMap<FotoDetayDto, KisiFoto>().ForMember(k => k.FotoId, islem => islem.Ignore());
            CreateMap<FotografYazDto, KisiFoto>();
        }
    }
}
