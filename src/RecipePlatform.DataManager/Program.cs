using Amazon.DynamoDBv2;
using Amazon.S3;
using RecipePlatform.DataManager.Application;
using RecipePlatform.DataManager.Application.Abstractions;
using RecipePlatform.DataManager.Application.Commands;
using RecipePlatform.DataManager.Infrastructure;
using RecipePlatform.DataManager.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddAWSService<IAmazonS3>();

builder.Services.AddSingleton<IRecipesRepository, RecipesRepository>();
builder.Services.AddSingleton<IStorageClient, StorageClient>();
builder.Services.AddSingleton<INotificationClient, EmailClient>();
builder.Services.AddSingleton<IRecipesService, RecipesService>();
builder.Services.AddSingleton<INotificationService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/health", () => "healthy");

app.MapGet("recipes/random", async (IRecipesService recipesService) =>
{
    var recipes = await recipesService.GetRandomRecipesAsync();
    return recipes.Count > 0 ? Results.Ok(recipes) : Results.NotFound();
});

app.MapGet("recipes/latest", async (IRecipesService recipesService) =>
{
    var recipes = await recipesService.GetLatestRecipesAsync();
    return recipes.Count > 0 ? Results.Ok(recipes) : Results.NotFound();
});

app.MapPost("recipes/confirm", async (
    ConfirmRecipesCommand command, 
    IRecipesService recipesService,
    INotificationService notificationService) =>
{
    var recipesConfirmed = await recipesService.ConfirmRecipesAsync(command.RecipeIds);
    if (!recipesConfirmed)
        return Results.BadRequest();

    await notificationService.SendConfirmation();

    return Results.Ok();
});

app.MapGet("recipes/migrate", async (IRecipesRepository repo) =>
{
    Console.WriteLine("---> In endpoint: recipes/migrate");
    await repo.MigrateData();
    return Results.Ok();
});

app.Run();
