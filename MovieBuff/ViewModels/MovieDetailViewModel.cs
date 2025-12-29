using MovieBuff.DTOs;
namespace MovieBuff.ViewModels
{
    public class MovieDetailViewModel
    {
        public MovieDetailDto Movie { get; set; }
        public bool IsInWatchLater { get; set; }
        public bool IsFavorite { get; set; }
        public int? UserRating { get; set; }
    }
}
