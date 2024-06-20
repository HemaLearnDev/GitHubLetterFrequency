

using Octokit;
using System.Collections.Concurrent;

namespace GitHubLetterFrequency.Handler
{
    public class GitHubHandler: IGitHubHandler
    {
        private readonly GitHubClient _githubClient;

        public GitHubHandler(GitHubClient githubClient)
        {
            _githubClient = githubClient;

        }
        public async Task ProcessDirectoryAsync(string owner, string repo, IReadOnlyList<RepositoryContent> contents, ConcurrentDictionary<char, int> letterFrequency)
        {
            var tasks = new List<Task>();

            foreach (var content in contents)
            {
                try
                {
                    if (content.Type == ContentType.Dir)
                    {
                        var subContents = await _githubClient.Repository.Content.GetAllContents(owner, repo, content.Path);
                        tasks.Add(ProcessDirectoryAsync(owner, repo, subContents, letterFrequency));
                    }
                    else if (content.Type == ContentType.File && (content.Name.EndsWith(".js") || content.Name.EndsWith(".ts")))
                    {
                        tasks.Add(ProcessFileAsync(owner, repo, content.Path, letterFrequency));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing {content.Path}: {ex.Message}");
                }
            }

            await Task.WhenAll(tasks);
        }

        private async Task ProcessFileAsync(string owner, string repo, string path, ConcurrentDictionary<char, int> letterFrequency)
        {
            try
            {
                var fileContent = await _githubClient.Repository.Content.GetAllContents(owner, repo, path);
                foreach (var file in fileContent)
                {
                    var text = await GetFileContentAsync(file.DownloadUrl);
                    CountLetters(text, letterFrequency);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file {path}: {ex.Message}");
            }
        }

        private async Task<string> GetFileContentAsync(string url)
        {
            try
            {
                using var httpClient = new HttpClient();
                return await httpClient.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching file from {url}: {ex.Message}");
                return string.Empty;
            }
        }

        private void CountLetters(string text, ConcurrentDictionary<char, int> letterFrequency)
        {
            foreach (var ch in text)
            {
                if (char.IsLetter(ch))
                {
                    var lowerChar = char.ToLower(ch);
                    letterFrequency.AddOrUpdate(lowerChar, 1, (key, oldValue) => oldValue + 1);
                }
            }
        }
    }
}
