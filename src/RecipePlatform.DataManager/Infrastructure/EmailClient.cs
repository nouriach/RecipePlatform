using RecipePlatform.DataManager.Application.Abstractions;
using RecipePlatform.DataManager.Persistence.DTOs;

namespace RecipePlatform.DataManager.Infrastructure;

public class EmailClient : INotificationClient
{
    public Task SendConfirmationAsync()
    {
        // call the Recipes.NotificationService Lambda
        throw new NotImplementedException();
    }
}