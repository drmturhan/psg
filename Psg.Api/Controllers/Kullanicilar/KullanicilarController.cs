using Core.Base;
using Core.EntityFramework;
using Identity.DataAccess;
using Identity.DataAccess.Dtos;
using Identity.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psg.Api.Base;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.DataAccess.Mappers;
using Psg.Api.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Psg.Api.Controllers
{
    [ServiceFilter(typeof(KullaniciAktiviteleriniTakipEt))]

    [Produces("application/json")]
    [Route("api/kullanicilar")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class KullanicilarController : MTController
    {
        private readonly IKullaniciRepository kullaniciRepo;
        private readonly IArkadaslikRepository arkadaslikRepo;
        private readonly IUrlHelper urlHelper;
        private readonly IPropertyMappingService propertyMappingService;
        private readonly ITypeHelperService typeHelperService;

        public KullanicilarController(
            IKullaniciRepository kullaniciRepo,
            IArkadaslikRepository arkdaslikRepo,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService) : base("Kullanıcı")
        {
            this.kullaniciRepo = kullaniciRepo;
            this.arkadaslikRepo = arkdaslikRepo;
            this.urlHelper = urlHelper;
            this.propertyMappingService = propertyMappingService;
            this.typeHelperService = typeHelperService;
            propertyMappingService.AddMap<KullaniciListeDto, Kullanici>(KullaniciPropertyMap.Values);
        }

        [HttpGet(Name = "Kullanicilar")]
        public async Task<IActionResult> Get(KullaniciSorgu sorgu)
        {
            return await KullaniciVarsaCalistir<Task<IActionResult>>(async () =>
            {
                if (!propertyMappingService.ValidMappingsExistsFor<KullaniciListeDto, Kullanici>(sorgu.SiralamaCumlesi))
                    return BadRequest(Sonuc.Basarisiz(new Hata[] { new Hata { Kod = "KullanicListesi", Tanim = "Sıralama bilgisi yanlış!" } }));

                if (!typeHelperService.TryHastProperties<KullaniciListeDto>(sorgu.Alanlar))
                    return BadRequest(Sonuc.Basarisiz(new Hata[] { new Hata { Kod = "KullanicListesi", Tanim = "Gösterilmek istenen alanlar hatalı!" } }));

                var kayitlar = await kullaniciRepo.ListeGetirKullanicilarTumuAsync(sorgu);

                var sby = new StandartSayfaBilgiYaratici(sorgu, "Kullanicilar", urlHelper);
                Response.Headers.Add("X-Pagination", kayitlar.SayfalamaMetaDataYarat<Kullanici>(sby));

                var sonuc = ListeSonuc<Kullanici>.IslemTamam(kayitlar);
                ListeSonuc<KullaniciListeDto> donecekListe = sonuc.ToKullaniciDetayDto();
                return Ok(donecekListe.ShapeData(sorgu.Alanlar));
            });
        }

        [AllowAnonymous]
        [Route("kullaniciadikullanimda")]
        [HttpGet()]
        public async Task<IActionResult> KullaniciAdiKullaniliyormu([FromQuery]string kullaniciAdi)
        {
            return await HataKontrolluCalistir<IActionResult>(async () =>
            {
                if (string.IsNullOrEmpty(kullaniciAdi.Trim()))
                    return BadRequest(Sonuc<KullaniciYazDto>.Basarisiz(new Hata[] { new Hata { Kod = "", Tanim = "Kullanıcı adı boş olamaz!" } }));
                var kullaniciVar = await kullaniciRepo.KullaniciAdiKullanimdaAsync(kullaniciAdi);

                return Ok(kullaniciVar);
            });
        }
        [AllowAnonymous]
        [Route("epostakullanimda")]
        [HttpGet()]
        public async Task<IActionResult> EpostaKullaniliyormu([FromQuery]string eposta)
        {
            return await HataKontrolluCalistir<IActionResult>(async () =>
            {
                if (string.IsNullOrEmpty(eposta.Trim()))
                    return BadRequest(Sonuc<KullaniciYazDto>.Basarisiz(new Hata[] { new Hata { Kod = "", Tanim = "Kullanıcı adı boş olamaz!" } }));
                var kullaniciVar = await kullaniciRepo.EpostaKullanimdaAsync(eposta);

                return Ok(kullaniciVar);
            });
        }


        [HttpGet("{id}", Name = "KullaniciGetir")]
        public async Task<IActionResult> Get(int id, [FromQuery] string neden, [FromQuery] string alanlar)
        {
            return await HataKontrolluCalistir<IActionResult>(async () =>
            {
                if (id <= 0)
                    return BadRequest(Sonuc<KullaniciYazDto>.Basarisiz(new Hata[] { new Hata { Kod = "", Tanim = SonucMesajlari.Liste[MesajAnahtarlari.SifirdanBuyukDegerGerekli] } }));

                var kayit = await kullaniciRepo.BulAsync(id);
                if (kayit == null)
                    return NotFound();
                if (neden == "yaz")
                {
                    
                    var yazSonucDto = KayitSonuc<KullaniciYazDto>.IslemTamam(kayit.ToDto());
                    return Ok(yazSonucDto.ShapeData(alanlar));
                }
                var resource = KayitSonuc<KullaniciDetayDto>.IslemTamam(kayit.ToKullaniciDetayDto());
                return Ok(resource.ShapeData(alanlar));

            });
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

                KullaniciMappers.Kopyala(yazDto, userFromRepo);
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
                    kullaniciRepo.KisileriniSil(dbdekiKullanici.Kisi);
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
                TeklifEdenNo= isteyenId,
                TeklifEdilenNo = cevaplayanId,
                IstekTarihi = DateTime.Now
            };
            await arkadaslikRepo.EkleAsync<ArkadaslikTeklif>(yeniTeklif);
            if (await arkadaslikRepo.KaydetAsync())
                return Ok();
            return BadRequest("Arkadaşlık teklifi yapılamad!");


        }
    }
 

}