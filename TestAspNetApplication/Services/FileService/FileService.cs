using Microsoft.AspNetCore.Authentication.Cookies;

using System.Security.Claims;

using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.Data;
using TestAspNetApplication.DTO;

namespace TestAspNetApplication.Services
{
    public class FileService
    {
        private readonly ILogger<FileService> _logger;
        public FileService(ILogger<FileService> logger) 
        {
            _logger = logger;
        }
        public async Task<string> UploadFormFile(IFormFile file, string directoryName, Guid contentId)
        {
            string relativeDir = $"content/{directoryName}/{contentId}";
            string uploadDirectory = Path.Combine($"{Directory.GetCurrentDirectory()}\\wwwroot", relativeDir);
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }
            string relativePath = $"{relativeDir}/{file.FileName}";
            string uploadPath = Path.Combine(uploadDirectory, file.FileName);
            using (FileStream fileStream = new FileStream(uploadPath, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fileStream);
            }
            return relativePath;
        }
    }
}