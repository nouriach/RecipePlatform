using Amazon.DynamoDBv2;
using Amazon.S3;
using Microsoft.AspNetCore.Http.HttpResults;
using RecipePlatform.DataManager.Application;
using RecipePlatform.DataManager.Application.Abstractions;
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
builder.Services.AddSingleton<IRecipesService, RecipesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/hello", () => "hello");

app.MapGet("/random-recipes", async (IRecipesService recipesService) =>
{
    var recipes = await recipesService.GetRandomRecipesAsync();
    return recipes.Count > 0 ? Results.Ok(recipes) : Results.NotFound();
});

app.Run();
