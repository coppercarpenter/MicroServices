using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/c/platforms")]
    public class PlatformsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICommandRepository _repo;
        private readonly IMapper _mapper;

        public PlatformsController(ILoggerFactory logger, ICommandRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger.CreateLogger("PlatformsController");
        }


        [HttpGet("")]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            _logger.LogInformation("--> Getting Platform from  Command Service");
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(_repo.GetPlatforms()));
        }

        [HttpPost("")]
        public ActionResult TestInboundConnection()
        {
            _logger.LogInformation("Inbound POST @ command service");
            return Ok("Inbound test Platforms Controller");
        }
    }
}
