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
using Identity.DataAccess.Mappers;

namespace Psg.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : MTSController
    {
        private readonly KullaniciYonetici userManager;
        private readonly SignInManager<Kullanici> signInManager;
        private readonly IEmailSender postaci;

        public UygulamaAyarlari uygulamaAyarlari { get; }

        public AccountController(
            KullaniciYonetici userManager,
            SignInManager<Kullanici> signInManager,

            IEmailSender postaci,
            IOptions<UygulamaAyarlari> uygulamaAyarConfig
            ) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.postaci = postaci;
            this.uygulamaAyarlari = uygulamaAyarConfig.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("girisyap", Name = "GirisYap")]
        public async Task<IActionResult> GirisYap([FromBody] GirisDto girisBilgileri, string returnUrl)
        {
            return await HataKontrolluDondur<Task<IActionResult>>(async () =>
            {
                var sonuc = await signInManager.PasswordSignInAsync(girisBilgileri.KullaniciAdi, girisBilgileri.Sifre, true, true);
                if (sonuc.Succeeded)
                    return await KullaniciBilgisiVeTokenYarat(girisBilgileri.KullaniciAdi);

                return BadRequest("Kullanıcı adı ve/veya şifre yanlış");
            });

        }
        [AllowAnonymous]
        [HttpPost]
        [Route("sifrekurtar", Name = "SifreKurtar")]
        public async Task<IActionResult> SifreKurtar([FromBody] SifreKurtarDto sifreKurtar, string returnUrl)
        {
            return await HataKontrolluDondur<Task<IActionResult>>(async () =>
            {

                var user = await userManager.FindByNameAsync(sifreKurtar.Eposta);
                if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                {
                    return Ok();
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                await postaci.SendEmailAsync(user.Email, "Şifre kurtarma",
                   "Şifre kurtarnaya devam etmek için <a href=\"" + callbackUrl + "\">tıklayınız</a>");
                return Ok();
                
            });

        }
        [Route("cikisyap", Name = "CikisYap")]
        public async Task<IActionResult> CikisYap()
        {
            return await KullaniciVarsaCalistir<IActionResult>(async () =>
            {
                await signInManager.SignOutAsync();
                return Ok();
            });

        }

        private async Task<IActionResult> KullaniciBilgisiVeTokenYarat(string kullaniciAdi, string returnUrl = null)
        {

            try
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
                var kullanici = bulunanKullanici.ToKullaniciBilgi();
                return Ok(new { tokenString, kullanici, returnUrl });
            }
            catch
            {

                return Unauthorized();
            }

        }


        [HttpPost]
        [AllowAnonymous]
        [Route("uyelikbaslat")]
        public async Task<IActionResult> UyelikBaslat([FromBody]UyelikYaratDto model)

        {
            return await HataKontrolluDondur<Task<IActionResult>>(async () =>
            {
                if (ModelState.IsValid)
                {

                    var user = model.ToEntity();
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
                        return BadRequest(hata);
                    }

                    AddErrors(result);
                }

                throw new BadRequestError("Üyelik başlatılamadı!");
            });
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
            return await HataKontrolluDondur<IActionResult>(async () =>
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
            return await HataKontrolluDondur<Task<IActionResult>>(async () =>
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
                    throw new BadRequestError("Yeniden kod gönderilemedi");
                }
            });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("kullaniciepostasinionayla")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            return await HataKontrolluDondur<Task<IActionResult>>(async () =>
            {
                if (userId == null || code == null)
                    return BadRequest("Kullanici bilgisi ve/veya kod yok!");

                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                    return BadRequest("Kullanıcı ve koda uyan kayt yok!");

                var result = await userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                    return RedirectPermanent(string.Format("http://localhost:4200/epostaonaylandi?kod={0}", user.SecurityStamp));
                throw new BadRequestError("Email onayı yapılamadı");
            });
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("sifreyisifirla")]
        public async Task<IActionResult> ResetPassword(SifreyiSifirlaDto model)
        {
            return await HataKontrolluDondur<Task<IActionResult>>(async () =>
            {

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
            });
        }



        [AllowAnonymous]
        [Route("kullaniciAdiVar")]
        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery]string kullaniciAdi)
        {
            return await HataKontrolluDondur<IActionResult>(async () =>
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