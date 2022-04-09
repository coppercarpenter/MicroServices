using AutoMapper;
using Grpc.Core;
using PlatformService.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepository _repo;
        private readonly IMapper _mapper;

        public GrpcPlatformService(IPlatformRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var res = new PlatformResponse();
            var platforms = _repo.GetPlatforms();
            foreach (var platform in platforms)
            {
                res.Platform.Add(_mapper.Map<GrpcPlatformModel>(platform));
            }
            return Task.FromResult(res);
        }
    }
}
