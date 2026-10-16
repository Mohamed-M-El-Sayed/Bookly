using Bookly.Application.Common.Results;

public static class FileErrors
{
    public static Error Required() =>
        ErrorFactory.Validation("File.Required", "File is required.");

    public static Error TooLarge(int maxMegabytes) =>
        ErrorFactory.Validation(
            "File.TooLarge",
            "File size exceeds the maximum limit of {0} MB.",
            maxMegabytes);

    public static Error InvalidType(IEnumerable<string> allowedExtensions) =>
        ErrorFactory.Validation(
            "File.InvalidType",
            "Invalid file type. Only {0} files are allowed.",
            string.Join(", ", allowedExtensions));
}