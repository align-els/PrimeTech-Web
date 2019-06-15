using System;
using System.Collections.Generic;
using RecipeJungle.Entities;
using RecipeJungle.Wrappers;

namespace RecipeJungle.Services
{
    public interface IUserService
    {
        User Authenticate(CreateUserRequest request);
        void CreateUser(CreateUserRequest request);
        void Delete(User user);
        User FindByUserName(string username);
        User UpdateUser(CreateUserRequest request,User user);
        List<Recipe> ListMyRecipes(User user);
    }
}
