using Bookly.Application.Common.Results;

public static class FileErrors
{
    public static Error Required() =>
        Errors.Validation("File.Required", "File is required.");

    public static Error TooLarge(int maxMegabytes) =>
        Errors.Validation(
            "File.TooLarge",
            "File size exceeds the maximum limit of {0} MB.",
            maxMegabytes);

    public static Error InvalidType(IEnumerable<string> allowedExtensions) =>
        Errors.Validation(
            "File.InvalidType",
            "Invalid file type. Only {0} files are allowed.",
            string.Join(", ", allowedExtensions));
}