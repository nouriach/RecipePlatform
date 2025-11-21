namespace RecipePlatform.NotificationService.Application.Abstractions;

public interface IRecipeEmailOrchestrator
{
    Task SendRecipeRecommendationsAsync();
    Task SendRecipeConfirmationAsync();
}