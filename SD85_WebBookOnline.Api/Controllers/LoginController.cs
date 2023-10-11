using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Api.IServices;
using SD85_WebBookOnline.Share.ViewModels;

namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginServices _loginServices;
        public LoginController(ILoginServices loginServices)
        {
            _loginServices = loginServices;
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {
            var result = await _loginServices.LoginAsync(loginUser);
            if (result.IsSuccess)
            {
                return Ok(result.Token);
            }
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
