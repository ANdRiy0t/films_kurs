using System.Diagnostics;
using System.Net;
using kino_ku.Entities;
using kino_ku.Models;
using kino_ku.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace kino_ku.Controllers;

	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly HttpClient _httpClient;

		public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
		{
			_logger = logger;
			_httpClient = httpClientFactory.CreateClient("API");//httpClientFactory.CreateClient("API");
		}

		public async Task<IActionResult> Index()
		{
			var viewModel = new HomeIndexViewModel();
			var endpoint = "Movie/GetAll";
			var movies = await _httpClient.GetFromJsonAsync<List<Movie>>(endpoint);
			viewModel.Movies = movies ?? new();
			
			return View(viewModel);
		}
		
		
		[HttpPost]
		public async Task<IActionResult> MovieDescription(string movieId)
		{
			var viewModel = new HomeMovieDescriptionViewModel();
			var movie = await _httpClient.GetFromJsonAsync<Movie>($"Movie/GetById/{movieId}");
			viewModel.Movie = movie ?? new();
			
			return View(viewModel);
		}
		
		[HttpPost]
		
		public async Task<IActionResult> OrderTicket([FromBody] HomeOrderTicketViewModel viewModel)
		{
			string dateTimeString = viewModel.Date + " " + viewModel.Time;
			HttpContext.Request.Cookies.TryGetValue("Login", out var userId);
			if (!ObjectId.TryParse(userId, out ObjectId objectId))
			{
				return Unauthorized();
			}
			
			var ticket = new Ticket
			{
				MovieId = viewModel.MovieId,
				UserId = userId,
				PlaceNumber = viewModel.Seat,
				Time = DateTime.ParseExact(dateTimeString, "yyyy-MM-dd HH:mm", null) 
			};
			 var movie = await _httpClient.PostAsJsonAsync("Ticket/Add", ticket);

			 if (movie.IsSuccessStatusCode)
			 {
				 return Ok();
			 }
			 
			return BadRequest();
		}
		
		[HttpGet]
		public async Task<IActionResult> GetSeats(string date, string time, string movieId)
		{
			
			var endpoint = $"Ticket/GetOrderedSeatNumbers?date={date}&time={time}&movieId={movieId}";
			var orderedSeats = await _httpClient.GetFromJsonAsync<List<int>>(endpoint);
			return Json(new { orderedSeats });
		}
		
		
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
