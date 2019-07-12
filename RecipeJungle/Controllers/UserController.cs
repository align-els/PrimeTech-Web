using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using RecipeJungle.Services;
using RecipeJungle.Helpers;
using RecipeJungle.Entities;
using RecipeJungle.Wrappers;
using System.Security.Principal;
using RecipeJungle.Exceptions;
using RecipeJungle.Filters;
using System.Net.Http.Headers;
using System.Net.Http;

namespace RecipeJungle.Controllers
{
    [Authorize]
    [ApiController]
    [UserFilter]
    [Route("/api/account")]
    public class UsersController : ControllerBase
    {
        private IUserService userService;
        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]CreateUserRequest request)
        {
            var user = userService.Authenticate(request);
            if (user == null) 
                throw new ActionFailedException("Username or password is incorrect!");

            return ActionUtils.Success(user.Token);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] CreateUserRequest request)
        {
            userService.CreateUser(request);
            return ActionUtils.Success("Register success");
        }
       
        [HttpPost("update")]
        public IActionResult Update([FromBody] CreateUserRequest request, [FromHeader] User user)
        {
            return ActionUtils.Success(userService.UpdateUser(request, user));
        }

        [HttpDelete("delete")]
        public IActionResult Delete(User user)
        {
            userService.Delete(user);
            return ActionUtils.Success();
        }

        [HttpGet("list")]
        public IActionResult ListMyRecipes([FromHeader] User user)
        {
            return ActionUtils.Success(userService.ListMyRecipes(user));
        }
    }
}