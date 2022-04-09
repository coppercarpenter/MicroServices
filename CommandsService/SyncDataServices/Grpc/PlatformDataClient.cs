using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlatformService;
using System;
using System.Collections.Generic;

namespace CommandsService.SyncDataServices.Grpc
{
    internal class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PlatformDataClient(IConfiguration config, IMapper mapper, ILoggerFactory logger)
        {
            _config = config;
            _mapper = mapper;
            _logger = logger.CreateLogger("PlatformDataClient");
        }

        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            _logger.LogInformation($"--> Calling gRPC Service {_config["GrpcPlatform"]}");

            var channel = GrpcChannel.ForAddress(_config["GrpcPlatform"]);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);

            var request = new GetAllRequest();

            try
            {
                var response = client.GetAllPlatforms(request);
                return _mapper.Map<IEnumerable<Platform>>(response.Platform);

            }
            catch (Exception ex)
            {
                _logger.LogError($"--> could not call gRPC server {ex.Message}");
                return null;
            }
        }
    }
}
