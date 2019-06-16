using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RecipeJungle.Entities;
using RecipeJungle.Helpers;
using RecipeJungle.Contexts;
using RecipeJungle.Wrappers;
using RecipeJungle.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace RecipeJungle.Services
{

    public class UserService : IUserService
    {
        private RecipeContext userContext;
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, RecipeContext userContext)
        {
            _appSettings = appSettings.Value;
            this.userContext = userContext;
        }

        public User Authenticate(CreateUserRequest request)
        {
            var user = userContext.Users.SingleOrDefault(x => x.Username == request.Username);

            if (user == null)
                throw new ActionFailedException("User is not found!");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.ToBeOrNotToBe);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)

                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);


            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new ActionFailedException("Wrong Password!");

            return user;
        }

        public void CreateUser(CreateUserRequest request)
        {
            if (request == null)
                throw new ActionFailedException("invalid request body");
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ActionFailedException("E-mail cannot be empty");
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new ActionFailedException("Username cannot be empty");
            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ActionFailedException("Password cannot be empty");

            if (request.Password.Length < 8)
                throw new ActionFailedException("Password is too short");
            if (request.Password.Length > 20)
                throw new ActionFailedException("Password is too long");

            if (userContext.Users.Any(x => x.Username == request.Username))
                throw new ActionFailedException("Username \"" + request.Username + "\" is already taken");
            if (userContext.Users.Any(x => x.Email == request.Email))
                throw new ActionFailedException("Email \"" + request.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);


            User user = new User();
            user.Username = request.Username;
            user.Email = request.Email;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Id = 0;
            user.RecipesOfUser = new List<Recipe>();
            user.LikedRecipesOfUser = new List<Recipe>();

            userContext.Users.Add(user);
            userContext.SaveChanges();
        }

        public User UpdateUser(CreateUserRequest request,User user)
        {
            if (request == null)
                throw new ActionFailedException("invalid request body");
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ActionFailedException("E-mail cannot be empty");
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new ActionFailedException("Username cannot be empty");
            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ActionFailedException("Password cannot be empty");

            if (request.Password.Length < 8)
                throw new ActionFailedException("Password is too short");
            if (request.Password.Length > 20)
                throw new ActionFailedException("Password is too long");

            if (userContext.Users.Any(x => x.Username == request.Username))
                throw new ActionFailedException("Username \"" + request.Username + "\" is already taken");
            if (userContext.Users.Any(x => x.Email == request.Email))
                throw new ActionFailedException("Email \"" + request.Username + "\" is already taken");


            user.Username = request.Username;
            user.Email = request.Email; 
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            userContext.Users.Update(user);
            userContext.SaveChanges();

            return user;
        }

        public void Delete(User user)
        {
            if (user == null)
                throw new ActionFailedException("User is not found!");

            userContext.Remove(user);
            //user ıcındekı recipeler taglar sılınecek
            userContext.SaveChanges();

        }

        public User FindByUserName(string username)
        {
            User user = userContext.Users.SingleOrDefault(x => x.Username == username);
            return user;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public List<Recipe> ListMyRecipes(User user)
        {
            return userContext.Recipes
                 .Include(x => x.Photos)
                 .Include(x => x.RecipeTags)
                     .ThenInclude(x => x.Tag)
                 .Where(x=>x.User.Id==user.Id).ToList();
        }
    }
}
