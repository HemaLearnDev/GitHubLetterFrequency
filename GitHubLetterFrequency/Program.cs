using GitHubLetterFrequency;
using GitHubLetterFrequency.Configuration;
using GitHubLetterFrequency.Handler;
using GitHubLetterFrequency.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Octokit;

var host = CreateHostBuilder(args).Build();
var service = host.Services.GetRequiredService<IGitHubWorkerService>();
await service.Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<GitHubClient>(provider =>
                    {
                        var config = provider.GetRequiredService<GitHubConfigSettings>();
                        if (string.IsNullOrEmpty(config.GitHubToken))
                        {
                            throw new InvalidOperationException("GitHub token not found in configuration.");
                        }
                        return new GitHubClient(new ProductHeaderValue("GitHubLetterFrequencyAnalyzer"))
                        {
                            Credentials = new Credentials(config.GitHubToken)
                        };
                    });

                    services.AddSingleton<IGitHubHandler, GitHubHandler> ();
                    services.AddTransient<IGitHubWorkerService, GitHubWorkerService>();
                    services.AddSingleton<GitHubConfigSettings>();
                });
