using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime.Documents;
using RecipePlatform.DataManager.Application.Abstractions;
using RecipePlatform.DataManager.Persistence.DTOs;
using Swashbuckle.AspNetCore.Swagger;
using Document = Amazon.DynamoDBv2.DocumentModel.Document;

namespace RecipePlatform.DataManager.Persistence.Repositories;

public class RecipesRepository : IRecipesRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;

    public RecipesRepository(IAmazonDynamoDB dynamoDb)
    {
        _dynamoDb = dynamoDb;
    }
    public async Task<List<RecipeDto>> GetRandomRecipesByBookAsync(int? count, string? book)
    {
        try
        {
            Console.WriteLine("---> About to call DynamoDB");
            var request = new QueryRequest
            {
                TableName = "RecipesV2",
                IndexName = "Book-LastRecommendedAt-index",
                KeyConditionExpression = "#partitionKey = :partitionKeyValue AND #sortKey < :sortKeyValue",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    { "#partitionKey", "Book" }, // Map the attribute name with hyphens
                    { "#sortKey", "LastRecommendedAt" }
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":partitionKeyValue", new AttributeValue { S = book } },
                    { ":sortKeyValue", new AttributeValue { S = "2024-05-02T00:00:00.000Z" } } // Exact match required - change to greater than in the future
                },
                Limit = count
            };
            var recipes = await _dynamoDb.QueryAsync(request);

            if (recipes.Items.Count == 0)
                throw new Exception("---> No recipes returned from the1 database.");

            Console.WriteLine("---> Items retrieved from DynamoDB");

            var returnedRecipes = recipes.Items;

            var recipeDtos = returnedRecipes.Select(dto => new RecipeDto
            {
                RecipeId = dto["recipeId"].S,
                Title = dto["Title"].S,
                Serves = int.Parse(dto["Serves"].N),
                PrepTime = dto["PrepTime"].S,
                CookTime = dto["CookTime"].S,
            });
            // display ingredients as well in the future
            return recipeDtos.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine("---> Exception thrown when calling DynamoDB");

            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<List<RecipeDto?>> ConfirmRecipesAsync(List<string> ids)
    {
        try
        {
            var updatedItems = new List<UpdateItemResponse>();

            foreach (var id in ids)
            {
                var updateRequest = new UpdateItemRequest
                {
                    TableName = "RecipesV2",
                    Key = new Dictionary<string, AttributeValue>
                    {
                        ["recipeId"] = new AttributeValue { S = id }
                    },
                    UpdateExpression = "SET LastRecommendedAt = :val",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        [":val"] = new AttributeValue { S = DateTime.UtcNow.ToString("yyyy-MM-dd") }
                    },
                    ReturnValues = "ALL_NEW"
                };
                    
                var updateResponse = await _dynamoDb.UpdateItemAsync(updateRequest);

                updatedItems.Add(updateResponse);
            }

            var recipeDtos = updatedItems.Select(x => TransformIntoDto(x));
            return recipeDtos.ToList() ?? new List<RecipeDto>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task MigrateData()
    {
        Console.WriteLine("---> In method: MigrateData");
        var oldItems = await _dynamoDb.ScanAsync(new ScanRequest { TableName = "Recipes" });
        Console.WriteLine($"Items retrieved from original table: {oldItems.Items.Count}");

        try
        {
            foreach (var item in oldItems.Items)
            {
                item.Remove("RecommendationKey"); // drop the range key
                await _dynamoDb.PutItemAsync(new PutItemRequest
                {
                    TableName = "RecipesV2",
                    Item = item
                });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    private RecipeDto? TransformIntoDto(UpdateItemResponse response)
    {
        var doc = Document.FromAttributeMap(response.Attributes);
        var recipeDto = JsonSerializer.Deserialize<RecipeDto>(
            doc.ToJson(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true } 
        );
        return recipeDto;
    }
}