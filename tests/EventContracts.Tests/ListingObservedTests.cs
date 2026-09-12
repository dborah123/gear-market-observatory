using System.Text.Json;
using GearMarketObservatory.EventContracts;
using Xunit;

namespace GearMarketObservatory.EventContracts.Tests;

public sealed class ListingObservedTests
{
    [Fact]
    public void SerializesTheStableSchemaVersionAndWireNames()
    {
        var json = JsonSerializer.Serialize(CreateEvent());
        using var document = JsonDocument.Parse(json);

        Assert.Equal(1, document.RootElement.GetProperty("schemaVersion").GetInt32());
        Assert.Equal("ListingObserved", document.RootElement.GetProperty("eventType").GetString());
        Assert.Equal("v1|123", document.RootElement.GetProperty("sourceListingId").GetString());
        Assert.True(document.RootElement.TryGetProperty("rawPayload", out _));
        Assert.False(json.Contains("EventId", StringComparison.Ordinal));
    }

    [Fact]
    public void DeserializesASchemaCompatiblePayloadWithUnknownFields()
    {
        const string json = """
        {
          "eventId": "00000000-0000-0000-0000-000000000001",
          "eventType": "ListingObserved",
          "schemaVersion": 1,
          "occurredAt": "2026-09-12T20:00:00Z",
          "source": "ebay",
          "sourceListingId": "v1|123",
          "sourceQueryId": "sample-query",
          "marketplace": "EBAY_US",
          "listingUrl": "https://www.ebay.com/itm/123",
          "rawPayload": { "bucket": "example", "key": "raw/example.json", "sha256": "abc" },
          "producer": { "name": "ebay-collector", "version": "test" },
          "futureField": "ignored"
        }
        """;

        var value = JsonSerializer.Deserialize<ListingObserved>(json);

        Assert.NotNull(value);
        Assert.Equal(1, value!.SchemaVersion);
        Assert.Equal("v1|123", value.SourceListingId);
    }

    private static ListingObserved CreateEvent() => new()
    {
        EventId = "00000000-0000-0000-0000-000000000001",
        OccurredAt = new DateTimeOffset(2026, 9, 12, 20, 0, 0, TimeSpan.Zero),
        Source = "ebay",
        SourceListingId = "v1|123",
        SourceQueryId = "sample-query",
        Marketplace = "EBAY_US",
        ListingUrl = new Uri("https://www.ebay.com/itm/123"),
        RawPayload = new RawPayloadReference
        {
            Bucket = "example",
            Key = "raw/example.json",
            Sha256 = "abc"
        },
        Producer = new ProducerMetadata
        {
            Name = "ebay-collector",
            Version = "test"
        }
    };
}
