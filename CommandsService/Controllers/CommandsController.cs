using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/c/platforms/{platform_id}/command")]
    public class CommandsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICommandRepository _repo;
        private readonly IMapper _mapper;

        public CommandsController(ILoggerFactory logger, ICommandRepository repo, IMapper mapper)
        {
            _logger = logger.CreateLogger("PlatformsController");
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("")]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommands(long platform_id)
        {
            _logger.LogInformation($"Hit GetCommands with Platform Id {platform_id}");

            if (!_repo.PlatformExists(platform_id))
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(_repo.GetCommands(platform_id)));
        }
        [HttpGet("{command_id}", Name = "GetCommand")]
        public ActionResult<CommandReadDto> GetCommand(long platform_id, long command_id)
        {
            _logger.LogInformation($"Hit GetCommand with Platform_Id {platform_id} and Command_Id {command_id}");

            if (!_repo.PlatformExists(platform_id))
                return NotFound();

            var command = _repo.GetCommand(platform_id, command_id);
            if (command == null)
                return NotFound();

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost("")]
        public ActionResult<CommandReadDto> AddCommands(long platform_id, CommandCreateDto model)
        {
            _logger.LogInformation($"Hit AddCommand with Platform Id {platform_id}");

            if (!_repo.PlatformExists(platform_id))
                return NotFound();

            var command = _mapper.Map<Command>(model);

            _repo.CreateCommand(platform_id, command);
            _repo.SaveChanges();

            var res = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommand), new { platform_id = platform_id, command_id = res.Id }, res);
        }
    }
}
