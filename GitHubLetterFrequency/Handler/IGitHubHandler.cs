

using Octokit;
using System.Collections.Concurrent;

namespace GitHubLetterFrequency.Handler
{
    public interface IGitHubHandler
    {
        Task ProcessDirectoryAsync(string owner, string repo, IReadOnlyList<RepositoryContent> contents, ConcurrentDictionary<char, int> letterFrequency);
    }
}
