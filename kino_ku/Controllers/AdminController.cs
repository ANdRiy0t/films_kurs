using System.Text.RegularExpressions;
using kino_ku.Entities;
using kino_ku.Helpers;
using kino_ku.Settings;
using kino_ku.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace kino_ku.Controllers;
	public class AdminController : Controller
	{
		private readonly HttpClient httpClient;
		public AdminController(IHttpClientFactory httpClientFactory)
		{
			httpClient = httpClientFactory.CreateClient("API");
		}


		public async Task<IActionResult> Index()
		{
			if (HttpContext.Request.Cookies.TryGetValue("sskey", out string sskey))
			{
				var validKey = sskey == ConstantValues.SecretKey;
				if(!validKey) return View("Error");
			}
			var viewModel = new AdminViewModel();
			
			var endpoint = "Movie/GetAll";
			var movies = await httpClient.GetFromJsonAsync<List<Movie>>(endpoint);
			viewModel.Movies = movies ?? new();
			
			endpoint = "User/GetAll";
			var users = await httpClient.GetFromJsonAsync<List<User>>(endpoint);
			viewModel.Users = users ?? new();
			
			return View(viewModel);
		}
		
		[HttpPost]
		public async Task<IActionResult> CreateMovie([FromForm] Movie movie, IFormFile? file)
		{
			if (HttpContext.Request.Cookies.TryGetValue("sskey", out string sskey))
			{
				var validKey = sskey == ConstantValues.SecretKey;
				if(!validKey) return View("Error");
			}
			
			string[] allowedExtensions = { ".png", ".jpeg", ".jpg" };
			if (file == null)
			{
				return RedirectToAction("Index");
			}

			var fileExtension = Path.GetExtension(file.FileName).ToLower();

			if (!allowedExtensions.Contains(fileExtension))
			{
				return RedirectToAction("Index");
			}
			
			string base64Image = await file.ConvertToBase64();
			movie.ImageToBase64 = base64Image;
			
			var endpoint = "Movie/Add";
			await httpClient.PostAsJsonAsync(endpoint, movie);

			return RedirectToAction("Index");
		}
				
		[HttpPost]
		public async Task<IActionResult> EditMovie([FromForm] Movie movie, IFormFile? file)
		{
			if (HttpContext.Request.Cookies.TryGetValue("sskey", out string sskey))
			{
				var validKey = sskey == ConstantValues.SecretKey;
				if(!validKey) return View("Error");
			}
			
			string[] allowedExtensions = { ".png", ".jpeg", ".jpg" };
			if (file != null)
			{
				var fileExtension = Path.GetExtension(file.FileName).ToLower();

				if (!allowedExtensions.Contains(fileExtension))
				{	
					return RedirectToAction("Index");
				}
				
				string base64Image = await file.ConvertToBase64();
				movie.ImageToBase64 = base64Image;
			}
			
			
			var endpoint = "Movie/Update";
			await httpClient.PutAsJsonAsync(endpoint, movie);

			return RedirectToAction("Index");
		}
		
		
		[HttpPost]
		public async Task<IActionResult> DeleteMovie([FromForm] string movieId)
		{
			if (HttpContext.Request.Cookies.TryGetValue("sskey", out string sskey))
			{
				var validKey = sskey == ConstantValues.SecretKey;
				if(!validKey) return View("Error");
			}
			
			var endpoint = $"Movie/Delete/{movieId}";
			await httpClient.DeleteAsync(endpoint);

			return RedirectToAction("Index");
		}
		
	}

