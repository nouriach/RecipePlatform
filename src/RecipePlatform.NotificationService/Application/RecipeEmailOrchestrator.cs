using RecipePlatform.NotificationService.Application.Abstractions;

namespace RecipePlatform.NotificationService.Application;

public class RecipeEmailOrchestrator(INotificationClient notificationClient, IStorageClient storageClient) : IRecipeEmailOrchestrator
{
    public async Task SendRecipeRecommendationsAsync()
    {
        var recipes = await storageClient.GetLatestRecipesAsync();
        if (recipes.Count > 0)
        {
            await notificationClient.SendRecipeRecommendationAsync(recipes);
        }
        else
        {
            throw new Exception("No recipes retrieved.");
        }
    }

    public async Task SendRecipeConfirmationAsync()
    {
        var recipes = await storageClient.GetLatestRecipesAsync();
        if (recipes.Count > 0)
        {
            await notificationClient.SendRecipeConfirmationAsync(recipes);
        }
    }
}