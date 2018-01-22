using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psg.Api.Base;
using Psg.Api.Dtos;
using Psg.Api.Helpers;
using Psg.Api.Models;
using Psg.Api.Repos;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Psg.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/arkadasliklar")]
    [Authorize]
    public class ArkadasliklarController : MTController
    {
        private readonly IArkadaslikRepository arkadaslikRepo;
        private readonly IMapper mapper;
        private readonly IUrlHelper urlHelper;
        private readonly IPropertyMappingService propertyMappingService;
        private readonly ITypeHelperService typeHelperService;

        public ArkadasliklarController(
            IArkadaslikRepository arkdaslikRepo,
            IMapper mapper,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService) : base("Arkadaşlıklar")
        {
            this.arkadaslikRepo = arkdaslikRepo;
            this.mapper = mapper;
            this.urlHelper = urlHelper;
            this.propertyMappingService = propertyMappingService;
            this.typeHelperService = typeHelperService;
            propertyMappingService.AddMap<ArkadaslarimListeDto, ArkadaslikTeklif>(ArkadaslikTeklifPropertyMap.Values);

        }


        [HttpGet(Name = "ArkadasliklarListesi")]
        public async Task<IActionResult> Get(ArkadaslikSorgusu sorgu)
        {

            return await HataKontrolluCalistir<Task<IActionResult>>(async () =>
            {
                if (!propertyMappingService.ValidMappingsExistsFor<ArkadaslarimListeDto, ArkadaslikTeklif>(sorgu.SiralamaCumlesi))
                    return BadRequest(Sonuc.Basarisiz(new Hata[] { new Hata { Kod = "ArkadaşListesi", Tanim = "Sıralama bilgisi yanlış!" } }));

                if (!typeHelperService.TryHastProperties<ArkadaslarimListeDto>(sorgu.Alanlar))
                    return BadRequest(Sonuc.Basarisiz(new Hata[] { new Hata { Kod = "ArkadaşListesi", Tanim = "Gösterilmek istenen alanlar hatalı!" } }));
                try
                {
                    var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    if (currentUserId <= 0)
                        return Unauthorized();
                    sorgu.TeklifEdenKullaniciNo = currentUserId;
                    var kayitlar = await arkadaslikRepo.ListeGetirTekliflerAsync(sorgu);
                    var sby = new StandartSayfaBilgiYaratici(sorgu, "ArkadasliklarListesi", urlHelper);
                    Response.Headers.Add("X-Pagination", kayitlar.SayfalamaMetaDataYarat<ArkadaslikTeklif>(sby));

                    var sonuc = ListeSonuc<ArkadaslikTeklif>.IslemTamam(kayitlar);
                    ListeSonuc<ArkadaslarimListeDto> donecekListe = mapper.Map<ListeSonuc<ArkadaslarimListeDto>>(sonuc);
                    return Ok(donecekListe.ShapeData(sorgu.Alanlar));
                }
                catch (Exception hata)
                {
                    return BadRequest(hata.Message);
                }
            });
        }
    }

}