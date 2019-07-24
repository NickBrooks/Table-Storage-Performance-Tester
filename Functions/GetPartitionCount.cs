using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TableStoragePerformanceTester
{
    public static class GetPartitionCount
    {
        [FunctionName("GetPartitionCount")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            string partitionKey = req.Query["partitionKey"];
            if (partitionKey == null)
                return new BadRequestObjectResult("Add a partition key.");

            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            int count = await TableStorage.GetCountOfEntitiesInPartition(partitionKey);

            // Stop timing.
            stopwatch.Stop();

            return new OkObjectResult(new { count, elapsed = $"{stopwatch.ElapsedMilliseconds} ms" });
        }
    }
}
