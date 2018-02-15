using System.Threading.Tasks;
using AutoMapper;
using Identity.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Psg.Api.Base;
using Psg.Api.Dtos;
using Psg.Api.Repos;
using Identity.DataAccess.Mappers;
using Microsoft.AspNetCore.Authorization;

namespace Psg.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/cinsiyetler")]
    
    public class CinsiyetlerController : MTController
    {
        private readonly ICinsiyetRepository repo;


        public CinsiyetlerController(ICinsiyetRepository repo) 
        {
            this.repo = repo;
        }
        [HttpGet()]
        public async Task<IActionResult> Get()
        {

            return await HataKontrolluDondur<Task<IActionResult>>(async () =>
            {
                var kayitlar = await repo.ListeGetirCinsiyetAsync();
                return Ok(kayitlar.ToDto());
            });
        }

    }
}