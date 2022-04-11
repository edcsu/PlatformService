namespace PlatformService.Business.Platform.Models
{
    public class BaseModel
    {
        public Guid Id { get; set; }

        public DateTime Created { get; set; }
        
        public DateTime? Updated { get; set; }
    }
}
