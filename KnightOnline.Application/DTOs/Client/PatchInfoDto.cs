namespace KnightOnline.Application.DTOs.Client
{
    public class PatchInfoDto
    {
        public string Version { get; set; }
        public string DownloadUrl { get; set; }
        public string? Description { get; set; }

        public PatchInfoDto()
        {
            Version = string.Empty;
            DownloadUrl = string.Empty;
        }
    }
}
