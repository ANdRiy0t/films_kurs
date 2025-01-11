using Microsoft.AspNetCore.Identity;

namespace kino_ku.Api.Entities;

public class Movie : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageToBase64 { get; set; }
}