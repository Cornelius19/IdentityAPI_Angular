using API.DTOs.Account;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTService _jWTService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(JWTService jwtService,
            SignInManager<User> signInManager,//responsible to sign the user in
            UserManager<User> userManager)//responsible to creating the user their both are taking the type in our case User that is derived from IdentityUser 
        {
            _jWTService = jwtService;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        [Authorize]//methods or endpoint that are accesible only for authorized users //if is not authorized we get messahe 401 unauthtorized
        [HttpGet("refresh-user-token")]
        public async Task<ActionResult<UserDto>> RefreshUserToken() //method for refreshing token
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            return CreateApplicationUserDto(user);
        }




        [HttpPost("login")] //wherever the client access this endPoint they are passing a model and in our case our model is of type LoginDto that contains a username and a password
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {   
            var user = await _userManager.FindByNameAsync(model.UserName);//base of the user is providing we are trying to receive our user from database 
            if (user == null)
            {
                return Unauthorized("Invalid username or password!");
            }
            if (user.EmailConfirmed == false)
            {
                return Unauthorized("Please confirm your email!");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);//false is for not locking the user even if they put the wrong password
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid username or password");
            }
            return CreateApplicationUserDto(user); //if everything in true then we return our UserDto in the method we are calling
        }

        //now for the register method
        [HttpPost("register")]
        public async Task<IActionResult>Register(RegisterDto model)
        {
            if(await CheckEmailExistAsync(model.Email))
            {
                return BadRequest($"An existing account is using {model.Email}, email address. Please try with another email address!");
            }
            var userToAdd = new User
            {
                FirstName = model.FirstName.ToLower(),
                LastName = model.LastName.ToLower(),
                UserName = model.Email.ToLower(),
                Email = model.Email.ToLower(),
                EmailConfirmed = true

            };

            var result = await _userManager.CreateAsync(userToAdd,model.Password); //this method is going to add our user to Database if everything is ok
            if(!result.Succeeded) 
            {
                return BadRequest(result.Errors);
            }
            return Ok("Your account has been created, you can login!");
        }



        //this is basicaly like a comment doesn't do anything in coding is just for readability for developers
        #region Private Helper Methods 

        private UserDto CreateApplicationUserDto(User user)
        {
            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                JWT=_jWTService.CreateJWT(user),
            };
        }

        //for checking if the user is trying to register a email address that is already register
        private async Task<bool> CheckEmailExistAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());//return a boolean true or false if this expression is true x => x.Email == email.ToLower()
        }

        #endregion
    }
}
