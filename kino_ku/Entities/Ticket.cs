using MongoDB.Bson;

namespace kino_ku.Entities;

public class Ticket 
{
    public string UserId { get; set; }
    public string MovieId { get; set; }
    public int PlaceNumber { get; set; }
    public DateTime Time { get; set; }
}