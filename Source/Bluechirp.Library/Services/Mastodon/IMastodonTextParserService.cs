using Bluechirp.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bluechirp.Library.Services.Mastodon;

/// <summary>
/// Defines a service interface for parsing HTML Mastodon content
/// into an abstract format.
/// </summary>
public interface IMastodonTextParserService
{
    /// <summary>
    /// Parses a toot HTML string into an abstract format asynchronously.
    /// </summary>
    /// <param name="htmlContent">The HTML string.</param>
    /// <returns>A list of <see cref="MastodonContent"/> containing the parsed toot.</returns>
    public Task<List<MastodonContent>> ParseHtmlAsync(string htmlContent);
}
