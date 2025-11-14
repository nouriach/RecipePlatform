using RecipePlatform.DataManager.Persistence.DTOs;

namespace RecipePlatform.DataManager.Application.Abstractions;

public interface INotificationClient
{
    Task SendConfirmationAsync();
}