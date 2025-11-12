using RecipePlatform.DataManager.Persistence.DTOs;

namespace RecipePlatform.DataManager.Application.Abstractions;

public interface IRecipesRepository
{
    Task<List<RecipeDto>> GetRandomRecipesByBookAsync(int? count = 8, string? book = "Green");
    Task<List<RecipeDto?>> ConfirmRecipesAsync(List<string> ids);
    Task MigrateData();
}