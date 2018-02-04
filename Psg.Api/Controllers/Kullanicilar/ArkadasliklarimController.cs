﻿using AutoMapper;
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
    [Route("api/arkadasliklar")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ArkadaslikliklarimController : MTController
    {
        private readonly IArkadaslikRepository arkadaslikRepo;
        private readonly IUrlHelper urlHelper;
        private readonly IPropertyMappingService propertyMappingService;
        private readonly ITypeHelperService typeHelperService;

        public ArkadaslikliklarimController(
            IArkadaslikRepository arkdaslikRepo,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService) : base("Arkadaşlıklar")
        {
            this.arkadaslikRepo = arkdaslikRepo;
            this.urlHelper = urlHelper;
            this.propertyMappingService = propertyMappingService;
            this.typeHelperService = typeHelperService;
            propertyMappingService.AddMap<ArkadaslarimListeDto, ArkadaslikTeklif>(ArkadaslikTeklifPropertyMap.Values);

        }


        [HttpGet()]
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
    }

}