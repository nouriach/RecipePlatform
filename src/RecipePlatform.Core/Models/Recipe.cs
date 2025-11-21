namespace RecipePlatform.Core.Models;

public class Recipe
{
    public string RecipeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public RecipeSummary Summary { get; set; } = null!;
    public List<string> Ingredients { get; set; } = new();
    public string Book { get; set; }
}