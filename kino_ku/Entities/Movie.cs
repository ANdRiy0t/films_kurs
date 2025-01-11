namespace kino_ku.Entities;

public class Movie
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageToBase64 { get; set; }
}