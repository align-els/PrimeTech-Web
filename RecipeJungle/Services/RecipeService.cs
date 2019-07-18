using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RecipeJungle.Contexts;
using RecipeJungle.Entities;
using RecipeJungle.Exceptions;
using RecipeJungle.Wrappers;

namespace RecipeJungle.Services
{
    public class RecipeService : IRecipeService {
        private RecipeContext recipeContext;

        public RecipeService(RecipeContext recipeContext) {
            this.recipeContext = recipeContext;
        }

        public void CreateRecipe(CreateRecipeRequest request, User user) {
            if (request == null)
                throw new ActionFailedException("invalid request body");
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ActionFailedException("title cannot be empty");
            if (string.IsNullOrWhiteSpace(request.Text))
                throw new ActionFailedException("title cannot be empty");

            request.Title = request.Title.Trim();
            request.Text = request.Text.Trim();

            if (request.Title.Length < 8)
                throw new ActionFailedException("title is too short");
            if (request.Text.Length < 20)
                throw new ActionFailedException("recipe details is too short");

            if (request.Portion <= 0)
                throw new ActionFailedException("portion cannot be smaller than 1");
            if (request.Portion > 20)
                throw new ActionFailedException("portion is too large");

            if (request.PrepareTime <= 0)
                throw new ActionFailedException("prepare time cannot be smaller than 1");
            if (request.PrepareTime > 600)
                throw new ActionFailedException("prepare time is too large");

            if (request.Ingredients == null)
                request.Ingredients = new string[0];
            if (request.Steps == null)
                request.Steps = new string[0];

            foreach (var item in request.Steps) {
                if (string.IsNullOrWhiteSpace(item))
                    throw new ActionFailedException("step items cannot be empty");
            }

            if (request.Photos == null)
                request.Photos = new List<string>();
            if (request.Tags == null)
                request.Tags = new string[0];

            if (request.Photos.Count == 0)
                throw new ActionFailedException("Select at least one image");

            Recipe recipe = new Recipe();
            recipe.Id = 0;
            recipe.Title = request.Title;
            recipe.Text = request.Text;
            recipe.Ingredients = JsonConvert.SerializeObject(request.Ingredients);
            recipe.Steps = JsonConvert.SerializeObject(request.Steps);
            recipe.CreatedTime = DateTime.Now;
            recipe.ModifiedTime = DateTime.Now;
            recipe.Portion = request.Portion;
            recipe.PrepareTime = request.PrepareTime;
            recipe.User = user;
            recipe.Photos = JsonConvert.SerializeObject(request.Photos);
            recipe.RecipeTags = new List<RecipeTag>();
            recipeContext.Recipes.Add(recipe);
            recipeContext.SaveChanges();

            foreach (var item in request.Tags) {
                if (string.IsNullOrWhiteSpace(item))
                    throw new ActionFailedException("tags cannot be empty");

                var tagText = item.ToLower();
                Tag tag = recipeContext.Tags.FirstOrDefault(x => x.Text == tagText);
                if (tag == null) {
                    tag = new Tag();
                    tag.Text = tagText;
                    recipeContext.Tags.Add(tag);
                    recipeContext.SaveChanges();
                }

                recipe.RecipeTags.Add(new RecipeTag {
                    Recipe = recipe,
                    Tag = tag,
                    RecipeId = recipe.Id,
                    TagId = tag.Id
                });
            }

            recipeContext.SaveChanges();
        }

        public List<Recipe> ListRecipes() {
            return recipeContext.Recipes
                .Include(x => x.RecipeTags)
                    .ThenInclude(x => x.Tag)
                .ToList();
        }

