# Azure Table Storage Performance Tester :cloud_with_lightning:

Using [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/) to run some simple tests on [Azure Table Storage](https://docs.microsoft.com/en-us/azure/cosmos-db/table-storage-how-to-use-dotnet).

- [Getting started :runner:](#getting-started)
- [Run locally :houses:](#run-locally)
- [Batch insert into partition :incoming_envelope:](#batch-insert-into-partition)
- [Count entities in a partition :1234:](#count-entities-in-a-partition)

## Deploy to Azure

1. In the Azure Portal
2. Create a new Table Storage resource.
3. Create a new Azure Function resource. For the sake of this README, the app name will be 'xyz'. Name it what you like and replace 'xyz' with your app name in the HTTP requests below.
4. Get the access key from Table Storage, create a new AppSetting in your Functions App named `TableStorageConnection`. The value of this setting will be the key you got from Table Storage.
5. Deploy the Functions app from this repo or open in Visual Studio and publish from there.

## Run locally

> :information_desk_person: Note that this is not reflective of real world conditions.

To run locally, copy `local.settings.example.json` and rename it to `local.settings.json` and add your Table Storage Key (or use development storage). Run the project!

## Batch insert into partition

Azure can [batch insert](https://docs.microsoft.com/en-us/azure/cosmos-db/table-storage-how-to-use-dotnet#insert-a-batch-of-entities) up to 100 entities at a time.

To batch insert a bunch of entities, run the following HTTP request, making sure to set `your_partition_key`. `count` refers to how many batches of 100 entities you'd like to insert. For this, we've set it to 1000 which equates to 100,000 entities.

```
GET
https://tstester.azurewebsites.net/api/batchinsert?partitionKey=your_partition_key&count=1000
```

#### My average batch insert results running in West US 2 data center:

- 100 items: **115ms**
- 1000 items: **1096ms (109ms per 100)**
- 100,000 items: **98985ms (98ms per 100)**

## Count entities in a partition

```
GET
https://xyz.azurewebsites.net/api/getPartitionCount?partitionKey=your_partition_key
```

#### My average count total partition entities (100,000) running in West US 2 data center:

- 100,000 items: **6400ms**
