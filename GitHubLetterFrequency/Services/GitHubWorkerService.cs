using GitHubLetterFrequency.Configuration;
using GitHubLetterFrequency.Handler;
using Microsoft.VisualBasic;
using Octokit;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubLetterFrequency.Services
{
    public class GitHubWorkerService : IGitHubWorkerService
    {
        private readonly GitHubClient _githubClient;
        private readonly IGitHubHandler _gitHubHandler;

        public GitHubWorkerService(GitHubClient githubClient, IGitHubHandler gitHubHandler)
        {
            _githubClient = githubClient;
            _gitHubHandler = gitHubHandler;
        }
        public async Task Run()
        {
            try
            {
                var letterFrequency = new ConcurrentDictionary<char, int>();
                var contents = await _githubClient.Repository.Content.GetAllContents(GitHubConfigSettings.Owner, GitHubConfigSettings.Repo);
                await _gitHubHandler.ProcessDirectoryAsync(GitHubConfigSettings.Owner, GitHubConfigSettings.Repo, contents, letterFrequency);
                var sortedFrequency = letterFrequency.OrderByDescending(kvp => kvp.Value);

                foreach (var kvp in sortedFrequency)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }
    }
}
