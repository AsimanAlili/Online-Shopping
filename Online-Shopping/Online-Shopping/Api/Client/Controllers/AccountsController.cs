using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Shopping.Api.Client.DTOs;
using Online_Shopping.Data;
using Online_Shopping.Data.Entities;
using Online_Shopping.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Api.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtService _jwtService;

        public AccountsController(AppDbContext context,UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            IJwtService jwtService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }

        #region Register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            AppUser user = await _userManager.FindByEmailAsync(registerDto.Email);

            //409
            #region CheckuserAlreadyExist
            if (user != null) StatusCode(409, $"User already exist by email {registerDto.Email}");
            #endregion

            user = new AppUser
            {
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            #region CheckResultFailed
            if (!result.Succeeded)
            {
                return StatusCode(402, result.Errors.First().Description);
            }
            #endregion
            await _userManager.AddToRoleAsync(user, "Member");

            return StatusCode(201, user.Id);
        }
        #endregion

        #region Login
        [HttpPost("login")]
        public async Task<IActionResult> Login(MemberLoginDto loginDto)
        {
            AppUser user = await _userManager.FindByEmailAsync(loginDto.Email);

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

            return Ok(new { user.FullName, Token = token });
        }
        #endregion

        #region EditProfile
        [Authorize(Roles = "Member")]
        [HttpPut("edit")]

        public async Task<IActionResult> EditProfile( MemberEditDto editDto)
        {
            AppUser existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            #region CheckUser
            if (existUser == null)
                return NotFound();
            #endregion

            #region CheckEmail
            if (_context.Users.Any(x => x.Email == editDto.Email && x.Id != existUser.Id))
                return StatusCode(409, $"User already exist by email {editDto.Email}");
            #endregion

            existUser.UserName = editDto.UserName;
            existUser.Email = editDto.Email;
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

        //[HttpPost("logout")]
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync("Member");

        //    return Ok();
        //}
    }
}
