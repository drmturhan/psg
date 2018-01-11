﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Psg.Api.Data
{
    public class IndetityContextFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            bool useSqLite = false;
            //SqlLite için ilgili nuget leri eklemek gerek.
            bool.TryParse(configuration["Data:useSqLite"], out useSqLite);
            string baglantiSatiri = useSqLite ? configuration["Data:SqlLiteConnectionString"] : configuration["Data:SqlServerConnectionString"];

            if (string.IsNullOrEmpty(baglantiSatiri))
                throw new Exception(baglantiSatiri + " boş.");

            DbContextOptionsBuilder builder = null;
            if (useSqLite)
            {
                //Nuget leri ekledikten sonra burayı ayarlayabiliriz.

            }
            else
            {
                builder = new DbContextOptionsBuilder<IdentityContext>();
                builder.UseSqlServer(baglantiSatiri);
            }
            return new IdentityContext(builder.Options as DbContextOptions<IdentityContext>);
        }
    }
}
