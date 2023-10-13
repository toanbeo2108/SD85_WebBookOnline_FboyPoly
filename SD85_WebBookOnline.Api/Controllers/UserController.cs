using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAllResponsitories<User> _allResponsitories;
        public AppDbContext _context = new AppDbContext();
        public UserController()
        {
            _allResponsitories = new AllResponsitories<User>(_context,_context.Users);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<User>> getUser()
        {
            return await _allResponsitories.GetAll();
        }


    }
}
