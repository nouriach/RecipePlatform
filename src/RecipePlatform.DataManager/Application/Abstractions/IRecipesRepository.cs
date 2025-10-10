using RecipePlatform.DataManager.Persistence.DTOs;

namespace RecipePlatform.DataManager.Application.Abstractions;

public interface IRecipesRepository
{
    Task<List<RecipeDto>> GetRandomRecipesByBook(int? count = 8, string? book = "Green");
    Task UpdateRecipeLastRecommendationDate(List<string> ids);
}