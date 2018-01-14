using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Psg.Api.Dtos;
using Psg.Api.Helpers;
using Psg.Api.Models;
using Psg.Api.Repos;

namespace Psg.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/kullanicilar/{kullaniciNo}/fotograflari")]
    public class KullaniciFotograflariController : Controller
    {
        private readonly KullaniciRepository repo;
        private readonly IMapper mapper;
        private readonly IOptions<CloudinarySettings> cloudinaryConfig;
        private readonly Cloudinary cloudinary;
        public KullaniciFotograflariController(KullaniciRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.cloudinaryConfig = cloudinaryConfig;
            Account acc = new Account(cloudinaryConfig.Value.CloudName, cloudinaryConfig.Value.ApiKey, cloudinaryConfig.Value.ApiSecret);
            cloudinary = new Cloudinary(acc);
        }
        [HttpGet(Name = "KullaniciFotografiniAl")]
        public async Task<IActionResult> KullaniciFotografiniAl(int kullaniciNo)
        {
            var entity = await repo.FotografBulAsync(kullaniciNo);
            var foto = mapper.Map<FotoOkuDto>(entity);
            return Ok(foto);
        }

        [HttpPost]
        public async Task<IActionResult> KullaniciyaFotografEkle(int kullaniciNo, FotografYazDto dto)
        {
            var kullanici = await repo.BulAsync(kullaniciNo);
            if (kullanici == null)
                return BadRequest("Kullanıcı bulunamadı!");
            var aktifKullaniciNo = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (aktifKullaniciNo != kullanici.Id)
                return Unauthorized();

            var dosya = dto.File;
            var yuklemeSonucu = new ImageUploadResult();
            if (dosya.Length > 0)
            {
                using (var stream = dosya.OpenReadStream())
                {
                    var yuklemePrametreleri = new ImageUploadParams()
                    {
                        File = new FileDescription(dosya.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")

                    };
                    yuklemeSonucu = cloudinary.Upload(yuklemePrametreleri);
                }
            }

            dto.Url = yuklemeSonucu.Uri.ToString();
            dto.PublicId = yuklemeSonucu.PublicId;

            var foto = mapper.Map<Foto>(dto);
            foto.Kullanici = kullanici;
            if (!kullanici.Fotograflari.Any(m => m.IlkTercihmi))
                foto.IlkTercihmi = true;

            kullanici.Fotograflari.Add(foto);
            if (await repo.Kaydet())
            {
                var donecekDto = mapper.Map<FotoOkuDto>(foto);
                return CreatedAtRoute("KullaniciFotografiniAl", new { id = foto.Id }, donecekDto);
            }
            return BadRequest("Fotoğraf eklenemedi!");
        }

        [HttpPost("{id}/asilYap")]
        public async Task<IActionResult> AsilFotoYap(int kullaniciNo, int id)
        {
            if (kullaniciNo != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                Unauthorized();
            var dbdekiKayit = await repo.FotografBulAsync(id);
            if (dbdekiKayit == null)
                return NotFound("Fotoğraf bulunamadı!");

            if (dbdekiKayit.IlkTercihmi)
                return BadRequest("Bu fotoğraf zaten asıl fotoğraf!");
            var suankiAsilFoto = await repo.KullanicininAsilFotosunuGetirAsync(kullaniciNo);
            if (suankiAsilFoto != null)
                suankiAsilFoto.IlkTercihmi = false;
            dbdekiKayit.IlkTercihmi = true;
            if (await repo.Kaydet())
                return NoContent();
            return BadRequest("Asıl foto yapılamadı!");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Sil(int kullaniciNo, int id)
        {

            if (kullaniciNo != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                Unauthorized();
            var dbdekiKayit = await repo.FotografBulAsync(id);
            if (dbdekiKayit == null)
                return NotFound("Fotoğraf bulunamadı!");

            if (dbdekiKayit.IlkTercihmi)
                return BadRequest("Asıl fotoğrafı silemezsiniz!");
            if (dbdekiKayit.PublicId != null)
            {

                var deleteParams = new DeletionParams(dbdekiKayit.PublicId);
                var result = cloudinary.Destroy(deleteParams);
                if (result.Result == "ok")
                    repo.Sil(dbdekiKayit);
            }
            if (dbdekiKayit.PublicId == null)
                repo.Sil(dbdekiKayit);

            if (await repo.Kaydet())
                return Ok();
            else
                return BadRequest("Fotoğraf silinemedi");

        }


    }
}