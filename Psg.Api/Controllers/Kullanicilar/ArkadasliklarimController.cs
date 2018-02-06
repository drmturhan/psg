using AutoMapper;
using Core.Base;
using Core.EntityFramework;
using Identity.DataAccess;
using Identity.DataAccess.Mappers;
using Identity.DataAccess.Dtos;
using Identity.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psg.Api.Base;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Psg.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/arkadasliklarim")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ArkadaslikliklarimController : MTController
    {
        private readonly IArkadaslikRepository arkadaslikRepo;
        private readonly IUrlHelper urlHelper;

        public ArkadaslikliklarimController(
            IArkadaslikRepository arkdaslikRepo,
            IUrlHelper urlHelper) : base("Arkadaşlıklar")
        {
            this.arkadaslikRepo = arkdaslikRepo;
            this.urlHelper = urlHelper;
        }


        [HttpGet()]
        public async Task<IActionResult> Get(ArkadaslikSorgusu sorgu)
        {

            return await HataKontrolluCalistir<Task<IActionResult>>(async () =>
            {

                try
                {
                    var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    if (currentUserId <= 0)
                        return Unauthorized();
                    sorgu.KullaniciNo = currentUserId;
                    //En az biri aktif kullanici degilse problem var!
                    //if (sorgu.TeklifEdenKullaniciNo != currentUserId && sorgu.CevapVerecekKullaniciNo != currentUserId) return Unauthorized();
                    var kayitlar = await arkadaslikRepo.ListeGetirTekliflerAsync(sorgu);
                    var sby = new StandartSayfaBilgiYaratici(sorgu, "ArkadasliklarListesi", urlHelper);
                    Response.Headers.Add("X-Pagination", kayitlar.SayfalamaMetaDataYarat<ArkadaslikTeklif>(sby));

                    var sonuc = ListeSonuc<ArkadaslikTeklif>.IslemTamam(kayitlar);
                    ListeSonuc<ArkadaslarimListeDto> donecekListe = sonuc.ToDto();
                    return Ok(donecekListe.ShapeData(sorgu.Alanlar));
                }
                catch (Exception hata)
                {
                    return BadRequest(hata.Message);
                }
            });
        }
        [HttpPost("{isteyenId}/teklif/{cevaplayanId}")]
        public async Task<IActionResult> TeklifEt(int isteyenId, int cevaplayanId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != isteyenId)
                return Unauthorized();
            if (isteyenId == cevaplayanId)
                return BadRequest("Kendinize arkadaşlık teklif edemezsiniz!!!");
            var teklifZatenVar = await arkadaslikRepo.TeklifiBulAsync(isteyenId, cevaplayanId);

            if (teklifZatenVar != null)
                return BadRequest("Bu kullanıcıya zaten arkadaşlık teklif ettiniz");
            var banaTeklifEtti = await arkadaslikRepo.TeklifiBulAsync(cevaplayanId, isteyenId);
            if (banaTeklifEtti != null)
                return BadRequest("Bu kullanıcı size zaten arkadaşlık teklif etti. Lütfen cevap verin...");
            if (await arkadaslikRepo.KullaniciBulAsync(cevaplayanId) == null)
                return NotFound();

            ArkadaslikTeklif yeniTeklif = new ArkadaslikTeklif
            {
                TeklifEdenNo = isteyenId,
                TeklifEdilenNo = cevaplayanId,
                IstekTarihi = DateTime.Now
            };
            await arkadaslikRepo.EkleAsync<ArkadaslikTeklif>(yeniTeklif);
            if (await arkadaslikRepo.KaydetAsync())
                return Ok();
            return BadRequest("Arkadaşlık teklifi yapılamad!");


        }
        [HttpPost("{isteyenId}/teklifiptal/{cevaplayanId}")]
        public async Task<IActionResult> TeklifIptalEt(int isteyenId, int cevaplayanId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);



            var teklif = await arkadaslikRepo.TeklifiBulAsync(isteyenId, cevaplayanId);
            if (teklif == null)
                return NotFound("Arkadaşlık bilgisine ulaşılamadı");
            if (await arkadaslikRepo.KullaniciBulAsync(cevaplayanId) == null)
                return NotFound();

            if (teklif.IptalEdildi == true)
                return BadRequest("Teklif zaten iptal edilmiş durumda!");
            teklif.IptalEdildi = true;
            teklif.IptalEdenKullaniciNo = currentUserId;
            teklif.IptalTarihi = DateTime.Now;

            if (await arkadaslikRepo.KaydetAsync())
                return Ok();
            return BadRequest("Arkadaşlık teklifi iptal edilemedi!");

        }
        [HttpPost("{isteyenId}/kararver/{cevaplayanId}")]
        public async Task<IActionResult> TeklifeKararVer(int isteyenId, int cevaplayanId, [FromBody] bool karar)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);



            var teklif = await arkadaslikRepo.TeklifiBulAsync(isteyenId, cevaplayanId);
            if (teklif == null)
                return NotFound("Arkadaşlık bilgisine ulaşılamadı");
            if (await arkadaslikRepo.KullaniciBulAsync(cevaplayanId) == null)
                return NotFound();

            if (teklif.IptalEdildi == true)
                return BadRequest("Teklif zaten iptal edilmiş durumda!");
            teklif.Karar = karar;
            teklif.CevapTarihi = DateTime.Now;

            if (await arkadaslikRepo.KaydetAsync())
                return Ok();
            return BadRequest("Arkadaşlık teklifi kararı kaydedilemedi!");

        }
    }

}