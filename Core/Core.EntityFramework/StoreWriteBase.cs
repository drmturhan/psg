using Core.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.EntityFramework
{
    public class StoreWriteBase<TContext, TEntity> : StoreBase, IStoreWriteBase<TContext, TEntity> where TContext : DbContext where TEntity : class
    {
        public bool AutoSaveChanges { get; set; } = true;
        public TContext Context { get; protected set; }
        private async Task SaveChanges(CancellationToken cancellationToken)
        {
            if (AutoSaveChanges)
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
        }
        public async virtual Task<Sonuc<TEntity>> YeniAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            KayitSonuc<TEntity> sonuc = null;
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            try
            {
                Context.Add(entity);
                await SaveChanges(cancellationToken);
                //sonuc = Sonuc<TEntity>.Tamam;
                sonuc.DonenNesne = entity;

            }
            catch (Exception)
            {
                //sonuc = KayitSonuc<TEntity>.Basarisiz(new List<Hata> { new Hata { Kod = "KimligeGoreKisiBulma", Tanim = $"Kayıt aranırken hata oluştu:{hata.Message}" } }.ToArray());
            }
            return sonuc;
        }

        public async virtual Task<Sonuc<TEntity>> DegistirAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            KayitSonuc<TEntity> sonuc = null;
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            try
            {
                Context.Attach(entity);
                Context.Update(entity);
                await SaveChanges(cancellationToken);
                //sonuc = Sonuc<TEntity>.Tamam;
                sonuc.DonenNesne = entity;

            }
            catch (DbUpdateConcurrencyException)
            {
                return Sonuc<TEntity>.Basarisiz(HataTanimlayici.ConcurrencyFailure());
            }
            catch (Exception hata)
            {
                return Sonuc<TEntity>.Basarisiz(new List<Hata> { new Hata { Kod = "KimligeGoreKisiBulma", Tanim = $"Kayıt aranırken hata oluştu:{hata.Message}" } }.ToArray());
            }
            return sonuc;
        }
        public async virtual Task<Sonuc<TEntity>> SilAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            KayitSonuc<TEntity> sonuc = null;
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            try
            {
                Context.Remove(entity);
                await SaveChanges(cancellationToken);
                //sonuc = Sonuc<TEntity>.Tamam;
                sonuc.DonenNesne = entity;
            }
            catch (DbUpdateConcurrencyException)
            {
                return Sonuc<TEntity>.Basarisiz(HataTanimlayici.ConcurrencyFailure());
            }
            catch (Exception hata)
            {
                return  Sonuc<TEntity>.Basarisiz(new List<Hata> { new Hata { Kod = "KimligeGoreKisiBulma", Tanim = $"Kayıt aranırken hata oluştu:{hata.Message}" } }.ToArray());
            }
            return sonuc;
        }


    }

    
}
