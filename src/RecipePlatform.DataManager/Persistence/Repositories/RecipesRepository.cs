using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using RecipePlatform.DataManager.Application.Abstractions;
using RecipePlatform.DataManager.Persistence.DTOs;

namespace RecipePlatform.DataManager.Persistence.Repositories;

public class RecipesRepository : IRecipesRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;

    public RecipesRepository(IAmazonDynamoDB dynamoDb)
    {
        _dynamoDb = dynamoDb;
    }
    public async Task<List<RecipeDto>> GetRandomRecipesByBook(int? count, string? book)
    {
        try
        {
            Console.WriteLine("---> About to call DynamoDB");
            var request = new QueryRequest
            {
                TableName = "Recipes",
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

    public Task UpdateRecipeLastRecommendationDate(List<string> ids)
    {
        throw new NotImplementedException();
    }
}