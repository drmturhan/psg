using Psg.Api.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic.Core;


namespace Psg.Api.Extensions
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; private set; }
        public bool Revert { get; private set; }
        public PropertyMappingValue(IEnumerable<string> destinationProperties, bool revert = false)
        {
            DestinationProperties = destinationProperties;
            Revert = revert;
        }
    }
    public static class IQueryableExtensions
    {
        public static IQueryable<T> SiralamayiAyarla<T>(this IQueryable<T> kaynak, string orderBy, Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (kaynak == null)
                throw new ArgumentNullException("kaynak boş olamaz");
            if (mappingDictionary == null)
                throw new ArgumentNullException("ALan harita listesi boş olamaz");
            if (string.IsNullOrWhiteSpace(orderBy))
                return kaynak;
            var orderByAfterSplit = orderBy.Split(',');
            foreach (var orderByClause in orderByAfterSplit)
            {
                var trimmedOrderByClause = orderByClause.Trim();
                var orderDescending = trimmedOrderByClause.EndsWith(" azalan");

                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ? trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

                if (!mappingDictionary.ContainsKey(propertyName))
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                var propertyMappingValue = mappingDictionary[propertyName];
                if (propertyMappingValue == null)
                    throw new ArgumentException("Property mapping value");

                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    if (propertyMappingValue.Revert)
                        orderDescending = !orderDescending;
                    kaynak = kaynak.OrderBy(destinationProperty + (orderDescending ? " descending" : " ascending"));

                }
            }
            return kaynak;
        }
    }

    public static class ExceptionExtensions
    {
        public static void SonucaYaz<T>(this Exception hata, Sonuc<T> sonuc)
        {
            if (sonuc == null) return;
            List<Hata> hatalar = new List<Hata> { new Hata { Kod = "", Tanim = hata.Message } };
            hatalar.ForEach(h => sonuc.Hatalar.Add(h));
            if (hata.InnerException != null)
                hata.InnerException.SonucaYaz<T>(sonuc);
        }
    }

    public static class PasswordHashExtensions
    {
        public static void CreateHashes(this string sifre, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(sifre));
            }
        }

        public static void VerifyPasswordHash(this string sifre, byte[] passwordHash, byte[] passswordSalt, out bool sonuc)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(sifre));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) sonuc = false;
                }
            }
            sonuc = true;
        }
    }

}
