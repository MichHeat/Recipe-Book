using Microsoft.AspNet.Identity;
using RecipeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace RecipeBook.Controllers
{
    public class HomeController : Controller
    {
        Recipes db = new Recipes();
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Recipes()
        {

            return View();
        }
        [Authorize]

        public ActionResult YourRecipes()
        {
            string userID = User.Identity.GetUserId();
            List<Recipe> recipes = db.Recipe.Where(a => a.UserID == userID).ToList();
            return View(recipes);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddRecipe(RecipeForView recipe)
        {
            if (recipe == null)
            {
                ViewBag.Message = "Invalid recipe data";
                return Json(new { Response = "Error" });
            }
            string UserID = User.Identity.GetUserId();
            Recipe res = new Recipe()
            {
                Title = recipe.Title,
                Description = recipe.Description,
                CookTime = recipe.CookTime,
                PrepTime = recipe.PrepTime,
                Ingredients = recipe.Ingredients,// Assuming you have Ingredients property in your Recipe model
                UserID = UserID,
            };

            db.Recipe.Add(res);
            db.SaveChanges();

            ViewBag.Message = "Recipe added successfully";
            return Json(new { Response = "Success" });
        }
        [Authorize]
        public ActionResult EditRecipe(int id)
        {
            Recipe recipe = db.Recipe.Where(a => a.Id == id).Include(a => a.Ingredients).FirstOrDefault();

            if (recipe == null)
            {
                return HttpNotFound();
            }

            return View(recipe);
        }
        [Authorize]
        [HttpPost]
        public ActionResult SaveRecipe(Recipe recipe)
        {
            // Check if the recipe exists
            var existingRecipe = db.Recipe.Include(r => r.Ingredients).SingleOrDefault(r => r.Id == recipe.Id);

            if (existingRecipe != null)
            {
                // Update scalar properties of the existing recipe
                db.Entry(existingRecipe).CurrentValues.SetValues(recipe);

                // Handle ingredients
                UpdateIngredients(existingRecipe, recipe);

                // Save changes to the database
                db.SaveChanges();

                return RedirectToAction("YourRecipes"); // Redirect to the recipe listing page
            }

            // Handle the case where the recipe does not exist
            return HttpNotFound();
        }

        [Authorize]
        private void UpdateIngredients(Recipe existingRecipe, Recipe updatedRecipe)
        {
            // If ingredients are provided, update or add new ingredients
            if (updatedRecipe.Ingredients != null)
            {
                foreach (var existingIngredient in existingRecipe.Ingredients.ToList())
                {
                    var updatedIngredient = updatedRecipe.Ingredients.FirstOrDefault(i => i.Id == existingIngredient.Id);

                    if (updatedIngredient == null)
                    {
                        // Ingredient was deleted
                        db.Ingredients.Remove(existingIngredient);
                    }
                    else
                    {
                        // Update existing ingredient
                        db.Entry(existingIngredient).CurrentValues.SetValues(updatedIngredient);
                    }
                }

                // Remove all ingredients if none are provided in the update
                if (updatedRecipe.Ingredients.Count == 0)
                {
                    foreach (var existingIngredient in existingRecipe.Ingredients.ToList())
                    {
                        db.Ingredients.Remove(existingIngredient);
                    }
                }
                else
                {
                    // Add new ingredients
                    foreach (var newIngredient in updatedRecipe.Ingredients.Where(i => i.Id == 0))
                    {
                        existingRecipe.Ingredients.Add(newIngredient);
                    }
                }
            }
            else
            {
                // Remove all existing ingredients
                foreach (var existingIngredient in existingRecipe.Ingredients.ToList())
                {
                    db.Ingredients.Remove(existingIngredient);
                }
            }
        }
        [Authorize]
        public ActionResult DeleteRecipe(int id)
        {
            // Find the recipe in the database
            var recipeToDelete = db.Recipe.Find(id);

            if (recipeToDelete != null)
            {
                // Remove the recipe from the database
                db.Recipe.Remove(recipeToDelete);

                // Save changes to the database
                db.SaveChanges();

                return RedirectToAction("YourRecipes"); // Redirect to the recipe listing page
            }

            // Handle the case where the recipe does not exist
            return HttpNotFound();
        }



    }
}