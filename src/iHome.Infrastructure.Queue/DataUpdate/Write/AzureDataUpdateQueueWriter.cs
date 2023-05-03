using Azure.Storage.Queues;
using iHome.Infrastructure.Queue.Models;
using System.Text.Json;

namespace iHome.Infrastructure.Queue.DataUpdate.Write
{
    internal class AzureDataUpdateQueueWriter : IDataUpdateQueueWriter
    {
        private readonly QueueClient _client;
        private const string QueueName = "device-data-events";

        public AzureDataUpdateQueueWriter(string connectionString)
        {
            _client = new QueueClient(connectionString, QueueName);
        }

        public Task Push(DataUpdateModel value)
        {
            return _client.SendMessageAsync(JsonSerializer.Serialize(value));
        }
    }
}
