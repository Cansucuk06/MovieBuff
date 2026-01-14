using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBuff.Data;
using MovieBuff.ViewModels;
using MovieBuff.DTOs;
using System.Security.Claims;
using MovieBuff.Models;
using MovieBuff.Services;
using MovieBuff.DTOs;
namespace MovieBuff.Controllers
{
    [Authorize]
    public class UserFilmListController : Controller
    {
        private readonly MovieBuffContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMovieService _movieService;
        public UserFilmListController(MovieBuffContext context, UserManager<ApplicationUser> userManager, IMovieService movieService)
        {
            _context = context;
            _userManager = userManager;
            _movieService = movieService;
        }

        private String GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var userLists = await _context.UserLists
                .Where(ul => ul.UserId == userId)
                .Include(ul => ul.ListItems)
                .ToListAsync();

            return View(userLists);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = GetUserId();

            var userLists = await _context.UserLists
                .Include(ul => ul.ListItems)
                .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.Id == id);

            if (userLists == null)
            {
                return NotFound();
            }

            var movieIds = userLists.ListItems
                .Select(li => li.FilmId)
                .ToList();

            var movies = new List<MovieResultDto>();

            foreach (var movieId in movieIds)
            {
                var movieDetail = await _movieService.GetMovieDetailsAsync(movieId);
                if (movieDetail != null)
                {
                    movies.Add(new MovieResultDto
                    {
                        Id = movieDetail.Id,
                        Title = movieDetail.Title,
                        PosterPath = movieDetail.PosterPath
                    });
                }
            }

            var viewModel = new UserListDetailViewModel
            {
                ListInfo = userLists,
                Movies = movies
            };

            return View(viewModel);
        }
        public IActionResult Create()
        {
            return View(new UserListCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserListCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                var userList = new UserList 
                {
                    Name = model.Name,
                    Description = model.Description,
                    UserId = GetUserId() 
                };

                _context.Add(userList);
                await _context.SaveChangesAsync();

                return RedirectToAction("Dashboard", "User");
            }
            return View(model);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = GetUserId();
            var userList = await _context.UserLists
                .FirstOrDefaultAsync(ul => ul.Id == id && ul.UserId == userId);

            if(userList == null)
            {
                return NotFound();
            }

            return View(userList);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetUserId();
            var userList = await _context.UserLists
                .Include(ul => ul.ListItems)
                .FirstOrDefaultAsync(ul => ul.Id == id && ul.UserId == userId);

            if (userList != null)
            {
                _context.UserLists.Remove(userList);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Dashboard", "User");
        }
    }
}
