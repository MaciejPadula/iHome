using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace iHome.Microservices.Devices.Infrastructure.Models;

public class FirestoreOptions
{
    public string ProjectId { get; set; } = default!;
    public ServiceAccountSettings JsonCredentials { get; set; } = default!;
}

public class ServiceAccountSettings
{
    [JsonProperty("type")]
    public string Type { get; set; } = default!;

    [JsonProperty("project_id")]
    public string ProjectId { get; set; } = default!;

    [JsonProperty("private_key_id")]
    public string PrivateKeyId { get; set; } = default!;

    [JsonProperty("private_key")]
    public string PrivateKey { get; set; } = default!;

    [JsonProperty("client_email")]
    public string ClientEmail { get; set; } = default!;

    [JsonProperty("client_id")]
    public string ClientId { get; set; } = default!;

    [JsonProperty("auth_uri")]
    public string AuthUri { get; set; } = default!;

    [JsonProperty("token_uri")]
    public string TokenUri { get; set; } = default!;

    [JsonProperty("auth_provider_x509_cert_url")]
    public string AuthProviderCertUrl { get; set; } = default!;

    [JsonProperty("client_x509_cert_url")]
    public string ClientCertUrl { get; set; } = default!;

    [JsonProperty("universe_domain")]
    public string UniverseDomain { get; set; } = default!;
}