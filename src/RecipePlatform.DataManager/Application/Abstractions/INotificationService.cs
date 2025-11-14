using RecipePlatform.DataManager.Persistence.DTOs;

namespace RecipePlatform.DataManager.Application.Abstractions;

public interface INotificationService
{
    Task SendConfirmation();
}