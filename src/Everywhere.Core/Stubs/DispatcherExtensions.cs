using Avalonia.Threading;

namespace Everywhere.Extensions;

/// <summary>
/// Stub extension methods for Avalonia Dispatcher that were previously provided by ShadUI.
/// These provide synchronous and asynchronous invoke functionality.
/// </summary>
public static class DispatcherExtensions
{
    /// <summary>
    /// Invokes an action on the dispatcher thread and waits for completion.
    /// </summary>
    public static void InvokeOnDemand(this Dispatcher dispatcher, Action action)
    {
        if (dispatcher.CheckAccess())
        {
            action();
        }
        else
        {
            dispatcher.Invoke(action);
        }
    }

    /// <summary>
    /// Invokes a function on the dispatcher thread and returns the result.
    /// </summary>
    public static T InvokeOnDemand<T>(this Dispatcher dispatcher, Func<T> func)
    {
        if (dispatcher.CheckAccess())
        {
            return func();
        }
        else
        {
            return dispatcher.Invoke(func);
        }
    }

    /// <summary>
    /// Invokes an asynchronous function on the dispatcher thread and returns the result.
    /// </summary>
    public static async Task<T> InvokeOnDemandAsync<T>(this Dispatcher dispatcher, Func<Task<T>> func)
    {
        if (dispatcher.CheckAccess())
        {
            return await func();
        }
        else
        {
            return await dispatcher.InvokeAsync(func);
        }
    }

    /// <summary>
    /// Invokes an asynchronous action on the dispatcher thread.
    /// </summary>
    public static async Task InvokeOnDemandAsync(this Dispatcher dispatcher, Func<Task> func)
    {
        if (dispatcher.CheckAccess())
        {
            await func();
        }
        else
        {
            await dispatcher.InvokeAsync(func);
        }
    }
}
