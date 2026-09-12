using System.Text.Json.Serialization;

namespace GearMarketObservatory.EventContracts;

/// <summary>
/// The source-agnostic event emitted when a source listing is observed.
/// The schema version is part of the serialized contract and changes only with a reviewed contract change.
/// </summary>
public sealed record ListingObserved
{
    [JsonPropertyName("eventId")]
    public required string EventId { get; init; }

    [JsonPropertyName("eventType")]
    public string EventType { get; init; } = "ListingObserved";

    [JsonPropertyName("schemaVersion")]
    public int SchemaVersion { get; init; } = 1;

    [JsonPropertyName("occurredAt")]
    public required DateTimeOffset OccurredAt { get; init; }

    [JsonPropertyName("source")]
    public required string Source { get; init; }

    [JsonPropertyName("sourceListingId")]
    public required string SourceListingId { get; init; }

    [JsonPropertyName("sourceQueryId")]
    public required string SourceQueryId { get; init; }

    [JsonPropertyName("marketplace")]
    public required string Marketplace { get; init; }

    [JsonPropertyName("listingUrl")]
    public required Uri ListingUrl { get; init; }

    [JsonPropertyName("rawPayload")]
    public required RawPayloadReference RawPayload { get; init; }

    [JsonPropertyName("producer")]
    public required ProducerMetadata Producer { get; init; }
}

public sealed record RawPayloadReference
{
    [JsonPropertyName("bucket")]
    public required string Bucket { get; init; }

    [JsonPropertyName("key")]
    public required string Key { get; init; }

    [JsonPropertyName("sha256")]
    public required string Sha256 { get; init; }
}

public sealed record ProducerMetadata
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("version")]
    public required string Version { get; init; }
}
