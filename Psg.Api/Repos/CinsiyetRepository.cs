using Microsoft.EntityFrameworkCore;
using Psg.Api.Data;
using Psg.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Psg.Api.Repos
{
    public interface ICinsiyetRepository
    {
        Task<IEnumerable<Cinsiyet>> ListeGetirCinsiyetAsync();
    }

    public class CinsiyetRepository : ICinsiyetRepository
    {
        private readonly IdentityContext db;

        public CinsiyetRepository(IdentityContext db)
        {
            this.db = db;
        }
        public async Task<IEnumerable<Cinsiyet>> ListeGetirCinsiyetAsync()
        {
            return await db.Cinsiyetler.ToListAsync();
        }
    }

}
