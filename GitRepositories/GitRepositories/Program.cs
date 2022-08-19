using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GitRepositories
{
    public class Program
    {

        private static HttpClient client = new HttpClient();

        public readonly record struct Repository
        {
            [JsonPropertyName("name")]
            public string Name { get; init; }
            [JsonPropertyName("description")]
            public string Description { get; init; }
            [JsonPropertyName("html_url")]
            public Uri GitHubHomeUrl { get; init; }
            [JsonPropertyName("homepage")]
            public Uri Homepage { get; init; }
            [JsonPropertyName("watchers")]
            public int Watchers { get; init; }
            [JsonPropertyName("pushed_at")]
            public DateTime LastPushUtc { get; init; }
            public DateTime LastPush => LastPushUtc.ToLocalTime();
        }

        private static async Task<List<Repository>> processGitRepositories(string gitUrl)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("pplication/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            return await JsonSerializer.DeserializeAsync<List<Repository>>(await client.GetStreamAsync(gitUrl)); 
        }
        static async Task Main(string[] args)
        {
            var repos = await processGitRepositories("https://api.github.com/orgs/dotnet/repos");

            foreach (var repo in repos)
            {
                Console.WriteLine(repo.Name);
                Console.WriteLine(repo.Description);
                Console.WriteLine(repo.GitHubHomeUrl);
                Console.WriteLine(repo.Homepage);
                Console.WriteLine(repo.Watchers);
                Console.WriteLine(repo.LastPush);
                Console.WriteLine();
            }
        }


    }
}