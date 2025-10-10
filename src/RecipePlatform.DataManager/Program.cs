using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Http.HttpResults;
using RecipePlatform.DataManager.Application.Abstractions;
using RecipePlatform.DataManager.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddSingleton<IRecipesRepository, RecipesRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/hello", () => "hello");

app.MapGet("/random-recipes", async (IRecipesRepository repo) =>
{
    var recipes = await repo.GetRandomRecipesByBook();
    return recipes.Count > 0 ? Results.Ok(recipes) : Results.NotFound();
});

app.Run();
