using Bookly.Api.Extensions;
using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UploadController(IFileService fileService) : ApiControllerBase
{
    private const int MaxFileSizeMb = 5;
    private const long MaxFileSize = MaxFileSizeMb * 1024 * 1024;
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png"];

    /// <summary>
    /// Uploads an image file (.jpg, .jpeg, .png, max 5 MB).
    /// </summary>
    [HttpPost]
    [RequestSizeLimit(6 * 1024 * 1024)]
    public async Task<IActionResult> UploadFile(IFormFile? file, CancellationToken cancellationToken)
    {
        var validation = ValidateFile(file);
        if (validation.IsFailure)
            return validation.ToResponse();

        await using var stream = file!.OpenReadStream();

        var filePath = await fileService.UploadFileAsync(
            stream, file.FileName, "uploads", cancellationToken);

        return Ok(new { FilePath = filePath });
    }

    private static Result ValidateFile(IFormFile? file)
    {
        if (file is null || file.Length == 0)
            return Result.Failure(FileErrors.Required());

        if (file.Length > MaxFileSize)
            return Result.Failure(FileErrors.TooLarge(MaxFileSizeMb));

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
            return Result.Failure(FileErrors.InvalidType(AllowedExtensions));

        return Result.Success();
    }
}