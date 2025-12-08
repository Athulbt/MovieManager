using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieManager.Data;
using MovieManager.Models;
using MovieManager.Services;

namespace MovieManager.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesApiController : ControllerBase
    {
        private readonly MovieService _service;

        public MoviesApiController(MovieService service)
        {
            _service = service;
        }

        
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> Get()
        {
            var movies = _service.Get();
            return Ok(movies);

        }

        [HttpGet("{id}")]
        public ActionResult<Movie> Get(string id)
        {
            var movie = _service.Get(id);
            if (movie == null)
                return NotFound();
            return Ok(movie);
        }

        [HttpPost]
        public ActionResult<Movie> Post(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _service.Create(movie);
                return CreatedAtAction(nameof(Get), movie);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, Movie movie)
        {
            if (ModelState.IsValid)
            {
                var existing = _service.Get(id);
                if (existing == null) return NotFound();
                _service.Update(id, movie);
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var mov = _service.Get(id);
            if (mov == null) return NotFound();
            _service.Remove(id);
            return NoContent();
        }
    }
}