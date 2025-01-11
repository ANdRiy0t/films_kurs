using kino_ku.Api.Entities;
using kino_ku.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace kino_ku.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketController : ControllerBase
{
    private readonly IGenericRepository<Ticket> _ticketRepository;

    public TicketController(IGenericRepository<Ticket> ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }
    
    [HttpGet]
    [Route("GetOrderedSeatNumbers")]
    public async Task<List<int>> GetOrderedSeatNumbers(string date, string time, ObjectId movieId)
    {
        if (movieId == ObjectId.Empty || string.IsNullOrEmpty(date) || string.IsNullOrEmpty(date))
            return null;
        
        string dateTimeString = date + " " + time;
        var dateTime = DateTime.ParseExact(dateTimeString, "yyyy-MM-dd HH:mm", null);
        var data = _ticketRepository.GetAll().Where(x=>x.Time.ToUniversalTime() == dateTime.ToUniversalTime() && x.MovieId == movieId).Select(x=>x.PlaceNumber).ToList();
        return data;
    }
    
    [HttpPost]
    [Route("Add")]
    public void Add(Ticket ticket)
    {
        _ticketRepository.Add(ticket);
    }
}