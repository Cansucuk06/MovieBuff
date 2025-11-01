namespace MovieBuff.Models
{
    public class WatchLater
    {
        public int WatchLaterId { get; set; }
        public int FilmId { get; set; }
        public string UserId { get; set; }
        public Film Film { get; set; }
        public ApplicationUser User { get; set; }   
    }
}
