// Services/MovieService.cs
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;  // Add this
using System.Collections.Generic;
using MovieManager.Models;

namespace MovieManager.Services
{
    public class MovieService
    {
        private readonly IMongoCollection<Movie> _movies;
        private readonly ILogger<MovieService> _logger;  // Add logger field

        public MovieService(IOptions<MongoSettings> settings, ILogger<MovieService> logger)  // Add logger parameter
        {
            _logger = logger;
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _movies = database.GetCollection<Movie>("Movies");
            _logger.LogInformation("MovieService initialized with MongoDB");
        }

        public List<Movie> Get()
        {
            _logger.LogInformation("Getting all movies from database");
            var movies = _movies.Find(movie => true).ToList();
            _logger.LogDebug("Retrieved {Count} movies from database", movies.Count);
            return movies;
        }

        public Movie Get(string id)
        {
            _logger.LogDebug("Getting movie by ID: {MovieId}", id);
            var movie = _movies.Find(m => m.Id == id).FirstOrDefault();
            
            if (movie == null)
            {
                _logger.LogWarning("Movie with ID {MovieId} not found", id);
            }
            else
            {
                _logger.LogDebug("Found movie: {Title} (ID: {MovieId})", movie.Title, id);
            }
            
            return movie;
        }

        public Movie Create(Movie movie)
        {
            _logger.LogInformation("Creating new movie: {Title}", movie.Title);
            _movies.InsertOne(movie);
            _logger.LogInformation("Movie created successfully: {Title} (ID: {Id})", movie.Title, movie.Id);
            return movie;
        }

        public void Update(string id, Movie updatedMovie)
        {
            _logger.LogInformation("Updating movie ID: {MovieId}", id);
            _movies.ReplaceOne(m => m.Id == id, updatedMovie);
            _logger.LogInformation("Movie updated successfully: {Title} (ID: {MovieId})", updatedMovie.Title, id);
        }

        public void Remove(string id)
        {
            _logger.LogInformation("Removing movie ID: {MovieId}", id);
            _movies.DeleteOne(m => m.Id == id);
            _logger.LogInformation("Movie removed successfully: {MovieId}", id);
        }
    }
}