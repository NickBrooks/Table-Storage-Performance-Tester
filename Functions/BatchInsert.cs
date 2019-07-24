
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TableStoragePerformanceTester
{
    public static class BatchInsert
    {
        [FunctionName("BatchInsert")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            string partitionKey = req.Query["partitionKey"];
            if (partitionKey == null)
                return new BadRequestObjectResult("Add a partition key.");

            int count = int.Parse(req.Query["count"]);

            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            await TableStorage.BatchOfBatchInserts(partitionKey, count);

            // Stop timing.
            stopwatch.Stop();

            var total = $"{stopwatch.ElapsedMilliseconds} ms";
            var average = $"{Math.Round(Convert.ToDouble(stopwatch.ElapsedMilliseconds / count), 0)} ms";

            return new OkObjectResult(new { total, average });
        }
    }
}
