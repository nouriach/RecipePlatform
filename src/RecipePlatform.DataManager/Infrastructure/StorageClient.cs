using System.Net;
using System.Text;
using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;
using RecipePlatform.DataManager.Application.Abstractions;
using RecipePlatform.DataManager.Persistence.DTOs;

namespace RecipePlatform.DataManager.Infrastructure;

public class StorageClient(IAmazonS3 s3Client) : IStorageClient
{
    private const string BucketName = "weekly-recipe-recommendations";
    private const string KeyName = "recipe-data";
    
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };


    public async Task<bool> SaveRecipesAsync(List<RecipeDto> recipes)
    {
        await DeleteBucketDataIfExistsAsync();
        try
        {
            var json = JsonSerializer.Serialize(recipes, _jsonSerializerOptions);
            Console.WriteLine("---> Serializing JSON");
            Console.WriteLine($"---> JSON to upload: {json}");

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var putRequest = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = KeyName,
                InputStream = stream,
                ContentType = "application/json"
            };

            Console.WriteLine($"---> Executing putrequest: {putRequest}");
            var resp = await s3Client.PutObjectAsync(putRequest);
            if (resp.HttpStatusCode != HttpStatusCode.OK)
                return false;

            Console.WriteLine("---> Upload successful");

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"---> EXCEPTION THROWN: {e.Message}");
            throw;
        }
    }

    public async Task<List<RecipeDto>> GetLatestRecipesAsync()
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
                return new List<RecipeDto>();

            var recipes = JsonSerializer.Deserialize<List<RecipeDto>>(json, _jsonSerializerOptions);
            return recipes ?? new List<RecipeDto>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    private async Task DeleteBucketDataIfExistsAsync()
    {
        try
        {
            Console.WriteLine($"---> Attempting to retrieve bucket: {KeyName} from {BucketName}");
            await s3Client.GetObjectMetadataAsync(BucketName, KeyName);

            Console.WriteLine($"---> {KeyName} exists. Attempting to delete Bucket.");
            await s3Client.DeleteObjectAsync(BucketName, KeyName);
            Console.WriteLine($"---> {KeyName} from {BucketName} has been deleted.");
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            Console.WriteLine($"---> Error occured while trying to delete {KeyName} from {BucketName}.");
            Console.WriteLine(ex.StatusCode);
            Console.WriteLine(ex.Message);
        }
    }
}