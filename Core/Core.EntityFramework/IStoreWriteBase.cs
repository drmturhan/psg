using Core.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.EntityFramework
{
    public interface IStoreWriteBase<TContext, TEntity> : IDisposable
      where TContext : DbContext
      where TEntity : class
    {
        bool AutoSaveChanges { get; set; }
        TContext Context { get; }

        Task<Sonuc<TEntity>> DegistirAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
        Task<Sonuc<TEntity>> SilAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
        Task<Sonuc<TEntity>> YeniAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
    }
   
}
