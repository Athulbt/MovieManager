// Controllers/MoviesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieManager.Models;
using MovieManager.Services;
using System.Collections.Generic;

namespace MovieManager.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieService _service;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(MovieService service, ILogger<MoviesController> logger)
        {
            _service = service;
            _logger = logger;
            _logger.LogInformation("MoviesController initialized");
        }

        // GET: Movies/index
        public IActionResult Index()
        {
            _logger.LogInformation("Movies Index page accessed");
            var movies = _service.Get();
            _logger.LogDebug("Displaying {Count} movies in index view", movies.Count);
            return View("Index", movies);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            _logger.LogInformation("Create movie page accessed");
            return View("Create");
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Movie movie)
        {
            _logger.LogInformation("Logger: Attempting to create new movie: {Title}", movie.Title);
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Logger: Create movie failed validation for: {Title}", movie.Title);
                return View("Create", movie);
            }

            _service.Create(movie);
            _logger.LogInformation("Logger: Movie created successfully: {Title}", movie.Title);
            TempData["SuccessMessage"] = $"Movie '{movie.Title}' has been added successfully!";
            return RedirectToAction("index");
        }

        // GET: Movies/Edit/{id}
        public IActionResult Edit(string id)
        {
            _logger.LogDebug("Logger: Edit page accessed for movie ID: {MovieId}", id);
            var movie = _service.Get(id);
            
            if (movie == null)
            {
                _logger.LogWarning("Logger: Movie not found for edit ID: {MovieId}", id);
                return NotFound();
            }

            _logger.LogInformation("Logger: Editing movie: {Title} (ID: {MovieId})", movie.Title, id);
            return View(movie);
        }

        // POST: Movies/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Movie updatedMovie)
        {
            _logger.LogInformation("Logger: Attempting to update movie: {Title} (ID: {MovieId})", updatedMovie.Title, updatedMovie.Id);
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Logger: Edit movie failed validation for ID: {MovieId}", updatedMovie.Id);
                return View(updatedMovie);
            }

            var movie = _service.Get(updatedMovie.Id);
            if (movie == null)
            {
                _logger.LogWarning("Logger: Movie not found for update ID: {MovieId}", updatedMovie.Id);
                return NotFound();
            }

            _service.Update(updatedMovie.Id, updatedMovie);
            _logger.LogInformation("Logger: Movie updated successfully: {Title} (ID: {MovieId})", updatedMovie.Title, updatedMovie.Id);
            TempData["SuccessMessage"] = $"Movie '{updatedMovie.Title}' has been updated successfully!";
            return RedirectToAction("index");
        }

        // GET: Movies/Delete/{id}
        public IActionResult Delete(string id)
        {
            _logger.LogDebug("Logger: Delete confirmation page for movie ID: {MovieId}", id);
            var movie = _service.Get(id);
            
            if (movie == null)
            {
                _logger.LogWarning("Movie not found for delete ID: {MovieId}", id);
                return NotFound();
            }

            _logger.LogInformation("Logger: Confirming delete for movie: {Title} (ID: {MovieId})", movie.Title, id);
            return View(movie);
        }

        // POST: Movies/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            _logger.LogInformation("Attempting to delete movie ID: {MovieId}", id);
            var movie = _service.Get(id);
            
            if (movie != null)
            {
                _service.Remove(id);
                _logger.LogInformation("Movie deleted successfully: {Title} (ID: {MovieId})", movie.Title, id);
                TempData["SuccessMessage"] = $"Movie '{movie.Title}' has been deleted successfully!";
            }
            else
            {
                _logger.LogWarning("Attempted to delete non-existent movie ID: {MovieId}", id);
            }
            
            return RedirectToAction("index");
        }
    }
}