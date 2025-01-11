using kino_ku.Api.Entities;
using kino_ku.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace kino_ku.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IGenericRepository<User> _userRepository;

        public UserController(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost]
		[Route ("Add")]
		public void Add(User user)
		{
			_userRepository.Add(user);
		}
        [HttpGet]
        [Route("GetAll")]
        public List<User> GetAll()
        {
            var data = _userRepository.GetAll().ToList();
            return data;
        }

		[HttpGet]
		[Route("GetById")]
		public async Task<User> GetById(string id)
		{
			var data = await _userRepository.Get(ObjectId.Parse(id));
			return data;
		}

		[HttpGet]	
        [Route("GetByEmail")]
        public async Task<User> GetByEmail(string email)
        {
            var data = await _userRepository.FindOneAsync(x=>x.Email == email);
            return data?.Id != null ? data : new User();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task Delete(string id)
        {
            var user = await _userRepository.FindOneAsync(x=>x.Id == ObjectId.Parse(id));
            await _userRepository.Delete(user);
        }

        [HttpPut]
        [Route("Update")]
        public void Update(User user)
        {
            _userRepository.Update(user);
        }
    }
}
