using System.ComponentModel.DataAnnotations;

namespace DoAnFlutterAPI.DTOs
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? IconUrl { get; set; }
        public bool IsActive { get; set; }
    }

    public class CategoryCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? IconUrl { get; set; }
    }

    public class MenuItemDto
    {
        public int MenuItemId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public int StockQty { get; set; }
        public int SortOrder { get; set; }
        public string? CategoryName { get; set; }
    }

    public class MenuItemCreateDto
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int StockQty { get; set; }
        public int SortOrder { get; set; }
    }

    public class ItemIngredientDto
    {
        public int ItemIngredientId { get; set; }
        public int MenuItemId { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; } = null!;
        public decimal AmountPerServing { get; set; }
        public string Unit { get; set; } = null!;
    }

    public class ItemIngredientCreateDto
    {
        public int MenuItemId { get; set; }
        public int IngredientId { get; set; }
        public decimal AmountPerServing { get; set; }
    }
}
