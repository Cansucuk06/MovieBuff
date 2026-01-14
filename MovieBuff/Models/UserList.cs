using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MovieBuff.Models
{
    public class UserList
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<UserListItem> ListItems { get; set; } = new List<UserListItem>();
    }
}
