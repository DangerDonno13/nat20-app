#region Usings
using Microsoft.AspNetCore.Mvc;

using Nat20Server.Services;
#endregion

namespace Nat20Server.Controllers
{
    /// <summary>
    /// REST Controller for RuleChapter management.
    /// Handles retrieval of rule chapters.
    /// </summary>
    /// <param name="ruleService"></param>
    [ApiController]
    [Route("api/rule")]
    public sealed class RuleController(RuleService ruleService) : ControllerBase
    {
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetRuleChapterAsync(string slug)
        {
            var ruleChapter = await ruleService.GetRuleChapterAsync(slug);
            return ruleChapter is null ? NotFound() : Ok(ruleChapter);
        }
    }
}
