using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psg.Api.Base;
using Psg.Api.Dtos;
using Psg.Api.Helpers;
using Psg.Api.Models;
using Psg.Api.Repos;
using Psg.Api.Helpers;

namespace Psg.Api.Controllers
{
    [ServiceFilter(typeof(KullaniciAktiviteleriniTakipEt))]
    [Produces("application/json")]
    [Route("api/kullanicilar")]
    [Authorize]
    public class KullanicilarController : MTController
    {
        private readonly IKullaniciRepository kullaniciRepo;
        private readonly IArkadaslikRepository arkadaslikRepo;
        private readonly IMapper mapper;
        private readonly IUrlHelper urlHelper;
        private readonly IPropertyMappingService propertyMappingService;
        private readonly ITypeHelperService typeHelperService;

        public KullanicilarController(
            IKullaniciRepository kullaniciRepo,
            IArkadaslikRepository arkdaslikRepo,
            IMapper mapper,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService) : base("Kullanıcı")
        {
            this.kullaniciRepo = kullaniciRepo;
            this.arkadaslikRepo = arkdaslikRepo;
            this.mapper = mapper;
            this.urlHelper = urlHelper;
            this.propertyMappingService = propertyMappingService;
            this.typeHelperService = typeHelperService;
            propertyMappingService.AddMap<KullaniciListeDto, Kullanici>(KullaniciPropertyMap.Values);
        }

        [HttpGet]
        public async Task<IActionResult> Get(KullaniciSorgu sorgu)
        {
            return await HataKontrolluCalistir<Task<IActionResult>>(async () =>
            {
                if (!propertyMappingService.ValidMappingsExistsFor<KullaniciListeDto, Kullanici>(sorgu.SiralamaCumlesi))
                    return BadRequest(Sonuc.Basarisiz(new Hata[] { new Hata { Kod = "CinsiyetListesi", Tanim = "Sıralama bilgisi yanlış!" } }));

                if (!typeHelperService.TryHastProperties<KullaniciListeDto>(sorgu.Alanlar))
                    return BadRequest(Sonuc.Basarisiz(new Hata[] { new Hata { Kod = "KullanicListesi", Tanim = "Gösterilmek istenen alanlar hatalı!" } }));

                var kayitlar = await kullaniciRepo.ListeGetirKullanicilarTumuAsync(sorgu);

                var sby = new StandartSayfaBilgiYaratici(sorgu, "ListeGetirCinsiyetler", urlHelper);
                Response.Headers.Add("X-Pagination", kayitlar.SayfalamaMetaDataYarat<Kullanici>(sby));

                var sonuc = ListeSonuc<Kullanici>.IslemTamam(kayitlar);
                ListeSonuc<KullaniciListeDto> donecekListe = mapper.Map<ListeSonuc<KullaniciListeDto>>(sonuc);

                return Ok(donecekListe.ShapeData(sorgu.Alanlar));
            });
        }

        [HttpGet("{id}", Name = "KullaniciGetir")]
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
                if (await kullaniciRepo.KaydetAsync())
                    return NoContent();
                else throw new Exception($"{id} numaralı kullanıcı bilgileri kaydedilemedi!");
            }
            catch (Exception hata)
            {
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Sil(int id)
        {
            if (id <= 0)
                BadRequest("Silmek istediğiniz kullanıcı numarası yanlış!!");
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId == id)
                return BadRequest("Kendinizi silemezsiniz!!");
            var dbdekiKullanici = await kullaniciRepo.BulAsync(id);
            if (dbdekiKullanici == null)
                NotFound("Silmek istediğiniz kullanıcı bulunamadı!");
            kullaniciRepo.Sil<Kullanici>(dbdekiKullanici);
            if (await kullaniciRepo.KaydetAsync())
            {
                try
                {
                    kullaniciRepo.KisileriniSil(dbdekiKullanici.KisiBilgisi);
                    await kullaniciRepo.KaydetAsync();
                    return NoContent();
                }
                catch
                {
                    return NoContent();
                }
            }
            return BadRequest("Kullanıcı silinemedi!");
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
            if (await arkadaslikRepo.KullaniciBulAsync(cevaplayanId) == null)
                return NotFound();

            ArkadaslikTeklif yeniTeklif = new ArkadaslikTeklif
            {
                ArkadaslikIsteyenNo = isteyenId,
                TeklifEdilenNo = cevaplayanId,
                IstekTarihi = DateTime.Now
            };
            await arkadaslikRepo.EkleAsync<ArkadaslikTeklif>(yeniTeklif);
            if (await arkadaslikRepo.KaydetAsync())
                return Ok();
            return BadRequest("Arkadaşlık teklifi yapılamad!");


        }
    }
    public class KullaniciSorgu : SorguBase { }

}