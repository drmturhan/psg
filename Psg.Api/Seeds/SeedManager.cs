using System;
using System.Collections.Generic;
using System.Linq;

namespace Psg.Api.Seeds
{
    public class SeederManager : ISeederManager
    {

        public SortedList<int, ISeeder> seeders = new SortedList<int, ISeeder>();

        public void SeedAll()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.GetInterface("ISeeder") != null);
            foreach (var sinif in types)
            {
                if (sinif.IsClass)
                {
                    ISeeder instance = Activator.CreateInstance(sinif) as ISeeder;
                    seeders.Add(instance.Oncelik, instance);
                }
            }
            foreach (var seederKey in seeders.Keys)
            {
                seeders[seederKey].Seed();
            }
        }
    }
}
