namespace MovieBuff.Models
{
    public class Favorite
    {
        public int FavoriteId { get; set; }
        public int FilmId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
