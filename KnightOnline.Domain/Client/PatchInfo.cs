// using KnightOnline.Domain.SharedKernel; // For Entity or ValueObject if applicable

namespace KnightOnline.Domain.Client
{
    // Represents information about a specific patch or version file
    public class PatchInfo
    {
        public string Version { get; set; }         // e.g., "2100", "2100b" - could be version of the patch itself or target client version
        public string FileName { get; set; }        // e.g., "patch2100.zip", "update.exe"
        public string DownloadUrl { get; set; }     // URL to download the patch file
        public long FileSize { get; set; }          // Size of the patch file in bytes
        public string Hash { get; set; }            // Checksum (e.g., MD5, SHA256) to verify integrity
        public string? Description { get; set; }    // Optional description of the patch
        public System.DateTime ReleaseDate { get; set; } // When the patch was released

        // Constructor
        public PatchInfo(string version, string fileName, string downloadUrl, long fileSize, string hash, System.DateTime releaseDate, string? description = null)
        {
            Version = version;
            FileName = fileName;
            DownloadUrl = downloadUrl;
            FileSize = fileSize;
            Hash = hash;
            ReleaseDate = releaseDate;
            Description = description;
        }
    }
}
