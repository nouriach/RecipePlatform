using RecipePlatform.Core.Models;

namespace RecipePlatform.NotificationService.Application.Abstractions;

public interface INotificationClient
{
    Task SendRecipeRecommendationAsync(List<Recipe> recipes);
    Task SendRecipeConfirmationAsync(List<Recipe> recipes);
}