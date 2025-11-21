using RecipePlatform.Core.Models;

namespace RecipePlatform.NotificationService.Application.Abstractions;

public interface IStorageClient
{
    Task<List<Recipe>> GetLatestRecipesAsync();
}