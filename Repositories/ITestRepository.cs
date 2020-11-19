using ExperimentCounter.Models;
using System.Threading.Tasks;

namespace ExperimentCounter.Repositories
{
    public interface ITestRepository
    {
        /// <summary>
        /// Get tests from remote service.
        /// </summary>
        /// <param name="seed">Sets the random number seed so you get the same results every time.</param>
        /// <param name="count">Sets the number of arrays to return.</param>
        /// <param name="length">Sets the length of the arrays returned.</param>
        /// <returns>Raw test data.</returns>
        Task<Tests> GetTests(string seed, int count, int length);
    }
}