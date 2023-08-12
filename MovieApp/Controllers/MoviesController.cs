using Microsoft.AspNetCore.Mvc;
using MovieApp.Models;
using MovieApp.services.Genres;
using MovieApp.services.Movies;
using MovieApp.ViewModel;
using NToastNotify;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IGenreServices _genreServices;
        private readonly IToastNotification _toastNotification;
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 11048576;

        public MoviesController(IMovieService movieService, IToastNotification toastNotification, IGenreServices genreServices)
        {
            _movieService = movieService;
            _toastNotification = toastNotification;
            _genreServices = genreServices;
        }

        public async Task<IActionResult> Index()
        {
            // var movies = await _context.Movies.OrderByDescending(m => m.Rate).ToListAsync();
            var movies = _movieService.GetAllOrdered(e => e.Rate).OrderByDescending(e=>e.Rate);

            return View(movies);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new MovieFormViewModel
            {
                // Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync()
                // Genres = _genreServices.GetAllOrdered(e => e.Name)
                Genres = _genreServices.GetAllOrdered(e => e.Name)
            };
            
            return View("MovieForm", PopulateViewModel(new MovieFormViewModel()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //  model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
              //  model.Genres = _genreServices.GetAllOrdered(e => e.Name);
                return View("MovieForm", PopulateViewModel(model));
            }

            var files = Request.Form.Files;

            if (!files.Any())
            {
                // model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
              //  model.Genres = _genreServices.GetAllOrdered(e => e.Name);
                ModelState.AddModelError("Poster", "Please select movie poster!");
                return View("MovieForm", PopulateViewModel(model));
            }

            var poster = files.FirstOrDefault();

            if (!_allowedExtenstions.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                //  model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
               // model.Genres = _genreServices.GetAllOrdered(e => e.Name);
                ModelState.AddModelError("Poster", "Only .PNG, .JPG images are allowed!");
                return View("MovieForm", PopulateViewModel(model));
            }

            if (poster.Length > _maxAllowedPosterSize)
            {
                // model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
               // model.Genres = _genreServices.GetAllOrdered(e => e.Name);
                ModelState.AddModelError("Poster", "Poster cannot be more than 1 MB!");
                return View("MovieForm", PopulateViewModel(model));
            }

            using var dataStream = new MemoryStream();

            await poster.CopyToAsync(dataStream);

            var movies = new Movie
            {
                Title = model.Title,
                GenreId = model.GenreId,
                Year = model.Year,
                Rate = model.Rate,
                Storeline = model.Storeline,
                Poster = dataStream.ToArray()
            };

            await _movieService.Add(movies);

            _toastNotification.AddSuccessToastMessage("Movie created successfully");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
                return BadRequest();

            //    var movie = await _context.Movies.FindAsync(id);
            var movie = await _movieService.GetById(id);

            if (movie == null)
                return NotFound();

            var viewModel = new MovieFormViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                GenreId = movie.GenreId,
                Rate = movie.Rate,
                Year = movie.Year,
                Storeline = movie.Storeline,
                Poster = movie.Poster,
                //   Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync()
                Genres = _genreServices.GetAllOrdered(e => e.Name)
            };

            return View("MovieForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = _genreServices.GetAllOrdered(e => e.Name);
                return View("MovieForm", model);
            }

            var movie = await _movieService.GetById(model.Id);

            if (movie == null)
                return NotFound();

            var files = Request.Form.Files;

            if (files.Any())
            {
                var poster = files.FirstOrDefault();

                using var dataStream = new MemoryStream();

                await poster.CopyToAsync(dataStream);

                model.Poster = dataStream.ToArray();

                if (!_allowedExtenstions.Contains(Path.GetExtension(poster.FileName).ToLower()))
                {
                    // model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                   // model.Genres = _genreServices.GetAllOrdered(e => e.Name);
                    ModelState.AddModelError("Poster", "Only .PNG, .JPG images are allowed!");
                    return View("MovieForm", PopulateViewModel(model));
                }

                if (poster.Length > _maxAllowedPosterSize)
                {
                    // model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                  //  model.Genres = _genreServices.GetAllOrdered(e => e.Name);
                    ModelState.AddModelError("Poster", "Poster cannot be more than 1 MB!");
                    return View("MovieForm", PopulateViewModel(model));
                }

                movie.Poster = model.Poster;
            }

            movie.Title = model.Title;
            movie.GenreId = model.GenreId;
            movie.Year = model.Year;
            movie.Rate = model.Rate;
            movie.Storeline = model.Storeline;
            await _movieService.updateAsync(movie.Id, movie);
            // _context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Movie updated successfully");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
                return BadRequest();

            // var movie = await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
            var movie = await _movieService.GetById(id, s => s.Genre);

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
                return BadRequest();

            // var movie = await _context.Movies.FindAsync(id);
            var movie = await _movieService.GetById(id);

            if (movie == null)
                return NotFound();

            //_context.Movies.Remove(movie);
            //_context.SaveChanges();
            await _movieService.DeleteAsync(id);

            return Ok();
        }

        private MovieFormViewModel PopulateViewModel(MovieFormViewModel? model = null)
        {
            //check the model
            MovieFormViewModel viewModel = model is null ? new MovieFormViewModel() : model;

            var Generic = _genreServices.GetAllOrdered(e => e.Name);

            viewModel.Genres = Generic;
            return viewModel;
        }
    }
}
