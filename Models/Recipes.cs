using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RecipeBook.Models
{
    public partial class Recipes : DbContext
    {
        public Recipes()
            : base("name=DefaultConnection")
        {
        }

        public virtual DbSet<Recipe> Recipe { get; set; }
        public virtual DbSet<Ingredients> Ingredients { get; set; }
    }
}