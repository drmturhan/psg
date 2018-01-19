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
using Psg.Api.Repos;

namespace Psg.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/arkadasliklar")]
    [Authorize]
    public class ArkadasliklarController : Controller
    {
        private readonly IArkadaslikRepository arkadaslikRepo;
        private readonly IMapper mapper;

        public ArkadasliklarController(IArkadaslikRepository arkdaslikRepo,IMapper mapper)
        {
            this.arkadaslikRepo = arkdaslikRepo;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(ArkadaslikSorgusu sorgu)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (currentUserId <= 0)
                    return Unauthorized();
                sorgu.TeklifEdenKullaniciNo = currentUserId;
                var arkadaslikListesi = await arkadaslikRepo.ListeGetirTekliflerAsync(sorgu);
                var resources = mapper.Map<IEnumerable<ArkadaslarimListeDto>>(arkadaslikListesi);
                return Ok(resources);
            }
            catch (Exception hata)
            {
                return BadRequest(hata.Message);
            }

        }
    }
}