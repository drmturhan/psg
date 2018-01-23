using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Psg.Api.Dtos;
using Psg.Api.Extensions;
using Psg.Api.Models;
using Psg.Api.Repos;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Psg.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]

    public class AuthController : Controller
    {
        private readonly IAuthRepository repo;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AuthController(IAuthRepository repo, IMapper mapper, IConfiguration configuration)
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
                        new Claim(ClaimTypes.Name,bulunanKullanici.KullaniciAdi),
                    }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var kullanici = mapper.Map<KullaniciDetayDto>(bulunanKullanici);

            return Ok(new { tokenString, kullanici });
        }
        [HttpGet("{id}", Name = "KullanicBul")]
        public async Task<IActionResult> KullaniciBul(int id)
        {
            return Ok(await repo.KullaniciBulAsync(id));
        }

        [HttpPost("uyeol")]
        public async Task<IActionResult> Uyeol([FromBody] UyelikYaratDto dto)
        {
            if (dto == null) throw new Exception("Üyelik bilgisi boş!");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            dto.KullaniciAdi = dto.KullaniciAdi.ToLower();
            if (await repo.KullaniciVarAsync(dto.KullaniciAdi))
                ModelState.AddModelError(nameof(UyeYazDto.KullaniciAdi), "Kullanıcı adı alınmış");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var yaratilacakKullanici = mapper.Map<Kullanici>(dto);
            var yaratilanKullanici = await repo.UyeOlAsync(yaratilacakKullanici, dto.Sifre);
            var donecekKullanici = mapper.Map<KullaniciDetayDto>(yaratilanKullanici);
            return CreatedAtRoute("KullaniciGetir", new { controller = "Kullanicilar", id = yaratilanKullanici.Id }, donecekKullanici);
        }
    }
}