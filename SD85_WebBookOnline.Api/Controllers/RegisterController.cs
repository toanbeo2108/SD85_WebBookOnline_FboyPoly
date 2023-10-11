using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.IServices;
using SD85_WebBookOnline.Share.ViewModels;

namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterServices _registerService;
        public RegisterController(IRegisterServices registerService)
        {
            _registerService = registerService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser registerUser, string role)
        {
            var result = await _registerService.RegisterAsync(registerUser, role);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
