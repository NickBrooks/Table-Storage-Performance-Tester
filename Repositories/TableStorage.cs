using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TableStoragePerformanceTester
{


    public class TableStorage
    {
        private static CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
        private static CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
        private static readonly string entityTable = "entities";

        public static async Task<int> GetCountOfEntitiesInPartition(string partitionKey)
        {
            CloudTable table = tableClient.GetTableReference(entityTable);

            TableQuery<DynamicTableEntity> tableQuery = new TableQuery<DynamicTableEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey)).Select(new string[] { "PartitionKey" });

            EntityResolver<string> resolver = (pk, rk, ts, props, etag) => null;

            var count = 0;
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<string> tableQueryResult =
                    await table.ExecuteQuerySegmentedAsync(tableQuery, resolver, continuationToken);

                continuationToken = tableQueryResult.ContinuationToken;

                count = count + tableQueryResult.Results.Count;
            } while (continuationToken != null);

            return count;
        }

        public static async Task<bool> BatchOfBatchInserts(string partitionKey, int count)
        {
            for (int i = 0; i < count; i++)
            {
                await BatchInsert(partitionKey);
            }

            return true;
        }

        private static async Task<bool> BatchInsert(string partitionKey)
        {
            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference(entityTable);

            // Create the batch operation.
            TableBatchOperation batchOperation = new TableBatchOperation();

            // Add both customer entities to the batch insert operation.
            Repeat(100, () => batchOperation.InsertOrReplace(new FakeUser(partitionKey)));

            // Execute the batch operation.
            var result = await table.ExecuteBatchAsync(batchOperation);

            return result != null;
        }

        private static void Repeat(int count, Action action)
        {
            for (int i = 0; i < count; i++)
            {
                action();
            }
        }
    }
}
