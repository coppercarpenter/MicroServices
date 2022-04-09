using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace CommandsService.EventProcessing
{
    internal class EventProcessor : IEventProcessor
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper, ILoggerFactory logger)
        {
            _mapper = mapper;
            _logger = logger.CreateLogger("Event Processor");
            _scopeFactory = scopeFactory;
        }

        public void ProecssEvent(string message)
        {

            var eventType = GetEventType(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                default:
                    break;
            }

        }

        private void AddPlatform(string platformRaw)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepository>();
                var platformPublished = JsonSerializer.Deserialize<PlatformPublishedDto>(platformRaw);
                try
                {
                    if (!repo.ExternalPlatformExists(platformPublished.Id))
                    {
                        var platform = _mapper.Map<Platform>(platformPublished);
                        repo.CreatePlatform(platform);
                        repo.SaveChanges();

                        _logger.LogInformation("Platform created");
                    }
                    else
                    {
                        _logger.LogInformation("Platform already exists");
                    }
                }
                catch (Exception ex)
                {

                    _logger.LogError($"Could not add platform: {ex.Message}");
                }
            }
        }
        public EventType GetEventType(string notificationMessage)
        {
            _logger.LogInformation("Getting Event Type");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            return eventType.Event switch
            {
                "Platform_Published" => EventType.PlatformPublished,
                _ => EventType.Undetermined,
            };
        }
    }


}