        public void UpdateRecipes(UpdateRecipeRequest request) {
            if (request == null)
                throw new ActionFailedException("invalid request body");
            if (request.Id == 0)
                throw new ActionFailedException("id cannot be empty");

            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ActionFailedException("title cannot be empty");
            if (string.IsNullOrWhiteSpace(request.Text))
                throw new ActionFailedException("title cannot be empty");

            request.Title = request.Title.Trim();
            request.Text = request.Text.Trim();

            if (request.Title.Length < 8)
                throw new ActionFailedException("title is too short");
            if (request.Text.Length < 20)
                throw new ActionFailedException("recipe details is too short");

            if (request.Portion <= 0)
                throw new ActionFailedException("portion cannot be smaller than 1");
            if (request.Portion > 20)
                throw new ActionFailedException("portion is too large");

            if (request.PrepareTime <= 0)
                throw new ActionFailedException("prepare time cannot be smaller than 1");
            if (request.PrepareTime > 600)
                throw new ActionFailedException("prepare time is too large");

            if (request.Ingredients == null)
                throw new ActionFailedException("ingredients cannot be empty");

            if (request.Steps == null)
                throw new ActionFailedException("steps cannot be empty");

            foreach (var item in request.Ingredients) {
                if (string.IsNullOrWhiteSpace(item))
                    throw new ActionFailedException("ingredient items cannot be empty");
            }

            foreach (var item in request.Steps) {
                if (string.IsNullOrWhiteSpace(item))
                    throw new ActionFailedException("step items cannot be empty");
            }

            Recipe recipe = recipeContext.Recipes.Include(x=>x.RecipeTags).FirstOrDefault(x => x.Id == request.Id);

            if (recipe == null)
                throw new ActionFailedException("invalid recipe");

            if (request.Tags == null)
                request.Tags = new string[0];

            recipe.RecipeTags.Clear();

            recipe.Title =request.Title;
            recipe.Text =request.Text;
            recipe.Ingredients = JsonConvert.SerializeObject(request.Ingredients);
            recipe.Steps = JsonConvert.SerializeObject(request.Steps);
            recipe.ModifiedTime = DateTime.Now;
            recipe.Portion = request.Portion;
            recipe.PrepareTime = request.PrepareTime;
            if (request.Photos != null)
                recipe.Photos = JsonConvert.SerializeObject(request.Photos);
            recipe.RecipeTags = new List<RecipeTag>();

            foreach (var item in request.Tags)
            {
                if (string.IsNullOrWhiteSpace(item))
                    throw new ActionFailedException("tags cannot be empty");

                var tagText = item.ToLower();
                Tag tag = recipeContext.Tags.FirstOrDefault(x => x.Text == tagText);
                if (tag == null)
                {
                    tag = new Tag();
                    tag.Text = tagText;
                    recipeContext.Tags.Add(tag);
                }

                recipe.RecipeTags.Add(new RecipeTag
                {
                    Recipe = recipe,
                    Tag = tag
                });
            }


            //TO-DO Recipe.User
            //TO-DO Photo ve Tag için add remove endpoint 

            recipeContext.SaveChanges();
        }
        
        public void DeleteRecipe(int id, User user)
        {
            var recipe = recipeContext.Recipes.Include(x => x.Photos).Include(x => x.RecipeTags).ThenInclude(x => x.Tag).FirstOrDefault(x => x.User == user && x.Id == id);
            if (recipe == null)
            {
                throw new ActionFailedException("Recipe with ID=" + id.ToString() + "is not found.");
            }

            if (recipe.RecipeTags.Count != 0 )
            {
                List<Tag> tags = recipe.RecipeTags.Select(x => x.Tag).ToList();

                foreach (Tag tag in tags)
                {
                    List<Recipe> recipes = recipeContext.Recipes.Select(x => x.RecipeTags.SingleOrDefault(y => y.Tag == tag && y.Recipe != recipe)).Where(x => x != null).Select(x => x.Recipe).ToList();
                    Console.WriteLine(recipes.Count);
                    if (recipes.Count == 0)
                        recipeContext.Tags.Remove(tag);
                }
            }
           
            recipeContext.Recipes.Remove(recipe);
            recipeContext.SaveChanges();
        }

