using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace TableStoragePerformanceTester
{
    public class FakeUser : TableEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string AnotherString { get; set; }

        public FakeUser(string _partitionKey)
        {
            PartitionKey = _partitionKey;
            RowKey = Guid.NewGuid().ToString();
            Name = Guid.NewGuid().ToString();
            AnotherString = Guid.NewGuid().ToString();

            Random rnd = new Random();
            Age = rnd.Next(100);
        }

        public FakeUser() { }
    }
}
