using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Psg.Api.Dtos;
using Psg.Api.Repos;

namespace Psg.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/kullanicilar")]
    [Authorize]
    public class KullanicilarController : Controller
    {
        private readonly KullaniciRepository kullaniciRepo;
        private readonly IMapper mapper;

        public KullanicilarController(KullaniciRepository kullaniciRepo, IMapper mapper)
        {
            this.kullaniciRepo = kullaniciRepo;
            this.mapper = mapper;
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var kayitlar = await kullaniciRepo.ListeGetirKullanicilarTumuAsync();
            var resources = mapper.Map<IEnumerable<KullaniciListeDto>>(kayitlar);
            return Ok(resources);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var kayit = await kullaniciRepo.BulAsync(id);
            var resource = mapper.Map<KullaniciDetayDto>(kayit);
            return Ok(resource);
        }
    }
}