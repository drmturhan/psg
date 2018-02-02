using Core.Base;
using System;

namespace Core.EntityFramework.SharedEntity
{
    public class ArkadaslikTeklif<TKey> : EBase, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
        public int ArkadaslikIsteyenNo { get; set; }

        public int TeklifEdilenNo { get; set; }

        public DateTime IstekTarihi { get; set; }
        public DateTime? CevapTarihi { get; set; }
        public bool? Karar { get; set; }
        public TKey Kimlik { get { return Id; } set { Id = value; } }
    }
}
