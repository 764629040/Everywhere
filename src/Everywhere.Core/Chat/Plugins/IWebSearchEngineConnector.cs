using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Everywhere.Chat.Plugins;

/// <summary>
/// Interface for web search engine connectors.
/// </summary>
public interface IWebSearchEngineConnector
{
    /// <summary>
    /// Performs a web search with the specified query.
    /// </summary>
    /// <typeparam name="T">The type of result to return (string or WebPage).</typeparam>
    /// <param name="query">The search query.</param>
    /// <param name="count">The number of results to return.</param>
    /// <param name="offset">The offset for pagination.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A collection of search results.</returns>
    Task<IEnumerable<T>> SearchAsync<T>(string query, int count, int offset, CancellationToken cancellationToken);
}

/// <summary>
/// Represents a web page result from a search.
/// </summary>
public class WebPage
{
    /// <summary>
    /// Gets or sets the title of the web page.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL of the web page.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the snippet/description of the web page.
    /// </summary>
    public string Snippet { get; set; } = string.Empty;
}
