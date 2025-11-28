namespace RecipePlatform.Core.Models;

public class Recipe
{
    public string RecipeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Serves { get; set; }
    public string PrepTime { get; set; } = string.Empty;
    public string CookTime { get; set; } = string.Empty;
    public List<string> Ingredients { get; set; } = new();
    public string Book { get; set; }
}