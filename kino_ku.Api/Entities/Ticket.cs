using System.Text.Json.Serialization;
using kino_ku.Api.Helpers;
using MongoDB.Bson;

namespace kino_ku.Api.Entities;

public class Ticket : BaseEntity
{
    [JsonConverter(typeof(ObjectIdJsonConverter))]
    public ObjectId UserId { get; set; }
    [JsonConverter(typeof(ObjectIdJsonConverter))]
    public ObjectId MovieId { get; set; }
    public int PlaceNumber { get; set; }
    public DateTime Time { get; set; }
}