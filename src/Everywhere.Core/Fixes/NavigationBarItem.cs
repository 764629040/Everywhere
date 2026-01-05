using Avalonia;
using Avalonia.Controls;

namespace ShadUI;

/// <summary>
/// Represents a navigation bar item.
/// This is a simple data class for navigation purposes.
/// </summary>
public partial class NavigationBarItem : AvaloniaObject
{
    /// <summary>
    /// Defines the <see cref="Content"/> property.
    /// </summary>
    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<NavigationBarItem, object?>(nameof(Content));

    /// <summary>
    /// Defines the <see cref="Route"/> property.
    /// </summary>
    public static readonly StyledProperty<object?> RouteProperty =
        AvaloniaProperty.Register<NavigationBarItem, object?>(nameof(Route));

    /// <summary>
    /// Defines the <see cref="Icon"/> property.
    /// </summary>
    public static readonly StyledProperty<Control?> IconProperty =
        AvaloniaProperty.Register<NavigationBarItem, Control?>(nameof(Icon));

    /// <summary>
    /// Defines the <see cref="ToolTip"/> property.
    /// </summary>
    public static readonly StyledProperty<object?> ToolTipProperty =
        AvaloniaProperty.Register<NavigationBarItem, object?>(nameof(ToolTip));

    /// <summary>
    /// Gets or sets the content of the navigation bar item.
    /// </summary>
    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the route associated with this navigation item.
    /// </summary>
    public object? Route
    {
        get => GetValue(RouteProperty);
        set => SetValue(RouteProperty, value);
    }

    /// <summary>
    /// Gets or sets the icon for the navigation item.
    /// </summary>
    public Control? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets or sets the tooltip for the navigation item.
    /// </summary>
    public object? ToolTip
    {
        get => GetValue(ToolTipProperty);
        set => SetValue(ToolTipProperty, value);
    }
}
