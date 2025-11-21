using Amazon.S3;
using Amazon.SimpleEmail;
using Amazon.SimpleSystemsManagement;
using RecipePlatform.NotificationService.Application;
using RecipePlatform.NotificationService.Application.Abstractions;
using RecipePlatform.NotificationService.Config;
using RecipePlatform.NotificationService.Infrastructure;

var builder = WebApplication.CreateBuilder();

// configure your app

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
builder.Configuration.AddApiConfiguration(config =>
{
    config.SsmClientFactory = () => new AmazonSimpleSystemsManagementClient();
});
    
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddAWSService<IAmazonSimpleEmailService>();


builder.Services.AddSingleton<IRecipeEmailOrchestrator, RecipeEmailOrchestrator>();
builder.Services.AddSingleton<IStorageClient, StorageClient>();
builder.Services.AddSingleton<INotificationClient, SimpleNotificationClient>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("recipes/send-recommendations", async (IRecipeEmailOrchestrator recipeOrchestrator) =>
{
    await recipeOrchestrator.SendRecipeRecommendationsAsync();
    return Results.Ok(); Results.NotFound();
});

app.MapGet("recipes/send-confirmation", async (IRecipeEmailOrchestrator recipeOrchestrator) =>
{
    await recipeOrchestrator.SendRecipeConfirmationAsync();
    return Results.Ok(); Results.NotFound();
});

app.Run();