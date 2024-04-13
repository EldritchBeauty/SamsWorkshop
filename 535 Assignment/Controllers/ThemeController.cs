using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _535_Assignment.Controllers
{
    /// <summary>
    /// Changes the page theme. Persists throughout the session.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ThemeController : ControllerBase
    {
        [HttpPost("ChangeTheme")]
        public async Task<IActionResult> ChangeTheme([FromBody] ThemeSetting theme)
        {
            HttpContext.Session.SetString("theme", theme.Theme);
            return Ok();
        }
    }

    public class ThemeSetting
    {
        public string Theme { get; set; }
    }
}