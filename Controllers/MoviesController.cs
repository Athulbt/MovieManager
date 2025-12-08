using Microsoft.AspNetCore.Mvc;
using MovieManager.Models;
using MovieManager.Data;
using MovieManager.Services;
using System.Collections.Generic;

namespace MovieManager.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieService _service;

        public MoviesController(MovieService service)
        {
            _service = service;
        }

        // GET: Movies/index
        public IActionResult Index()
        {
            var movies = _service.Get();
            return View("Index", movies);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View("Create");
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", movie);
            }

            _service.Create(movie);
            return RedirectToAction("index");
        }

        // GET: Movies/Edit/{id}
        public IActionResult Edit(string id)
        {
            var movie = _service.Get(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        // POST: Movies/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Movie updatedMovie)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedMovie);
            }

            var movie = _service.Get(updatedMovie.Id);
            if (movie == null) return NotFound();

            _service.Update(updatedMovie.Id, updatedMovie);
            return RedirectToAction("index");
        }

        // GET: Movies/Delete/{id}
        public IActionResult Delete(string id)
        {
            var movie = _service.Get(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        // POST: Movies/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var movie = _service.Get(id);
            if (movie == null) return NotFound();

            _service.Remove(id);
            return RedirectToAction("index");
        }
    }
}
