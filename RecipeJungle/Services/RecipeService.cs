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

        public void CreateRecipe(CreateRecipeRequest request) {
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

            foreach (var item in request.Ingredients) {
                if (string.IsNullOrWhiteSpace(item))
                    throw new ActionFailedException("ingredient items cannot be empty");
            }

            if (request.Photos == null)
                request.Photos = new List<int>();
            if (request.Tags == null)
                request.Tags = new List<string>();

            Recipe recipe = new Recipe();
            recipe.Id = 0;
            recipe.Title = request.Title;
            recipe.Text = request.Text;
            recipe.Ingredients = JsonConvert.SerializeObject(request.Ingredients);
            recipe.CreatedTime = DateTime.Now;
            recipe.ModifiedTime = DateTime.Now;
            recipe.Portion = request.Portion;
            recipe.PrepareTime = request.PrepareTime;
            recipe.User = null; // TODO
            recipe.Photos = new List<Photo>();
            recipe.RecipeTags = new List<RecipeTag>();

            foreach (var item in request.Photos) {
                Photo photo = recipeContext.Photos.Find(item);
                if (photo == null)
                    throw new ActionFailedException("photo not found");
                recipe.Photos.Add(photo);
            }

            foreach (var item in request.Tags) {
                if (string.IsNullOrWhiteSpace(item))
                    throw new ActionFailedException("tags cannot be empty");

                var tagText = item.ToLower();
                Tag tag = recipeContext.Tags.FirstOrDefault(x => x.Text == tagText);
                if (tag == null) {
                    tag = new Tag();
                    tag.Text = tagText;
                    recipeContext.Tags.Add(tag);
                }

                recipe.RecipeTags.Add(new RecipeTag {
                    Recipe = recipe,
                    Tag = tag
                });
            }

            recipeContext.Recipes.Add(recipe);
            recipeContext.SaveChanges();
        }

        public List<Recipe> ListRecipes() {
            return recipeContext.Recipes
                .Include(x => x.Photos)
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

            foreach (var item in request.Ingredients)
            {
                if (string.IsNullOrWhiteSpace(item))
                    throw new ActionFailedException("ingredient items cannot be empty");
            }

            Recipe recipe = recipeContext.Recipes.Find(request.Id);

            if (recipe == null)
                throw new ActionFailedException("invalid recipe");

            if (request.Photos == null)
                request.Photos = new List<int>();
            if (request.Tags == null)
                request.Tags = new List<string>();

            recipe.Title =request.Title;
            recipe.Text =request.Text;
            recipe.Ingredients = JsonConvert.SerializeObject(request.Ingredients);
            recipe.ModifiedTime = DateTime.Now;
            recipe.Portion = request.Portion;
            recipe.PrepareTime = request.PrepareTime;
            recipe.Photos = new List<Photo>();
            recipe.RecipeTags = new List<RecipeTag>();

            //TO-DO Recipe.User
            //TO-DO Photo ve Tag için add remove endpoint 

            recipeContext.SaveChanges();
        }
    }
}
