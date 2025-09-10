using AZ_AI_Language_CustomNER.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AZ_AI_Language_CustomNER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController(ICustomNerAnalysis customNerAnalysis) : ControllerBase
    {
        [HttpPost("Recognize")]
        public async Task<IActionResult> CustomNerAnalysis(List<string> documents)
        {
            var result = await customNerAnalysis.CustomNerAsync(documents);
            return Ok(result);
        }
    }
}
