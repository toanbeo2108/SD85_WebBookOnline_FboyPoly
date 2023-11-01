﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly UserManager<User> _userManager;
        public UserController(UserManager<User> userManager)
        {   
            _allResponsitories = new AllResponsitories<User>(_context, _context.Users);
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetAllUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<User>> getUser()
        {
            //return await _allResponsitories.GetAll();
            return await _userManager.Users.ToListAsync();
        }
        [HttpGet]
        [Route("GetUsersByRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<User>> getUserByRole(string roleName)
        {
            return await _userManager.GetUsersInRoleAsync(roleName);    
        }

        [HttpGet]
        [Route("GetUsersById")]
        [Authorize(Roles = "Admin")]
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


    }
}
