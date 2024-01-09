namespace RecipeBook.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Recipe")]
    public partial class Recipe
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(50)]
        public string PrepTime { get; set; }

        [StringLength(50)]
        public string CookTime { get; set; }
        public string UserID { get; set; }

        public List<Ingredients> Ingredients { get; set; }
    }
    public partial class RecipeForView
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(50)]
        public string PrepTime { get; set; }

        [StringLength(50)]
        public string CookTime { get; set; }
        public List<Ingredients> Ingredients { get; set; }
    }
    public partial class Ingredients
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Qty { get; set; }
        public int RecipeID { get; set; }
        public Recipe Recipe { get; set; }
    }
}
