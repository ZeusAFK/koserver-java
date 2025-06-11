using MediatR;
using KnightOnline.Application.Contracts.Infrastructure; // For IPatchInfoRepository
using KnightOnline.Application.DTOs.Client; // For PatchInfoDto
using System.Threading;
using System.Threading.Tasks;
using System;

namespace KnightOnline.Application.Features.Patch.Queries
{
    public class GetPatchInfoQueryHandler : IRequestHandler<GetPatchInfoQuery, GetPatchInfoQuery.Response>
    {
        private readonly IPatchInfoRepository _patchInfoRepository;

        public GetPatchInfoQueryHandler(IPatchInfoRepository patchInfoRepository)
        {
            _patchInfoRepository = patchInfoRepository ?? throw new ArgumentNullException(nameof(patchInfoRepository));
        }

        public async Task<GetPatchInfoQuery.Response> Handle(GetPatchInfoQuery request, CancellationToken cancellationToken)
        {
            var latestPatchDomain = await _patchInfoRepository.GetLatestPatchAsync();

            if (latestPatchDomain == null)
            {
                // Handle case where no patch info is found, perhaps return a default or throw an exception
                // For now, returning a default or empty DTO.
                return new GetPatchInfoQuery.Response(new PatchInfoDto { Version = "N/A", DownloadUrl = string.Empty });
            }

            // TODO: Create PatchInfoDto and map from domain PatchInfo
            // For now, direct mapping for simplicity if structure is identical
            var patchDto = new PatchInfoDto
            {
                Version = latestPatchDomain.Version,
                DownloadUrl = latestPatchDomain.DownloadUrl,
                Description = latestPatchDomain.Description
            };

            return new GetPatchInfoQuery.Response(patchDto);
        }
    }
}
