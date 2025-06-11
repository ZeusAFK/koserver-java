using KnightOnline.Domain.SharedKernel; // For Entity or ValueObject if applicable

namespace KnightOnline.Domain.Client
{
    // Represents information about a specific patch or version
    public class PatchInfo // Could be an Entity if it has a lifecycle, or ValueObject if it's just data
    {
        public string Version { get; set; } // e.g., "2100"
        public string DownloadUrl { get; set; } // URL to download the patch files
        public string? Description { get; set; } // Optional description

        public PatchInfo(string version, string downloadUrl, string? description = null)
        {
            Version = version;
            DownloadUrl = downloadUrl;
            Description = description;
        }
    }
}
