namespace Everywhere.I18N;

public partial class LocaleManager
{
    public bool TryGetResource(object key, string? cultureName, out object? resource)
    {
        // Simple implementation for compatibility
        // This should be replaced with the actual implementation from the source generator
        resource = key?.ToString();
        return resource != null;
    }

    public static partial LocaleManager Shared { get; }
}
