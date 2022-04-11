using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace PlatformService.Core
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class BaseController : ControllerBase
    {
    }
}
