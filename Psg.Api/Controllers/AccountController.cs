using System;
using System.Threading.Tasks;
using AutoMapper;
using Identity.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Psg.Api.Base;
using Psg.Api.Dtos;
using Psg.Api.Extensions;
using Core.Base;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Core.EntityFramework;
using Identity.DataAccess.Dtos;

namespace Psg.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : MTController
    {
        private readonly KullaniciYonetici userManager;
        private readonly SignInManager<Kullanici> signInManager;
        private readonly IMapper mapper;
        private readonly IEmailSender postaci;

        public UygulamaAyarlari uygulamaAyarlari { get; }

        public AccountController(
            KullaniciYonetici userManager,
            SignInManager<Kullanici> signInManager,
            IMapper mapper,
            IEmailSender postaci,
            IOptions<UygulamaAyarlari> uygulamaAyarConfig
            ) : base("Hesap")
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.postaci = postaci;
            this.uygulamaAyarlari = uygulamaAyarConfig.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("girisyap", Name = "GirisYap")]
        public async Task<IActionResult> GirisYap([FromBody] GirisDto girisBilgileri, string returnUrl)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            var sonuc = await signInManager.PasswordSignInAsync(girisBilgileri.KullaniciAdi, girisBilgileri.Sifre, true, true);
            if (sonuc.Succeeded)
                return await KullaniciBilgisiVeTokenDondur(girisBilgileri.KullaniciAdi);

            return Unauthorized();

        }
        private async Task<IActionResult> KullaniciBilgisiVeTokenDondur(string kullaniciAdi)
        {
            var bulunanKullanici = await userManager.KullaniciyiGetirKullaniciAdinaGoreAsync(kullaniciAdi);
            if (bulunanKullanici == null) return Unauthorized();

            var claims = new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier,bulunanKullanici.Id.ToString()),
                        new Claim(ClaimTypes.Name,bulunanKullanici.UserName),
                        new Claim(ClaimTypes.GivenName,bulunanKullanici.Kisi.Ad),
                        new Claim(ClaimTypes.Surname,bulunanKullanici.Kisi.Soyad),
                        new Claim(ClaimTypes.DateOfBirth,bulunanKullanici.Kisi.DogumTarihi.ToShortDateString()),
                        new Claim(ClaimTypes.Email,bulunanKullanici.Email),
                        new Claim(ClaimTypes.Gender,bulunanKullanici.Kisi.Cinsiyeti.CinsiyetAdi),
                        new Claim("kisiNo",bulunanKullanici.KisiNo.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(uygulamaAyarlari.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                uygulamaAyarlari.JwtIssuer,
                uygulamaAyarlari.JwtIssuer,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var kullanici = mapper.Map<KullaniciBilgi>(bulunanKullanici);
            return Ok(new { tokenString, kullanici });

        }


        [HttpPost]
        [AllowAnonymous]
        [Route("uyelikbaslat")]
        public async Task<IActionResult> UyelikBaslat([FromBody]UyelikYaratDto model)
        {

            if (ModelState.IsValid)
            {

                var user = mapper.Map<Kullanici>(model);
                user.YaratilmaTarihi = DateTime.Now;
                IdentityResult result = null;

                try
                {
                    result = await userManager.CreateAsync(user, model.Sifre);
                    if (result.Succeeded)
                    {
                        await EPostaAktivasyonKoduPostala(user);

                        return Ok();
                    }
                }
                catch (Exception hata)
                {
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return BadRequest(ModelState);
        }

        private async Task EPostaAktivasyonKoduPostala(Kullanici user)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.EmailConfirmationLink(user.Id.ToString(), code, Request.Scheme);
            await postaci.SendEmailAsync(user.Email, "Kullanıcı eposta aktivasyonu", string.Format("<a href=\"{0}\">Kullanıcıyı onayla</a>", callbackUrl));
        }
        [AllowAnonymous]
        [Route("guvenlikkodudogrumu")]
        [HttpGet()]
        public async Task<IActionResult> GuvenlikKoduDogrumu([FromQuery]string kod)
        {
            return await HataKontrolluCalistir<IActionResult>(async () =>
            {
                if (string.IsNullOrEmpty(kod.Trim()))
                    return BadRequest("Kod boş olamaz!");
                var sonuc = await userManager.KullaniciGuvenlikKoduDogrumu(kod);
                if (sonuc)
                    return Ok();
                else
                    return BadRequest();

            });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("tekrarepostaonaykodupostala")]
        public async Task<IActionResult> YenidenKodGoner(string kullaniciAdi, string eposta)
        {
            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(eposta))
            {
                return BadRequest("Kullanici adı ve/veya epostası yanlış!");
            }
            Kullanici user = null;
            user = await userManager.FindByNameAsync(kullaniciAdi);
            if (user == null)
            {
                return BadRequest("Kullanıcı adına uyan kayıt yok!");
            }
            if (user.Email != eposta)
                return BadRequest("Kullanici adı ve/veya epostası yanlış!");
            try
            {
                await EPostaAktivasyonKoduPostala(user);

                return Ok();
            }
            catch
            {
                return BadRequest("Email onayı yapılamadı");
            }

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("kullaniciepostasinionayla")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest("Kullanici bilgisi ve/veya kod yok!");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("Kullanıcı ve koda uyan kayt yok!");
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectPermanent(string.Format("http://localhost:4200/epostaonaylandi?kod={0}", user.SecurityStamp));
            return BadRequest("Email onayı yapılamadı");
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("sifreyisifirla")]
        public async Task<IActionResult> ResetPassword(SifreyiSifirlaDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await userManager.FindByEmailAsync(model.Eposta);
            if (user == null)
            {
                return NotFound("Kullanıcı yok!");
            }
            var result = await userManager.ResetPasswordAsync(user, model.Kod, model.Sifre);
            if (result.Succeeded)
            {
                return Ok();
            }
            AddErrors(result);
            return BadRequest(ModelState);
        }



        [AllowAnonymous]
        [Route("kullaniciAdiVar")]
        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery]string kullaniciAdi)
        {
            return await HataKontrolluCalistir<IActionResult>(async () =>
            {
                if (string.IsNullOrEmpty(kullaniciAdi.Trim()))
                    return BadRequest(Sonuc<KullaniciYazDto>.Basarisiz(new Hata[] { new Hata { Kod = "", Tanim = "Kullanıcı adı boş olamaz!" } }));
                var kullaniciVar = await userManager.KullaniciAdiKullanimdaAsync(kullaniciAdi);

                return Ok(kullaniciVar);
            });
        }


        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        #endregion


    }
}