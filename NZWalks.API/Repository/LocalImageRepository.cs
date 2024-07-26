using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _db;
        public LocalImageRepository(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor, ApplicationDbContext db)
        {
            _db = db;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Image> UploadAsync(Image image)
        {
            var localFilePath = Path.Combine(_environment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");
            // Upload image to localpath
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // https://localhost:1234/image/image.jpeg
            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            // Add Image to the Images table
            await _db.Images.AddAsync(image);
            await _db.SaveChangesAsync();
            return image;
        }
    }
}
