using MovieBuff.DTOs;
using MovieBuff.Models;
namespace MovieBuff.ViewModels
{
    public class DashboardViewModel
    {
        public List<MovieResultDto> WatchLaterMovies { get; set; }
        public List<MovieResultDto> FavoriteMovies { get; set; }
        public List<UserRatingViewModel> LastRatings { get; set; }
        public List<UserList> UserLists { get; set; } 
    }
}
