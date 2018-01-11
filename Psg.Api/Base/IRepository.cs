using Psg.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psg.Api.Base
{
    public interface IRepository
    {
        
        Task Ekle<T>(T entity) where T : class;
        void Sil<T>(T entity) where T : class;
        Task<bool> Kaydet();
    }

    



}
