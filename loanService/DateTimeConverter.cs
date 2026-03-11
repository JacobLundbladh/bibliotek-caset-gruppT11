namespace loanService;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DateTimeConverter : JsonConverter<DateTime>
{
    private readonly string format = "yyyy-MM-dd"; // Här anger du formatet du vill ha

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // När JSON kommer in, parsar string till DateTime
        return DateTime.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        // När JSON skickas, formateras DateTime som string
        writer.WriteStringValue(value.ToString(format));
    }
}