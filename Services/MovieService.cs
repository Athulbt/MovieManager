using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using MovieManager.Models;

namespace MovieManager.Services
{
    public class MovieService
    {
        private readonly IMongoCollection<Movie> _movies;
 
        public MovieService(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _movies = database.GetCollection<Movie>("Movies");
        }
 
        public List<Movie> Get() => _movies.Find(movie => true).ToList();
        public Movie Get(string id) => _movies.Find(m => m.Id == id).FirstOrDefault();
 
        public Movie Create(Movie movie)
        {
            _movies.InsertOne(movie);
            return movie;
        }
        public void Update(string id, Movie updatedMovie) =>
            _movies.ReplaceOne(m => m.Id == id, updatedMovie);
 
        public void Remove(string id) =>
            _movies.DeleteOne(m => m.Id == id);
 
    }
}
