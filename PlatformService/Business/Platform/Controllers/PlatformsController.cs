using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Business.Platform.Services.PlatformService;
using PlatformService.Business.Platform.ViewModels;
using PlatformService.Core;

namespace PlatformService.Business.Platform.Controllers
{
    [Route("api/platforms")]
    public class PlatformsController : BaseController
    {
        private readonly IPlatformService _platformService;

        public PlatformsController(IPlatformService platformService)
        {
            _platformService = platformService;
        }

        [HttpGet(Name = "GetPlatforms")]
        public ActionResult<IEnumerable<PlatformDetails>> GetPlatforms()
        {
            var result = _platformService.GetAllPlatformsAsync();
            return Ok(result);
        }
        
        [HttpGet(template: "{id:guid}", Name = "GetPlatform")]
        public ActionResult<PlatformDetails> GetPlatform([FromRoute] Guid id)
        {
            var result = _platformService.GetPlatformByIdAsync(id);

            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost(Name = "CreatePlatform")]
        public ActionResult<PlatformDetails> CreatePlatform([FromBody] PlatformCreate createModel)
        {
            var result = _platformService.CreatePlatformAsync(createModel);

            return result is null ? NotFound() : Ok(result);
        }

        [HttpPut(template: "{id:guid}", Name = "UpdatePlatform")]
        public ActionResult<PlatformDetails> UpdatePlatform( [FromRoute] Guid id, [FromBody] PlatformUpdate platformUpdate)
        {
            var result = _platformService.UpdatePlatformAsync(id, platformUpdate);

            return result is null ? NotFound() : Ok(result);
        }
    }
}
