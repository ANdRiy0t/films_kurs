using System.Text.Json;
using System.Text.RegularExpressions;
using kino_ku.Entities;
using kino_ku.Helpers;
using kino_ku.Settings;
using kino_ku.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace kino_ku.Controllers;
	public class AccountController : Controller
	{
		private readonly HttpClient httpClient;
		public AccountController(IHttpClientFactory httpClientFactory)
		{
			httpClient = httpClientFactory.CreateClient("API");
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(UserRegistrationViewModel viewModel)
		{
			bool password = Regex.IsMatch(viewModel.Password, RegexPatterns.PasswordRegexPattern) && viewModel.Password == viewModel.ConfirmedPassword;
			bool userName = Regex.IsMatch(viewModel.Username, RegexPatterns.NameRegexPattern);
			bool email = Regex.IsMatch(viewModel.Email, RegexPatterns.EmailRegexPattern);
			
			// viewModel.UsernameMessage = string.Empty;
			// viewModel.EmailMessage = string.Empty;
			// viewModel.PasswordMessage = string.Empty;
			
			viewModel.PasswordMessage = RegexPatterns.AddMessage(password, "Password must be 8+ chars and use EN lang, include uppercase and number");
			viewModel.UsernameMessage = RegexPatterns.AddMessage(userName, "Username must be 2-13 chars");
			
			
			if (password && userName && email)
			{
				
				string endpoint = $"User/GetByEmail?email={viewModel.Email}";
				var data = await httpClient.GetFromJsonAsync<User>(endpoint);

				if (data?.Email == viewModel.Email)
				{
					viewModel.EmailMessage = "This email already register";
				}
				
				User user = new User
				{
					Email = viewModel.Email,
					HashedPassword = viewModel.Password.HashToSha256(),
					Username = viewModel.Username,
				};

				endpoint = "User/Add";

				var response = await httpClient.PostAsJsonAsync(endpoint, user);

				if (response.IsSuccessStatusCode)
				{
					endpoint = $"User/GetByEmail?email={user.Email}";
					response = await httpClient.GetAsync(endpoint);
					data = await response.Content.ReadFromJsonAsync<User>();

					CookieOptions cookie = new CookieOptions();
					cookie.Expires = DateTime.Now.AddMinutes(60);
					
					Response.Cookies.Append("Login", data.Id, cookie);
					Response.Cookies.Append("Username", data.Username, cookie);

					return RedirectToAction("Index", "Home");
				}
			}
			
			viewModel.PasswordMessage = RegexPatterns.AddMessage(password, "Password must be 8+ chars and use EN lang, include uppercase and number");
			viewModel.Username = RegexPatterns.AddMessage(userName, "Username must be 2-13 chars");
            // viewModel.EmailMessage = RegexPatterns.AddMessage(data.Email == null, "This email already register");

			return View(viewModel);
		}

		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(UserLoginViewModel viewModel)
		{
			viewModel.EmailMessage = string.Empty;
			viewModel.PasswordMessage = string.Empty;


			string endpoint = $"User/GetByEmail?email={viewModel.Email}";
			var data = await httpClient.GetFromJsonAsync<User>(endpoint);


			if (data.Email == null)
			{
				viewModel.EmailMessage = "This email not register";

				return View(viewModel);
			}
			
			var hashedPassword = viewModel.Password.HashToSha256();
			if (hashedPassword == data.HashedPassword)
			{
				CookieOptions cookie = new CookieOptions();
				cookie.Expires = DateTime.Now.AddMinutes(60);

				Response.Cookies.Append("Login", data.Id, cookie);
				Response.Cookies.Append("Username", data.Username, cookie);

				if (data.TypeUser == TypeUser.Admin)
				{
					// sskey - secret cookies for
					Response.Cookies.Append("sskey", ConstantValues.SecretKey, cookie);
					return RedirectToAction("Index", "Admin");
				}
				return RedirectToAction("Index", "Home");
			}

			viewModel.PasswordMessage = RegexPatterns.AddMessage(hashedPassword == data.HashedPassword, "Wrong password");

			return View(viewModel);

		}

		[HttpPost]
		public IActionResult Logout()
		{
			Response.Cookies.Delete("Login");
			Response.Cookies.Delete("Username");
			return Redirect("/");
		}

	}




