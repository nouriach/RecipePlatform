using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;
using RecipePlatform.Core.Models;
using RecipePlatform.NotificationService.Application.Abstractions;

namespace RecipePlatform.NotificationService.Infrastructure;

public class StorageClient(IAmazonS3 s3Client) : IStorageClient
{
    // Eventually, store these in Parameter Store
    private const string BucketName = "weekly-recipe-recommendations";
    private const string KeyName = "recipe-data";
    
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    public async Task<List<Recipe>> GetLatestRecipesAsync()
    {
        try
        {
            var getRequest = new GetObjectRequest
            {
                BucketName = BucketName,
                Key = KeyName
            };
            
            using var response = await s3Client.GetObjectAsync(getRequest);

            using var reader = new StreamReader(response.ResponseStream);
            var json = await reader.ReadToEndAsync();
            if (string.IsNullOrEmpty(json))
                return new List<Recipe>();

            var recipes = JsonSerializer.Deserialize<List<Recipe>>(json, _jsonSerializerOptions);
            return recipes ?? new List<Recipe>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}