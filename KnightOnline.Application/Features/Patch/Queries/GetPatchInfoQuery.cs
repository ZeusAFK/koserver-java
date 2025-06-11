using MediatR;
using KnightOnline.Application.DTOs.Client; // Assuming a DTO for PatchInfo will be created
using System.Collections.Generic;

namespace KnightOnline.Application.Features.Patch.Queries
{
    // Query to get patch information
    public class GetPatchInfoQuery : IRequest<GetPatchInfoQuery.Response>
    {
        // Add any parameters if needed, e.g., specific version or client type
        // public string ClientVersion { get; set; }

        public class Response
        {
            public PatchInfoDto LatestPatch { get; }
            // public IEnumerable<PatchInfoDto> AllPatches { get; } // If returning all patches

            public Response(PatchInfoDto latestPatch)
            {
                LatestPatch = latestPatch;
            }
        }
    }
}
