using System;
using System.Collections.Generic;
using System.Linq;

namespace Psg.Api.Seeds
{
    public class SeederManager : ISeederManager
    {
        
        
        public void SeedAll()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.GetInterface("ISeeder")!=null);
            foreach (var sinif in types)
            {
                if (sinif.IsClass)
                {
                    var instance = Activator.CreateInstance(sinif);
                    if (instance != null)
                        (instance as ISeeder).Seed();
                }
            }
        }
    }
}
