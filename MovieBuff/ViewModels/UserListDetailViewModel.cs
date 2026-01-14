using MovieBuff.Models;
using MovieBuff.DTOs;
using System.Collections.Generic;
namespace MovieBuff.ViewModels
{
    public class UserListDetailViewModel
    {
        public UserList ListInfo { get; set; }
        public List<MovieResultDto> Movies { get; set; }
    }
}
