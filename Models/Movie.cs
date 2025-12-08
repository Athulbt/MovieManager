using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieManager.Models
{
    public class Movie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be more than 100 chars")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Director is required")]
        [StringLength(50, ErrorMessage = "Director name cannot be more than 50 chars")]
        public string? Director { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(30, ErrorMessage = "Genre cannot be more than 30 chars")]
        public string? Genre { get; set; }

        [Required(ErrorMessage = "Year is required")]
        [Range(1900, 2030, ErrorMessage = "Year must be between 1900 and 2030")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(0.0, 10.0, ErrorMessage = "Rating must be between 0.0 and 10.0")]
        public double Rating { get; set; }
    }
}


