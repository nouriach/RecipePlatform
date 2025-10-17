using RecipePlatform.DataManager.Persistence.DTOs;

namespace RecipePlatform.DataManager.Application.Abstractions;

public interface IStorageClient
{
    Task<bool> SaveRecipesAsync(List<RecipeDto> recipes);
}