using Core.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Core.EntityFramework
{

    public abstract class SorguBase
    {
        public string AramaCumlesi { get; set; }
        public string Alanlar { get; set; }
        public string SiralamaCumlesi { get; set; }
        public int Sayfa { get; set; } = 1;
        public int SayfaBuyuklugu { get; set; } = 10;
       
    }

    public class StoreBase<TContext, TEntity> : StoreBase, IStoreReadBase<TContext, TEntity> where TContext : DbContext where TEntity : class
    {
        public TContext Context { get; protected set; }
        public virtual IQueryable<TEntity> Liste { get { return Context.Set<TEntity>().AsNoTracking().AsQueryable(); } }
    }
    public class StoreBase : IDisposable
    {
        public HataTanimlayici HataTanimlayici { get; protected set; }

        protected bool _disposed;
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _disposed = true;
        }
    }
}
