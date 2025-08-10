using CinemaApp.Data;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;
using static CinemaApp.Web.ViewModels.ValidationMessages.Movie;

namespace CinemaApp.Web.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService movieService;
        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllMoviesIndexViewModel> allMovies = await this.movieService.GetAllMoviesAsync();

            return View(allMovies);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(MovieFormInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                await this.movieService.AddMovieAsync(inputModel);
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                this.ModelState.AddModelError(string.Empty, ServiceCreateError);
                return this.View(inputModel);
            }


        }
        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                MovieDetailsViewModel? movieDetails = await this.movieService.GetMovieDetailsByIdAsync(id);
                if (movieDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }
                return this.View(movieDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            try
            {
                MovieFormInputModel? editableMovie = await this.movieService.GetEditableMovieForEditByIdAsync(id);
                if (editableMovie == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }
                return this.View(editableMovie);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(MovieFormInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }
            try
            {
                bool editSucces = await this.movieService.EditMovieAsync(inputModel);
                if (!editSucces)
                {
                    return this.RedirectToAction(nameof(Details));
                }
                return this.RedirectToAction(nameof(Details), new { id = inputModel.Id });

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }

        }
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            try
            {
                MovieDetailsViewModel? movieDetails = await this.movieService.GetMovieDetailsByIdAsync(id);
                if (movieDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }
                return this.View(movieDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(MovieDetailsViewModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                bool deleteResult = await this.movieService.DeleteMovieAsync(inputModel.Id);
                if (!deleteResult)
                {
                    return this.RedirectToAction(nameof(Index));
                }
                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }


    }
}
