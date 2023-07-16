﻿using iHome.Infrastructure.Firestore.Serializers;
using iHome.Microservices.Devices.Infrastructure.Logic;

namespace iHome.Microservices.Devices.Infrastructure.Repositories;

public class FirestoreDeviceDataRepository : IDeviceDataRepository
{
    private readonly IFirestoreConnectionFactory _connectionFactory;
    private readonly IMessageSerializer _messageSerializer;

    private const string CollectionPath = "devices";

    public FirestoreDeviceDataRepository(IFirestoreConnectionFactory connectionFactory, IMessageSerializer messageSerializer)
    {
        _connectionFactory = connectionFactory;
        _messageSerializer = messageSerializer;
    }

    public async Task<string> GetDeviceData(string macAddess)
    {
        var conn = await _connectionFactory.GetFirestoreConnection();

        return await conn
            .Collection(CollectionPath)
            .GetDocumentAsync(macAddess);
    }

    public async Task SetDeviceData(string macAddess, string data)
    {
        var conn = await _connectionFactory.GetFirestoreConnection();

        await conn
            .Collection(CollectionPath)
            .SetDocumentAsync(macAddess, data);
    }
}
