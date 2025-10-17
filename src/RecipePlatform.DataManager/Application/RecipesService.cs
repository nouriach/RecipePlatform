using RecipePlatform.DataManager.Application.Abstractions;
using RecipePlatform.DataManager.Persistence.DTOs;

namespace RecipePlatform.DataManager.Application;

public class RecipesService(IRecipesRepository recipesRepository, IStorageClient storageClient) : IRecipesService
{
    public async Task<List<RecipeDto>> GetRandomRecipesAsync()
    {
        var recipesDto = await recipesRepository.GetRandomRecipesByBookAsync();
        var hasSaved = await storageClient.SaveRecipesAsync(recipesDto);

        if (!hasSaved)
            throw new Exception("Failed to save to storage.");

        return recipesDto;
    }

    public async Task<List<RecipeDto>> GetLatestRecipesAsync()
    {
        return await storageClient.GetLatestRecipesAsync();
    }
}