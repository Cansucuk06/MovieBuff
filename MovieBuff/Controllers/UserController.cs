using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MovieBuff.DTOs;
using MovieBuff.Models;
using MovieBuff.Services;
using MovieBuff.Data;
using MovieBuff.ViewModels;
using System.Security.Claims;
using Microsoft.Identity.Client;

[Authorize]
public class UserController: Controller
{
    private readonly MovieBuffContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMovieService _movieService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UserController(MovieBuffContext context,UserManager<ApplicationUser> userManager, IMovieService movieService,IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _userManager = userManager;
        _movieService = movieService;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Dashboard()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var watchLaterTask = await _context.WatchLaters
            .Where(w => w.UserId == userId)
            .Select(w => w.FilmId)
            .ToListAsync();

        var favoriteTask = await _context.Favorites
            .Where(f => f.UserId == userId)
            .Select(f => f.FilmId)
            .ToListAsync();

        var userListTask = await _context.UserLists
            .Where(l => l.UserId == userId)
            .OrderBy(l => l.Name)
            .ToListAsync();

        var ratingTask = await _context.Ratings
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.RatingId)
            .Take(5)
            .ToListAsync();


        var watchLaterIds = watchLaterTask;
        var favoriteIds = favoriteTask;
        var ratingIds =  ratingTask;
        var userLists = userListTask;

        var watchLaterMovies = new List<MovieResultDto>();
        foreach(var id in watchLaterIds)
        {
            var movieDetail = await _movieService.GetMovieDetailsAsync(id);
            if(movieDetail != null)
            {
                watchLaterMovies.Add(new MovieResultDto
                {
                    Id = movieDetail.Id,
                    Title = movieDetail.Title,
                    PosterPath = movieDetail.PosterPath 
                });
            }
        }

        var favoriteMovies = new List<MovieResultDto>();
        foreach(var id in favoriteIds)
        {
            var movieDetail = await _movieService.GetMovieDetailsAsync(id);
            if(movieDetail != null)
            {
                favoriteMovies.Add(new MovieResultDto
                {
                    Id = movieDetail.Id,
                    Title = movieDetail.Title,
                    PosterPath = movieDetail.PosterPath
                });
            }
        }

        var lastRatings = new List<UserRatingViewModel>();
        foreach(var rating in ratingIds)
        {
            var movieDetail= await _movieService.GetMovieDetailsAsync(rating.FilmId);
            if(movieDetail!= null)
            {
                lastRatings.Add(new UserRatingViewModel
                {
                    Score = rating.Score,
                    Movie = new MovieResultDto
                    {
                        Id = movieDetail.Id,
                        Title = movieDetail.Title,
                        PosterPath = movieDetail.PosterPath
                    }
                });
            }
        }
        

        var dashboardViewModel = new DashboardViewModel
        {
            WatchLaterMovies = watchLaterMovies,
            FavoriteMovies = favoriteMovies,
            LastRatings = lastRatings,
            UserLists = userLists
        };

        return View(dashboardViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> EditProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if(user == null)
        {
            return NotFound($"Kullanıcı Bulunamadı.");
        }

        var viewModel = new ProfileViewModel
        {
            Email = user.Email,
            UserName = user.UserName,
            Country = user.Country,
            ProfilePictureUrl = user.ProfilePictureUrl
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(ProfileViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if(user == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.ProfilePictureUrl = user.ProfilePictureUrl;
            return View(model);
        }

        if(model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/profiles");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfilePictureFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.ProfilePictureFile.CopyToAsync(fileStream);
            }
            user.ProfilePictureUrl = "/images/profiles/" + uniqueFileName;
        }

        user.UserName = model.UserName;
        user.Country = model.Country;
        user.Email = model.Email;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Profiliniz başarıyla güncellendi.";
            return RedirectToAction("Dashboard");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        model.ProfilePictureUrl = user.ProfilePictureUrl;
        return View(model);
    }
}
