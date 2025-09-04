using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Text.Json;

namespace backend.Utilities
{
    public interface IJsonFileProcessor
    {
        Task<JsonDocument> ProcessJsonFile(IFormFile jsonFile);
        Task<string> SaveJsonFile(IFormFile jsonFile, string subfolder = "uploads/json");
    }

    public class JsonFileProcessor : IJsonFileProcessor
    {
        private readonly IWebHostEnvironment _environment;
        private const int MaxFileSize = 5 * 1024 * 1024; // 5MB
        private static readonly string[] AllowedContentTypes = { "application/json" };

        public JsonFileProcessor(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<JsonDocument> ProcessJsonFile(IFormFile jsonFile)
        {
            ValidateJsonFile(jsonFile);

            using var streamReader = new StreamReader(jsonFile.OpenReadStream());
            string jsonContent = await streamReader.ReadToEndAsync();

            try
            {
                return JsonDocument.Parse(jsonContent);
            }
            catch (JsonException ex)
            {
                throw new InvalidDataException("Invalid JSON format: " + ex.Message);
            }
        }

        public async Task<string> SaveJsonFile(IFormFile jsonFile, string subfolder = "uploads/json")
        {
            ValidateJsonFile(jsonFile);

            var uploadsFolder = Path.Combine(_environment.WebRootPath ??
                              Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
                              subfolder);

            Directory.CreateDirectory(uploadsFolder);
            var fileName = $"{Guid.NewGuid()}.json";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await jsonFile.CopyToAsync(fileStream);

            return $"/{subfolder}/{fileName}";
        }

        private void ValidateJsonFile(IFormFile jsonFile)
        {
            if (jsonFile == null || jsonFile.Length == 0)
                throw new ArgumentException("No JSON file provided");

            if (jsonFile.Length > MaxFileSize)
                throw new ArgumentException($"JSON file exceeds maximum size of {MaxFileSize / 1024 / 1024}MB");

            var contentType = jsonFile.ContentType.ToLower();
            var extension = Path.GetExtension(jsonFile.FileName).ToLower();

            if (!AllowedContentTypes.Contains(contentType) || extension != ".json")
                throw new ArgumentException("Only JSON files are allowed");
        }
    }
}
