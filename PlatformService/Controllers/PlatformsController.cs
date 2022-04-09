using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlatformService.AsyncDataServices;
using PlatformService.Database;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/platforms")]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandDataClient _commandData;
        private readonly IMessageBusClient _client;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IPlatformRepository _repo;

        public PlatformsController(IPlatformRepository repo, IMapper mapper, ICommandDataClient commandData,
                                   ILoggerFactory logger, IMessageBusClient client)
        {
            _repo = repo;
            _mapper = mapper;
            _commandData = commandData;
            _client = client;
            _logger = logger.CreateLogger("PlatformsController");
        }


        [HttpPost("")]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto model)
        {

            var platform = _mapper.Map<Platform>(model);

            _repo.CreatePlatform(platform);
            _repo.SaveChanges();

            var res = _mapper.Map<PlatformReadDto>(platform);


            try
            {
                await _commandData.SendPlatformToCommandAsync(res, _logger);
            }
            catch (Exception ex)
            {
                _logger.LogError($"could not send synchronusly: {ex.Message}");
            }


            try
            {
                var publishedPlatform = _mapper.Map<PlatformPublishedDto>(res);
                publishedPlatform.Event = "Platform_Published";
                _client.PublishNewPlatform(publishedPlatform);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Could not send async message: {ex.Message}");
            }
            return CreatedAtRoute(nameof(GetPlatform), new { res.Id }, res);
        }

        [HttpGet("")]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            _logger.LogInformation(" --> getting platforms");
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(_repo.GetPlatforms()));
        }

        [HttpGet("{id}", Name = "GetPlatform")]
        public ActionResult<PlatformReadDto> GetPlatform(long id)
        {
            var platform = _repo.GetPlatform(id);
            if (platform == null)
                return NotFound("Platform");
            return Ok(_mapper.Map<PlatformReadDto>(platform));
        }
    }
}
