using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Psg.Api.Dtos;
using Psg.Api.Models;
using Psg.Api.Repos;

namespace Psg.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/cinsiyetler")]
    public class CinsiyetlerController : Controller
    {
        private readonly ICinsiyetRepository repo;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public CinsiyetlerController(ICinsiyetRepository repo, IMapper mapper, IConfiguration configuration)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.configuration = configuration;
        }
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var liste = await repo.ListeGetirCinsiyetAsync();
            if (liste != null)
                return Ok(liste);
            else
                return BadRequest("Cinsiyet listesi alınamadı!");
        }

    }
}