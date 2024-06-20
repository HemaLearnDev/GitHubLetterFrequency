# GitHubLetterFrequency

GitHubLetterFrequency is a .NET Core console application designed to analyze the letter frequency in the files of a specified GitHub repository. This tool uses the Octokit library to interact with the GitHub API and fetch repository contents.

## Features

- Fetches contents from a specified GitHub repository.
- Analyzes letter frequency in `.js` and `.ts` files.
- Outputs a sorted list of letter frequencies.

## Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) (version 5.0 or later)
- [Git](https://git-scm.com/)
- A GitHub Personal Access Token with appropriate permissions

## Setup

1. **Clone the repository:**

   ```sh
   git clone https://github.com/HemaLearnDev/GitHubLetterFrequency.git
   cd GitHubLetterFrequency

2. **Setup environment variables:**

Ensure you have a GitHub Personal Access Token stored in your environment variables. This token should be configured as My_GITHUB_TOKEN.

On Windows, you can set it using the Command Prompt or PowerShell:
setx GITHUB_TOKEN "your_personal_access_token"

