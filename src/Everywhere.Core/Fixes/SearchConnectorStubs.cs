using Everywhere.Chat.Plugins;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Everywhere.Chat.Plugins;

/// <summary>
/// Stub implementation of GoogleConnector for compatibility.
/// Google Connectors are temporarily disabled due to missing dependencies.
/// </summary>
public class GoogleConnector : IWebSearchEngineConnector
{
    public GoogleConnector(string apiKey, string searchEngineId, HttpClient httpClient, ILoggerFactory loggerFactory, Uri? customEndpoint = null)
    {
        throw new NotSupportedException("Google Connector is temporarily disabled due to missing dependencies.");
    }
    
    public Task<IEnumerable<T>> SearchAsync<T>(string query, int count, int offset, CancellationToken cancellationToken)
    {
        throw new NotSupportedException("Google Connector is temporarily disabled due to missing dependencies.");
    }
}

/// <summary>
/// Stub implementation of BraveConnector for compatibility.
/// Brave Connectors are temporarily disabled due to missing dependencies.
/// </summary>
public class BraveConnector : IWebSearchEngineConnector
{
    public BraveConnector(string apiKey, HttpClient httpClient, ILoggerFactory loggerFactory, Uri? customEndpoint = null)
    {
        throw new NotSupportedException("Brave Connector is temporarily disabled due to missing dependencies.");
    }
    
    public Task<IEnumerable<T>> SearchAsync<T>(string query, int count, int offset, CancellationToken cancellationToken)
    {
        throw new NotSupportedException("Brave Connector is temporarily disabled due to missing dependencies.");
    }
}
