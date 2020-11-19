using ExperimentCounter.Models;
using ExperimentCounter.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExperimentCounter.Services
{
    public class TestService : ITestService
    {
        private readonly ITestRepository testRepository;
        private readonly IConfiguration config;
        public TestService(ITestRepository testRepository, IConfiguration config)
        {
            this.testRepository = testRepository;
            this.config = config; 
        }

        public async Task<string> GetTestCounts(string seed, int? count, int? length)
        {
            if (string.IsNullOrEmpty(seed))
                seed = Guid.NewGuid().ToString();
            Tests tests = await this.testRepository.GetTests(seed, count ?? config.GetValue<int>("Data:DefaultCount"), length ?? config.GetValue<int>("Data:DefaultLength"));
            return GetTestCounts(tests);
        }

        public string GetTestCounts(Tests tests)
        {
            if (tests == null || tests.Data == null)
                throw new ArgumentNullException("Tests.Data");
            return GetOutput(CountTests(tests));
        }

        private string GetOutput(TestResult[] results)
        {
            string output = "";
            for (int i = 0; i < results.Length - 1; i++)
            {
                output += $"{results[i].Name} -> {results[i].Count}\n";
            }
            return output += $"{results[^1].Name} -> {results[^1].Count}";
        }

        private TestResult[] CountTests(Tests tests)
        {
            List<TestResult> results = new List<TestResult>();

            foreach (var test in tests.Data)
            {
                int[] validTest = this.GetValidTests(test);
                if (validTest.Length > 0)
                    AddTestToResults(results, validTest);
            }
            return results.ToArray();
        }

        private void AddTestToResults(List<TestResult> results, int[] validTest)
        {
            if (!results.Any(r => r.Test.OrderBy(t => t).SequenceEqual(validTest.OrderBy(t => t))))
            {
                results.Add(this.GetTestResult(validTest));
            }
            else
            {
                var result = results.Where(r => r.Test.OrderBy(t => t).SequenceEqual(validTest.OrderBy(t => t))).FirstOrDefault();
                result.Count++;
            }
        }

        private TestResult GetTestResult(int[] validTest)
        {
            return new TestResult
            {
                Count = 1,
                Name = this.GetName(validTest),
                Test = validTest,
            };
        }

        private string GetName(int[] test)
        {
            string name = $"{test[0]}";
            for (int i = 1; i < test.Length - 1; i++)
            {
                name += $", {test[i]}";
            }
            return test.Length > 1 ? name + $" and {test[^1]}" : name;
        }

        private int[] GetValidTests(List<object> test)
        {
            List<int> results = new List<int>();
            foreach (var data in test)
            {
                if (data != null && int.TryParse(data.ToString(), out int dataInt))
                   results.Add(dataInt);
            }
            return results.ToArray();
        }
    }
}
