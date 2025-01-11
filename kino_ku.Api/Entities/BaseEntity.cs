using System.Text.Json.Serialization;
using kino_ku.Api.Helpers;
using MongoDB.Bson;

namespace kino_ku.Api.Entities;
    public class BaseEntity
    {
        [JsonConverter(typeof(ObjectIdJsonConverter))]
        public ObjectId Id { get; set; }
    }

