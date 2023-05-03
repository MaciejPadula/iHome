using Azure.Storage.Queues;
using iHome.Infrastructure.Queue.Models;
using System.Text.Json;

namespace iHome.Infrastructure.Queue.DataUpdate.Read
{
    internal class AzureDataQueueReader : IDataUpdateQueueReader
    {
        private readonly QueueClient _client;
        private const string QueueName = "device-data-events";

        public AzureDataQueueReader(string connectionString)
        {
            _client = new QueueClient(connectionString, QueueName);
        }

        public async Task<DataUpdateModel?> Peek()
        {
            var response = await _client.PeekMessageAsync();
            return response?.Value?.Body?.ToObjectFromJson<DataUpdateModel>();
        }

        public async Task<DataUpdateModel?> Pop()
        {
            var response = await _client.ReceiveMessageAsync();
            return response?.Value?.Body?.ToObjectFromJson<DataUpdateModel>();
        }
    }
}
