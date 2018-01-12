using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Psg.Api.Dtos;
using Psg.Api.Models;
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
        public async Task<IActionResult> Get(int id, [FromQuery] string neden)
        {
            var kayit = await kullaniciRepo.BulAsync(id);
            if (neden == "yaz")
            {
                var yazResource = mapper.Map<KullaniciYazDto>(kayit);
                return Ok(yazResource);
            }
            var resource = mapper.Map<KullaniciDetayDto>(kayit);
            return Ok(resource);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] KullaniciYazDto yazDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var userFromRepo = await kullaniciRepo.BulAsync(currentUserId);

                if (userFromRepo == null)
                    return NotFound($"{id} numaralı kullanıcı bulunamadı!");
                if (currentUserId != userFromRepo.Id)
                    return Unauthorized();

                mapper.Map(yazDto, userFromRepo);
                if (await kullaniciRepo.Kaydet())
                    return NoContent();
                else throw new Exception($"{id} numaralı kullanıcı bilgileri kaydedilemedi!");
            }
            catch (Exception hata)
            {
            }
            return BadRequest(ModelState);
        }
    }
}