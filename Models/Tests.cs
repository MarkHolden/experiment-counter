using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExperimentCounter.Models
{
    public class Tests
    {
        [JsonProperty("data")]
        public List<List<object>> Data { get; set; }
    }
}