using System.Text.Json;
using System.Text.Json.Serialization;
using MongoDB.Bson;

namespace kino_ku.Api.Helpers;

public class ObjectIdJsonConverter : JsonConverter<ObjectId>
{
    public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var id = reader.GetString();
        ObjectId.TryParse(id, out ObjectId objectId);
        return objectId;
    }

    public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
    
}