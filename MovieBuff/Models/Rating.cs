namespace MovieBuff.Models
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int FilmId { get; set; }
        public string UserId { get; set; }
        public int Score { get; set; }
        public ApplicationUser User { get; set; }   
    }
}
