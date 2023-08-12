using Microsoft.AspNetCore.Mvc;
using MovieApp.Models;
using MovieApp.services.Genres;
using MovieApp.services.Movies;
using MovieApp.ViewModel;
using NToastNotify;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreServices _genreServices;
        private readonly IMovieService _movieService;
        private readonly IToastNotification _toastNotification;


        public GenreController(IGenreServices genreServices, IToastNotification toastNotification, IMovieService movieService)
        {
            _genreServices = genreServices;
            _toastNotification = toastNotification;
            _movieService = movieService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _genreServices.GellAll();
            var viewModel =  data.Select(e => new GenreViewModel
            {
                Id = e.Id,
                Name = e.Name
            });
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            GenreFormViewModel model=new();
            return View("GenreForm", model);
        }

        [HttpPost]
        public IActionResult Create(GenreFormViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            var genre = new Genre
            {
                Name = model.Name
            };
            _genreServices.Add(genre);
            _toastNotification.AddSuccessToastMessage("Genr Insert successfully");

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var genre = await _genreServices.GetById(id);
            if (genre is null) return NotFound();

            var viewModel = new GenreFormViewModel
            {
                Id = genre.Id,
                Name = genre.Name
            };

            return View("GenreForm", viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(GenreFormViewModel model)
        {

            if (!ModelState.IsValid) return BadRequest(model);
            var genre = await _genreServices.GetById(model.Id);

            genre.Name = model.Name;
            await _genreServices.updateAsync(genre.Id, genre);

            _toastNotification.AddSuccessToastMessage("Genr updated successfully");
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
                return BadRequest();

            var movie = await _genreServices.GetById(id);

            if (movie == null)
                return NotFound();

  
            await _genreServices.DeleteAsync(id);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Search()
        {
            var viewModel = new MovieFormViewModel
            {
         
                Genres = _genreServices.GetAllOrdered(e => e.Name)
            };

            return View("SearchByGenre", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Search([FromQuery]int genreId)
        {

            var data = await _genreServices.GetAllMovieByGenreName(genreId);
          
          
           // return Ok(list);
            return PartialView("_GetMovieByGener", data);
        }

        


    }
}
