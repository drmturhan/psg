using Core.EntityFramework.SharedEntity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.DataAccess.Repositories
{
    public interface ICinsiyetRepository
    {
        Task<IEnumerable<KisiCinsiyet>> ListeGetirCinsiyetAsync();
    }

    public class CinsiyetRepository : ICinsiyetRepository
    {
        private readonly MTIdentityDbContext db;

        public CinsiyetRepository(MTIdentityDbContext db)
        {
            this.db = db;
        }
        public async Task<IEnumerable<KisiCinsiyet>> ListeGetirCinsiyetAsync()
        {
            return await db.Cinsiyetler.ToListAsync();
        }

        
    }
}
