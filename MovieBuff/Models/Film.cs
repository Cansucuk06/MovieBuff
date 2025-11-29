using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace MovieBuff.Models
{
    public class Film
    {
        [Key]
        public int FilmId { get; set; }

        public int ApiFilmId { get; set; }

        [Required]
        public string Title { get; set; }
        public int Year { get; set; }
        public string? Genre { get; set; }

        public string? Summary { get; set; }
        public string? PosterUrl { get; set; }
        public double ImdbRating { get; set; }
        public string? Director { get; set; }
        public string? Cast { get; set; }
    }
}
