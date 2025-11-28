using System.Net;
using System.Text;
using System.Text.Json;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using RecipePlatform.Core.Models;
using RecipePlatform.NotificationService.Application.Abstractions;
using RecipePlatform.NotificationService.Infrastructure.Templates;

namespace RecipePlatform.NotificationService.Infrastructure;

public class SimpleNotificationClient(IAmazonSimpleEmailService simpleEmailClient, IConfiguration config) : INotificationClient
{
    public async Task SendRecipeRecommendationAsync(List<Recipe> recipes)
    {
        try
        {
            Console.WriteLine($"--> Sending email using SES with {recipes.Count} recipes.");

            var toEmail = config["/email/send_to"];
            var fromEmail = config["/email/send_from"];
            if (string.IsNullOrEmpty(toEmail) || string.IsNullOrEmpty(fromEmail))
                throw new Exception("Failed to retrieve email from SSM");

            var emailContent = BuildHtmlBody(recipes);

            Console.WriteLine($"Retrieving To and From emails from config: {toEmail}, {fromEmail}");

            var sendRequest = new SendTemplatedEmailRequest()
            {
                Source = fromEmail, 
                Template = EmailTemplates.RecipeRecommendationTemplate,
                Destination = new Destination
                {
                    ToAddresses = new List<string> { toEmail }
                },
                TemplateData = JsonSerializer.Serialize(new {name = "Nathan", recipes_html = BuildHtmlBody(recipes)})
            };

            var response = await simpleEmailClient.SendTemplatedEmailAsync(sendRequest);

            Console.WriteLine(response.HttpStatusCode == HttpStatusCode.OK ? "---> Email sent successfully." : "---> Email failed to send.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SendRecipeConfirmationAsync(List<Recipe> recipes)
    {
        try
        {

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private string BuildHtmlBody(List<Recipe> emailContent)
    {
        var recipesHtml = new StringBuilder();
        foreach (var recipe in emailContent)
        {
            recipesHtml.Append($@"
                <div class='recipe'>
                    <h2>{recipe.Title}</h2>
                    <p><strong>Prep Time:</strong> {recipe.PrepTime} |
                       <strong>Cook Time:</strong> {recipe.CookTime} |
                       <strong>Serves:</strong> {recipe.Serves}</p>
                </div>
            ");
        }

        return recipesHtml.ToString();
    }
}