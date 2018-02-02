﻿using System.Threading.Tasks;
using AutoMapper;
using Identity.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Psg.Api.Base;
using Psg.Api.Dtos;
using Psg.Api.Repos;

namespace Psg.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/cinsiyetler")]
    public class CinsiyetlerController : MTController
    {
        private readonly ICinsiyetRepository repo;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IUrlHelper urlHelper;

        public CinsiyetlerController(ICinsiyetRepository repo, IMapper mapper, IConfiguration configuration, IUrlHelper urlHelper) : base("Cinsiyetler")
        {
            this.repo = repo;
            this.mapper = mapper;
            this.configuration = configuration;
            this.urlHelper = urlHelper;
        }
        [HttpGet()]
        public async Task<IActionResult> Get()
        {

            return await HataKontrolluCalistir<Task<IActionResult>>(async () =>
            {


                var kayitlar = await repo.ListeGetirCinsiyetAsync();
                return Ok(kayitlar);
            });
        }

    }
}