using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Psg.Api.Dtos;
using Psg.Api.Models;
using Psg.Api.Repos;

namespace Psg.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository repo;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AuthController(IAuthRepository repo, IMapper mapper,IConfiguration configuration)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpPost("girisyap")]
        public async Task<IActionResult> GirisYap([FromBody] GirisDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bulunanKullanici = await repo.GirisYapAsync(dto.KullaniciAdi, dto.Sifre);
            if (bulunanKullanici == null)
                ModelState.AddModelError(nameof(dto.KullaniciAdi), "Kullanıcı adı ve/veya şifre yanlış");
            if (!ModelState.IsValid)
                return Unauthorized();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier,bulunanKullanici.Id.ToString()),
                        new Claim(ClaimTypes.Name,bulunanKullanici.Username),
                    }),
                Expires=DateTime.Now.AddHours(1),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha512)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenstring = tokenHandler.WriteToken(token);
            return Ok(new { tokenstring });
        }
        [HttpGet("{id}", Name = "KullanicBul")]
        public async Task<IActionResult> KullaniciBul(int id)
        {
            return Ok(await repo.KullaniciBulAsync(id));
        }

        [HttpPost("uyeol")]
        public async Task<IActionResult> Uyeol([FromBody] UyeYazDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            model.KullaniciAdi = model.KullaniciAdi.ToLower();
            if (await repo.KullaniciVarAsync(model.KullaniciAdi))
                ModelState.AddModelError(nameof(UyeYazDto.KullaniciAdi), "Kullanıcı adı alınmış");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var yaratilacakKullanici = mapper.Map<Kullanici>(model);
            var yaratilanKullanici = await repo.UyeOlAsync(yaratilacakKullanici, model.Sifre);
            return StatusCode(201);
        }
    }
}