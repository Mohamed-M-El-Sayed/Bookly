namespace Bookly.Application.Common.Mappings;

public static class MappingExtensions
{
    private static string? _appUrl;
    public static void Configure(string appUrl)
    {
        _appUrl = appUrl;
    }
    public static string? ToFullImageUrl(this string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return imageUrl;

        if (Uri.TryCreate(imageUrl, UriKind.Absolute, out _))
            return imageUrl;

        return $"{_appUrl?.TrimEnd('/')}/{imageUrl.TrimStart('/')}";
    }
}
