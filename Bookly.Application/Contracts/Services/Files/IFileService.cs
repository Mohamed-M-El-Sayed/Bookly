namespace Bookly.Application.Contracts.Services.Files;

public interface IFileService
{
    Task<string> UploadFileAsync(Stream file, string fileName, string folderName, CancellationToken cancellationToken);
    Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken);
}
