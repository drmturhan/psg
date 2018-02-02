using Core.EntityFramework;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Psg.Api.Extensions;
using Psg.Api.Properties;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;

namespace Psg.Api.Base
{
    public class MTController : Controller
    {
        protected readonly string TEMEL_ENTITY_ADI = "";
        public MTController(string entityAdi)
        {
            TEMEL_ENTITY_ADI = entityAdi;
        }
        protected async Task<IActionResult> HataKontrolluCalistir<R>(Func<Task<IActionResult>> codetoExecute) where R : class
        {
            try
            {
                return await codetoExecute.Invoke();
            }
            catch (ModelStateError)
            {
                return new DortYuzYirmiIki(ModelState);
            }
            catch (Exception hata)
            {
                Sonuc<R> sonuc = Sonuc<R>.Basarisiz(new Exception("Beklenmedik bir hata oluştu", hata));
                return StatusCode(500, sonuc);
            }
        }
        protected async Task<IActionResult> GecerlilikKontrolluCalistir<R>(Func<IActionResult> codetoExecute) where R : class
        {
            if (!ModelState.IsValid)
                return new DortYuzYirmiIki(ModelState);
            try
            {
                return codetoExecute.Invoke();
            }

            catch (BadRequestError hata)
            {
                return BadRequest(new BadRequestError(hata.Message));
            }
            catch (ModelStateError)
            {
                return new DortYuzYirmiIki(ModelState);
            }
            catch (NotFoundError hata)
            {
                return NotFound(Sonuc.Basarisiz(new Exception("Kayıt bulunamadı!", hata)));
            }
            catch (InternalServerError hata)
            {
                return NotFound(Sonuc.Basarisiz(new Exception("İşlem başarısız. Lütfen daha sonra tekrar deneyiniz!", hata)));
            }
            catch (Exception hata)
            {
                return StatusCode(500, Sonuc.Basarisiz(new Exception(Properties.Resources.IslemGerceklesmedi, hata)));
            }
        }

        #region Kapat
        //protected void Calistir(Action codetoExecute)
        //{
        //    try
        //    {
        //        codetoExecute.Invoke();
        //    }
        //    catch (AuthorizationValidationException ex)
        //    {
        //        throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
        //    }
        //    catch (FaultException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new FaultException(ex.Message);
        //    }
        //} 
        #endregion

        #region Yardimcilar
        protected string AnBilgisiAl()
        {

            return $"Kullanıcı:{User.Identity.Name}, Tarih:{DateTime.UtcNow}, IpNo:{GetClientIp()}, MACAddress:{GetClientMAC()}";
        }
        private string GetClientIp()
        {
            return HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();//{Request.HttpContext.Connection.RemoteIpAddress} de kullanilmis orneklerde
        }

        private string GetClientMAC()
        {
            string macAddresses = "";

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }
        #endregion

    }
    public class ModelStateError : ArgumentException
    {

    }
    public class BadRequestError : ArgumentException
    {
        public BadRequestError()
        {
        }

        public BadRequestError(string message) : base(message)
        {
        }
    }
    public class NotFoundError : ArgumentException
    {
        public NotFoundError()
        {
        }

        public NotFoundError(string message) : base(message)
        {
        }
    }
    public class DortYuzYirmiIki : ObjectResult
    {
        public DortYuzYirmiIki(ModelStateDictionary modelState) : base(new SerializableError(modelState))
        {
            if (modelState == null)
                throw new ArgumentNullException(nameof(modelState));
            StatusCode = 422;
        }
    }

    public class InternalServerError : InvalidOperationException
    {
        public InternalServerError()
        {
        }

        public InternalServerError(string message) : base(message)
        {
        }
    }
}

