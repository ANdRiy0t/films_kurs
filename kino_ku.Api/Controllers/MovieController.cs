using kino_ku.Api.Entities;
using kino_ku.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kino_ku.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IGenericRepository<Movie> _movieRepository;

        public MovieController(IGenericRepository<Movie> movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // Додати новий фільм
        [HttpPost]
        [Route("Add")]
        public async Task Add([FromBody] Movie movie)
        {
            if (movie == null)
            {
                return;
            }
            
            await _movieRepository.Add(movie);
        }

        // Отримати всі фільми
        [HttpGet]
        [Route("GetAll")]
        public async Task<List<Movie>> GetAll()
        {
            var data = _movieRepository.GetAll().ToList();
            return data;
        }

        // Отримати фільм за ID
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<Movie>> GetById(string id)
        {
            var movie = await _movieRepository.Get(ObjectId.Parse(id));
            if (movie == null)
            {
                return NotFound($"Movie with ID {id} not found.");
            }
            return Ok(movie);
        }

        // Оновити фільм
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Movie movie)
        {
            if (movie == null || movie.Id == null)
            {
                return BadRequest("Invalid movie data.");
            }

            var existingMovie = await _movieRepository.Get(ObjectId.Parse(movie.Id.ToString()));
            if (existingMovie == null)
            {
                return NotFound($"Movie with ID {movie.Id} not found.");
            }

            await _movieRepository.Update(movie);
            return Ok(new { message = "Movie updated successfully!", movie });
        }

        // Видалити фільм
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task Delete(string id)
        {
            var movie = await _movieRepository.Get(ObjectId.Parse(id));
            if (movie.Title == null)
            {
                return;
            }

            await _movieRepository.Delete(movie);
        }
    }
}
