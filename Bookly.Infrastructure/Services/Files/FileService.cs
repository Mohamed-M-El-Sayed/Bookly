using Bookly.Application.Contracts.Services.Files;

namespace Bookly.Infrastructure.Services.Files;

public class FileService(string basePath) : IFileService
{
    public async Task<string> UploadFileAsync(Stream file,string fileName ,string folderName, CancellationToken cancellationToken)
    {
       var folderPath =  Path.Combine(basePath, folderName);
       if(!Directory.Exists(folderPath))
           Directory.CreateDirectory(folderPath);
       var uniqueName = $"{Guid.NewGuid()}_{fileName}";
       var filePath = Path.Combine(folderPath, uniqueName);
       using var fileStream = new FileStream(filePath, FileMode.Create);
       await file.CopyToAsync(fileStream, cancellationToken);
       return Path.Combine(folderName, uniqueName).Replace("\\", "/");
    }
}
