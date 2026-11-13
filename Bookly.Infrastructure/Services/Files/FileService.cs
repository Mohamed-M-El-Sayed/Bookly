using Bookly.Application.Contracts.Services.Files;

namespace Bookly.Infrastructure.Services.Files;

public class FileService(string basePath) : IFileService
{
    public async Task<string> UploadFileAsync(Stream file, string fileName, string folderName, CancellationToken cancellationToken)
    {
        var folderPath = Path.Combine(basePath, folderName);
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        var uniqueName = $"{Guid.NewGuid()}_{fileName}";
        var filePath = Path.Combine(folderPath, uniqueName);
        using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream, cancellationToken);
        return Path.Combine(folderName, uniqueName).Replace("\\", "/");
    }
    public Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
            return Task.CompletedTask;

        cancellationToken.ThrowIfCancellationRequested();
        var filePath = Path.GetFullPath(
            Path.Combine(basePath, fileUrl));

        var rootPath = Path.GetFullPath(basePath)
            .TrimEnd(Path.DirectorySeparatorChar)
            + Path.DirectorySeparatorChar;
        if (!filePath.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Invalid file path.");
        if (File.Exists(filePath))
            File.Delete(filePath);
        return Task.CompletedTask;
    }
}
