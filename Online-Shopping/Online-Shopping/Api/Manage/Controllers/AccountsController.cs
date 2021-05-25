using JobbApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Shopping.Api.Manage.DTOs;
using Online_Shopping.Data;
using Online_Shopping.Data.Entities;
using Online_Shopping.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Manage.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public AccountsController(AppDbContext context, UserManager<AppUser> userManager, IJwtService jwtService,
            RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _jwtService = jwtService;
            _roleManager = roleManager;
            _env = env;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminLoginDto loginDto)
        {
            AppUser user = await _userManager.FindByNameAsync(loginDto.UserName);

            //404
            #region CheckUserNotFound
            if (user == null)
                return NotFound();
            #endregion

            //404
            #region CheckPasswordIncorrect
            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return NotFound();
            #endregion

            #region JWT Generate
            var roleNames = await _userManager.GetRolesAsync(user);
            string token = _jwtService.Generate(user, roleNames);
            #endregion

            return Ok(token);
        }

        #region EditProfile
        [Authorize(Roles = "Admin")]
        [HttpPut("edit")]

        public async Task<IActionResult> EditProfile([FromForm] AdminEditDto editDto)
        {
            AppUser existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            #region CheckUser
            if (existUser == null)
                return NotFound();
            #endregion

            #region CheckEmail
            if (_context.Users.Any(x => x.UserName == editDto.UserName && x.Id != existUser.Id))
                return StatusCode(409, $"User already exist by email {editDto.UserName}");
            #endregion
            #region CheckFile
            if (editDto.File != null)
            {
                #region CheckPhotoLength
                if (editDto.File.Length > 4 * (1024 * 1024))
                {
                    return StatusCode(409, "File cannot be more than 4MB");
                }
                #endregion
                #region CheckPhotoContentType
                if (editDto.File.ContentType != "image/png" && editDto.File.ContentType != "image/jpeg")
                {
                    return StatusCode(409, "File only jpeg and png files accepted");
                }
                #endregion

                string filename = FileManagerHelper.Save(_env.WebRootPath, "uploads/admins", editDto.File);
                if (!string.IsNullOrWhiteSpace(existUser.Photo))
                {
                    FileManagerHelper.Delete(_env.WebRootPath, "uploads/admins", existUser.Photo);
                }
                existUser.Photo = filename;
            }
            #endregion


            existUser.UserName = editDto.UserName;
            existUser.PhoneNumber = editDto.PhoneNumber;
            existUser.ModifiedAt = DateTime.UtcNow.AddHours(4);

            #region CheckPassword
            if (!string.IsNullOrWhiteSpace(editDto.CurrentPassword) && !string.IsNullOrWhiteSpace(editDto.Password) && !string.IsNullOrWhiteSpace(editDto.ConfirmPassword))
            {
                var resultPass = await _userManager.ChangePasswordAsync(existUser, editDto.CurrentPassword, editDto.Password);
                if (!resultPass.Succeeded)
                    return StatusCode(402, resultPass.Errors.First().Description);
            }
            #endregion

            await _context.SaveChangesAsync();
            return StatusCode(201, new { existUser.UserName, existUser.Id });
        }
        #endregion

        #region Test
        //[HttpGet("test")]
        //public async Task<IActionResult> Test()
        //{
        //    //await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
        //    //await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });

        //    //AppUser user = new AppUser
        //    //{
        //    //    UserName = "SuperAdmin",
        //    //};

        //    AppUser user = await _userManager.FindByNameAsync("SuperAdmin");
        //    await _userManager.AddToRoleAsync(user, "Admin");

        //    //await _userManager.CreateAsync(user, "Admin123");
        //    return Ok();
        //}
        #endregion



    }
}