        public List<Recipe> ListWithLabels(int id)
        {
            Tag tag = recipeContext.Tags.Find(id);
            if(tag == null)
            {
                throw new ActionFailedException("Tag is not found");
            }
            List<Recipe> recipes = recipeContext.Recipes.Select(x => x.RecipeTags.SingleOrDefault(y=> y.Tag == tag)).Where(x => x!=null).Select(x=>x.Recipe).ToList();
            Console.WriteLine(recipes.Count);
            if (recipes == null)
            {
                throw new ActionFailedException("No such a recipe!"); //buna gerek kalmayabilir 
            }
            return recipes;
        }

        public void LikeRecipe(int recipeId, User user)
        {
            var recipe = recipeContext.Recipes.Include(x => x.RecipeLikes).FirstOrDefault(x => x.Id == recipeId);
            if (recipe == null) return;

            if (IsUserLiked(recipeId, user.Id)) {
                var olds = recipe.RecipeLikes.Where(x => x.RecipeId == recipeId && x.UserId == user.Id).ToArray();
                foreach (var item in olds)
                    recipe.RecipeLikes.Remove(item);
                recipeContext.SaveChanges();
            } else {
                UserRecipe ur = new UserRecipe { Recipe = recipe, User = user };
                recipe.RecipeLikes.Add(ur);
                recipeContext.SaveChanges();
            }
        }
        public List<Recipe> GlobalSearch(string query) {
            return SearchByQueryAndUser(query, null);
        }

        public List<Recipe> SearchByQueryAndUser(string search, User user) {
            var allRecipes = recipeContext.Recipes
                .Include(x => x.User)
                .Include(x => x.RecipeTags)
                .ToList();
            var allTags = recipeContext.Tags.ToDictionary(x => x.Id);
            if (user != null) {
                allRecipes = allRecipes.Where(x => x.User.Id == user.Id).ToList();
            }
            if (!string.IsNullOrWhiteSpace(search)) {
                search = search.Trim();
                allRecipes = allRecipes.Where(
                    x => (x.Title.Contains(search)) || (x.Text.Contains(search))
                    || (x.Ingredients.Contains(search)) || (x.Steps.Contains(search))
                    || (x.RecipeTags.Any(y => allTags[y.TagId].Text.Contains(search)))
                    ).ToList();
            }
            return allRecipes;
        }

        public Recipe GetReceiveById(int id)
        {
            var recipe = recipeContext.Recipes.Include(x => x.User).FirstOrDefault(x => x.Id == id);
            if (recipe == null)
                throw new ActionFailedException("Recipe with ID=" + id.ToString() + "is not found.");
            return recipe;
        }

        public string[] GetTagsOfRecipe(Recipe recipe) {
            int[] tagIds = recipeContext.Recipes
                .Include(x => x.RecipeTags)
                .FirstOrDefault(x => x.Id == recipe.Id).RecipeTags.Select(x => x.TagId).ToArray();
            return tagIds.Select(x => recipeContext.Tags.Find(x).Text).ToArray();
        }

        public List<Recipe> ListMyRecipes(User user) {
            return recipeContext.Recipes
                 .Include(x => x.Photos)
                 .Include(x => x.RecipeTags)
                     .ThenInclude(x => x.Tag)
                 .Where(x => x.User.Id == user.Id).ToList();
        }

        public bool IsUserLiked(int recipeId, int userId) {
            var recipe = recipeContext.Recipes.Include(x => x.RecipeLikes).FirstOrDefault(x => x.Id == recipeId);
            if (recipe == null)
                return false;
            return recipe.RecipeLikes.Any(x => x.UserId == userId);
        }

        public int GetLikeCount(int id) {
            var recipe = recipeContext.Recipes.Include(x => x.RecipeLikes).FirstOrDefault(x => x.Id == id);
            if (recipe == null)
                return 0;
            return recipe.RecipeLikes.Count();
        }
    }
} 
