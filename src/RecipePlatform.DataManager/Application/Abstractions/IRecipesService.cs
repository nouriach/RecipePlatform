using RecipePlatform.DataManager.Persistence.DTOs;

namespace RecipePlatform.DataManager.Application.Abstractions;

public interface IRecipesService
{
    Task<List<RecipeDto>> GetRandomRecipesAsync();
}