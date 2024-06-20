
namespace GitHubLetterFrequency.Configuration
{
    public class GitHubConfigSettings
    {
        public static string Owner { get; } = "lodash";
        public static string Repo { get; } = "lodash";
        public string GitHubToken { get; }
        public GitHubConfigSettings()
        {
            GitHubToken = Environment.GetEnvironmentVariable("My_GITHUB_TOKEN");// Read token from environment variable
            if (string.IsNullOrEmpty(GitHubToken))
            {
                throw new InvalidOperationException("GitHub token not found in environment variables.");
            }
        }
    }
}
