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
using RecipeJungle.Contexts;
using System.Linq;

namespace RecipeJungle.Controllers
{
    [ApiController]
    [Route("/api/account")]
    public class UsersController : ControllerBase
    {
        private IUserService userService;
        private RecipeContext recipeContext;

        public UsersController(IUserService userService, RecipeContext recipeContext)
        {
            this.userService = userService;
            this.recipeContext = recipeContext;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]CreateUserRequest request)
        {
            var user = userService.Authenticate(request);
            if (user == null) 
                throw new ActionFailedException("Username or password is incorrect!");

            return ActionUtils.Success(user.Username);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] CreateUserRequest request)
        {
            userService.CreateUser(request);
            return ActionUtils.Success("Register success");
        }
       
        [HttpPost("update")]
        [UserFilter]
        public IActionResult Update([FromBody] CreateUserRequest request, [FromHeader] User user)
        {
            return ActionUtils.Success(userService.UpdateUser(request, user));
        }

        [HttpDelete("delete")]
        [UserFilter]
        public IActionResult Delete(User user)
        {
            userService.Delete(user);
            return ActionUtils.Success();
        }

        [HttpGet("changePref")]
        [UserFilter]
        public IActionResult ChangePref(int value, [FromHeader] User user)
        {
            var name = user.Username;
            var v = recipeContext.Preferences.FirstOrDefault(x => x.Username == name);
            if (v == null) {
                recipeContext.Preferences.Add(new Preference {
                    Username = name,
                    Value = value
                });
                recipeContext.SaveChanges();
                v = recipeContext.Preferences.FirstOrDefault(x => x.Username == name);
            }
            v.Value = value;
            recipeContext.SaveChanges();

            return ActionUtils.Success();
        }
    }
}