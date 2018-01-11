using Psg.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psg.Api.Profiles
{
    public static class ProfileExtensions
    {
        public static string AsilFotografUrlGetir(this Kullanici entity)
        {
            if (entity == null || entity.Fotograflari.Count == 0) return string.Empty;
            Foto asilFoto = entity.Fotograflari.FirstOrDefault(f => f.IlkTercihmi);
            return asilFoto != null ? asilFoto.Url : string.Empty;
        }
    }
}
