using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;
using SD85_WebBookOnline.Share.ViewModels;

namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public AppDbContext _context = new AppDbContext();
        private readonly UserManager<User> _userManager;
        public UserController(UserManager<User> userManager)
        {   
            _userManager = userManager;
        }
   
        [HttpGet]
        [Route("GetUserId/{username}")]
        public async Task<string> GetUserId(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return null;
            }
            return user.Id;
        }

        [HttpGet]
        [Route("GetAllUser")]
        //[Authorize]
        public async Task<IEnumerable<User>> getUser()
        {
            return await _userManager.Users.ToListAsync();
        }
        [HttpGet]
        [Route("GetUsersByRole")]
        //[Authorize(Roles = "Admin")]
        public async Task<IEnumerable<User>> getUserByRole(string roleName)
        {
            return await _userManager.GetUsersInRoleAsync(roleName);    
        }

        [HttpGet]
        [Route("GetUsersById")]
        [Authorize]
        public async Task<User> getUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        [HttpPut]
        [Route("UpdateUser")]
        [Authorize]
        public async Task<User> UpdateUser(User model)
        {
            // Tìm người dùng theo ID
            var user = await _userManager.FindByIdAsync(model.Id);

            // Cập nhật thông tin người dùng
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Avatar = model.Avatar;
            user.Status = model.Status;
            user.Country = model.Country;
            user.UpdateDate = model.UpdateDate;
            user.DeliveryAddressID = model.DeliveryAddressID;
            user.CardNumber = model.CardNumber; 
            // Cập nhật các thuộc tính khác của User tại đây

            // Lưu thay đổi
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return user;
            }
            else 
            {
                return null;
            }

        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [Route("ChangePassword")]
        public async Task<bool> ChangePassword(ChangePasswordModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [HttpPost]
        public async Task<bool> plusPoint(string UserId,int point)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return false;
            }
            user.Point += point;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
