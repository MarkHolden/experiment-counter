using ExperimentCounter.Models;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExperimentCounter.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly HttpClient httpClient;
        public TestRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Tests> GetTests(string seed, int count, int length)
        {
            AsyncRetryPolicy retryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, timeSpan) => { seed = Guid.NewGuid().ToString(); });

            HttpResponseMessage result = null;
            await retryPolicy.ExecuteAsync(async () =>
            {
                result = await httpClient.GetAsync($"?seed={seed}&count={count}&length={length}");
                result.EnsureSuccessStatusCode();
            });

            return JsonConvert.DeserializeObject<Tests>(await result?.Content.ReadAsStringAsync());
        }
    }
}
