namespace PlatformService.Business.Platform.ViewModels
{
    public class PlatformDetails
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Publisher { get; set; }

        public string? Cost { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }
    }
}
