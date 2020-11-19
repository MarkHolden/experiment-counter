using ExperimentCounter.Models;
using System.Threading.Tasks;

namespace ExperimentCounter.Services
{
    public interface ITestService
    {
        /// <summary>
        /// Submit tests and return processed results.
        /// </summary>
        /// <param name="tests">Experiment data.</param>
        /// <returns>Processed results of Experiments.</returns>
        string GetTestCounts(Tests tests);

        /// <summary>
        /// Get tests from remote service and processes results.
        /// </summary>
        /// <param name="seed">Sets the random number seed so you get the same results every time.</param>
        /// <param name="count">Sets the number of arrays to return.</param>
        /// <param name="length">Sets the length of the arrays returned.</param>
        /// <returns>Processed results of Experiments.</returns>
        Task<string> GetTestCounts(string seed, int? count, int? length);
    }
}