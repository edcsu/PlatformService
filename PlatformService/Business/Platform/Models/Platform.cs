namespace PlatformService.Business.Platform.Models
{
    public class Platform : BaseModel
    {
#nullable disable
        public string Name { get; set; }

        public string Publisher { get; set; }
        
        public string Cost { get; set; }
    }
}
