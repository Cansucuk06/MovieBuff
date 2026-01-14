namespace MovieBuff.Models
{
    public class UserListItem
    {
        public int Id { get; set; }

        public int FilmId { get; set; }
        public int UserListId { get; set; } 
        public virtual UserList UserFilmList { get; set; }
    }
}
