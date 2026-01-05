using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.Input;
using Everywhere.Chat.Plugins;
using Everywhere.Configuration;
using Everywhere.Views;
using ShadUI;

namespace Everywhere.ViewModels;

public partial class ChatPluginPageViewModel(IChatPluginManager manager) : BusyViewModelBase
{
    public IChatPluginManager Manager => manager;

    public ChatPlugin? SelectedPlugin
    {
        get;
        set
        {
            if (!SetProperty(ref field, value)) return;
            OnPropertyChanged(nameof(SelectedBuiltInPlugin));
            OnPropertyChanged(nameof(SelectedMcpPlugin));

            ContentTabItems.Clear();
            if (value is null) return;

            if (value.SettingsItems is { Count: > 0 } settingsItems)
            {
                ContentTabItems.Add(new SettingsTabItem(settingsItems));
            }

            ContentTabItems.Add(new FunctionsTabItem(value));

            if (value is McpChatPlugin mcpPlugin)
            {
                ContentTabItems.Add(new LogsTabItem(mcpPlugin.LogEntries));
            }
        }
    }

    /// <summary>
    /// Helper property to get the selected plugin as a BuiltInChatPlugin.
    /// </summary>
    public BuiltInChatPlugin? SelectedBuiltInPlugin
    {
        get => SelectedPlugin as BuiltInChatPlugin;
        set
        {
            if (value is not null)
            {
                SelectedPlugin = value;
            }
        }
    }

    /// <summary>
    /// Helper property to get the selected plugin as a McpChatPlugin.
    /// </summary>
    public McpChatPlugin? SelectedMcpPlugin
    {
        get => SelectedPlugin as McpChatPlugin;
        set
        {
            if (value is not null)
            {
                SelectedPlugin = value;
            }
        }
    }

    public ObservableCollection<IContentTabItem> ContentTabItems { get; } = [];

    [RelayCommand]
    private async Task AddMcpPluginAsync()
    {
        var form = new McpTransportConfigurationForm();
        // Temporarily simplified - using Show() instead of ShowAsync()
        DialogManager
            .CreateDialog(LocaleResolver.ChatPluginPageViewModel_AddMcpPlugin_DialogTitle, "")
            .Show();
        
        // Auto-add for now
        try
        {
            if (form.Configuration.Validate())
            {
                SelectedPlugin = manager.CreateMcpPlugin(form.Configuration);
            }
        }
        catch (Exception e)
        {
            ToastExceptionHandler.HandleException(e, "Failed to add MCP Plugin");
        }
    }

    [RelayCommand(CanExecute = nameof(IsNotBusy))]
    private Task StartMcpPluginAsync(McpChatPlugin? plugin, CancellationToken cancellationToken)
    {
        if (plugin is null) return Task.CompletedTask;

        return ExecuteBusyTaskAsync(
            token => Task.Run(() => manager.StartMcpClientAsync(plugin, token), token),
            ToastExceptionHandler,
            cancellationToken: cancellationToken);
    }

    [RelayCommand(CanExecute = nameof(IsNotBusy))]
    private Task StopMcpPluginAsync(McpChatPlugin? plugin, CancellationToken cancellationToken)
    {
        if (plugin is null) return Task.CompletedTask;

        return ExecuteBusyTaskAsync(
            token => Task.Run(() => manager.StopMcpClientAsync(plugin), token),
            ToastExceptionHandler,
            cancellationToken: cancellationToken);
    }

    [RelayCommand(CanExecute = nameof(IsNotBusy))]
    private Task EditMcpPluginAsync(McpChatPlugin? plugin, CancellationToken cancellationToken)
    {
        if (plugin is null) return Task.CompletedTask;

        return ExecuteBusyTaskAsync(
            async token =>
            {
                var form = new McpTransportConfigurationForm
                {
                    Configuration = JsonSerializer.Deserialize(
                        JsonSerializer.Serialize(
                            plugin.TransportConfiguration,
                            McpTransportConfigurationJsonSerializerContext.Default.McpTransportConfiguration),
                        McpTransportConfigurationJsonSerializerContext.Default.McpTransportConfiguration) ?? new StdioMcpTransportConfiguration()
                };
                // Temporarily simplified
                DialogManager
                    .CreateDialog(LocaleResolver.ChatPluginPageViewModel_EditMcpPlugin_DialogTitle, "")
                    .Show();
                
                if (form.Configuration.Validate())
                {
                    await Task.Run(() => manager.UpdateMcpPluginAsync(plugin, form.Configuration), token);
                }
            },
            ToastExceptionHandler,
            cancellationToken: cancellationToken);
    }

    [RelayCommand(CanExecute = nameof(IsNotBusy))]
    private Task RemoveMcpPluginAsync(McpChatPlugin? plugin, CancellationToken cancellationToken)
    {
        if (plugin is null) return Task.CompletedTask;

        return ExecuteBusyTaskAsync(
            async token =>
            {
                // Temporarily simplified
                DialogManager
                    .CreateDialog(LocaleResolver.ChatPluginPageViewModel_RemoveMcpPlugin_ConfirmationMessage.Format(plugin.HeaderKey), "")
                    .WithPrimaryButton(LocaleResolver.Common_Yes, async () =>
                    {
                        await Task.Run(() => manager.RemoveMcpPluginAsync(plugin), token);
                        if (SelectedPlugin == plugin) SelectedPlugin = null;
                    })
                    .WithCancelButton(LocaleResolver.Common_No)
                    .Show();
            },
            ToastExceptionHandler,
            cancellationToken: cancellationToken);
    }

    [RelayCommand]
    private async Task CopyLogsAsync(McpChatPlugin? plugin)
    {
        if (plugin is null) return;

        await Clipboard.SetTextAsync(string.Join('\n', plugin.LogEntries));
        ToastManager
            .CreateToast("Logs copied to clipboard.")
            .OnBottomRight()
            .ShowSuccess();
    }

    protected override void OnIsBusyChanged()
    {
        base.OnIsBusyChanged();
        StartMcpPluginCommand.NotifyCanExecuteChanged();
        StopMcpPluginCommand.NotifyCanExecuteChanged();
        EditMcpPluginCommand.NotifyCanExecuteChanged();
        RemoveMcpPluginCommand.NotifyCanExecuteChanged();
    }

    #region ContentTabItems

    // Helpers for content tab items in MVVM pattern

    public interface IContentTabItem
    {
        DynamicResourceKeyBase Header { get; }
    }

    public class SettingsTabItem(IReadOnlyList<SettingsItem> settingsItems) : IContentTabItem
    {
        public DynamicResourceKeyBase Header => new DynamicResourceKey(LocaleKey.ChatPluginPage_TabItem_Settings_Header);

        public IReadOnlyList<SettingsItem> SettingsItems { get; } = settingsItems;
    }

    public class FunctionsTabItem(ChatPlugin plugin) : IContentTabItem
    {
        public DynamicResourceKeyBase Header => new DynamicResourceKey(LocaleKey.ChatPluginPage_TabItem_Functions_Header);

        public ChatPlugin Plugin { get; } = plugin;
    }

    public class LogsTabItem(ReadOnlyObservableCollection<McpChatPlugin.LogEntry> logEntries) : IContentTabItem
    {
        public DynamicResourceKeyBase Header => new DynamicResourceKey(LocaleKey.ChatPluginPage_TabItem_Logs_Header);

        public ReadOnlyObservableCollection<McpChatPlugin.LogEntry> LogEntries { get; } = logEntries;
    }

    #endregion
}