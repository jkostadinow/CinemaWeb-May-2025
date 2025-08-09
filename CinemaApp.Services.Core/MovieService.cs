

using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Globalization;
using static CinemaApp.GCommon.ApplicationConstants;
namespace CinemaApp.Services.Core
{
    public class MovieService : IMovieService
    {
        private readonly CinemaAppDbContext dbContext;
        public MovieService(CinemaAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddMovieAsync(MovieFormInputModel inputModel)
        {
            Movie newMovie = new Movie()
            {
                Title = inputModel.Title,
                Genre = inputModel.Genre,
                ReleaseDate = DateOnly.ParseExact(inputModel.ReleaseDate, AppDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None),
                Duration = inputModel.Duration,
                Director = inputModel.Director,
                Description = inputModel.Description,
                ImageUrl = inputModel.ImageUrl ?? $"~{NoImageUrl}"
            };
            await this.dbContext.Movies.AddAsync(newMovie);

        }

        public async Task<bool> EditMovieAsync(MovieFormInputModel inputModel)
        {
            Movie? editableMovie = await this.dbContext
                .Movies
                .SingleOrDefaultAsync(m => m.Id.ToString() == inputModel.Id);
            if (editableMovie == null)
            {
                return false;
            }

           DateOnly movieReleaseDate = DateOnly
                .ParseExact(inputModel.ReleaseDate, AppDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None);
            
            editableMovie.Title = inputModel.Title;
            editableMovie.Genre = inputModel.Genre;
            editableMovie.ReleaseDate = movieReleaseDate;
            editableMovie.Duration = inputModel.Duration;
            editableMovie.Director = inputModel.Director;
            editableMovie.Description = inputModel.Description;
            editableMovie.ImageUrl = inputModel.ImageUrl ?? $"~{NoImageUrl}";
            
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync()
        {
            IEnumerable<AllMoviesIndexViewModel> allMovies = await this.dbContext.Movies
                .AsNoTracking()
                .Select(m => new AllMoviesIndexViewModel
                {
                    Id = m.Id.ToString(),
                    Title = m.Title,
                    Genre = m.Genre,
                    ReleaseDate = m.ReleaseDate.ToString(AppDateFormat),
                    Director = m.Director,
                    ImageUrl = m.ImageUrl
                })
                .ToListAsync();
            foreach (AllMoviesIndexViewModel movie in allMovies)
            {
                if (String.IsNullOrEmpty(movie.ImageUrl))
                {
                    movie.ImageUrl = $"~{NoImageUrl}.jpg";
                }
            }

            return allMovies;
        }

        public async Task<MovieFormInputModel?> GetEditableMovieForEditByIdAsync(string? id)
        {
            MovieFormInputModel? editableMovie = null;
            if (Guid.TryParse(id, out Guid movieId))
            {
                editableMovie = await this.dbContext.Movies
                    .AsNoTracking()
                    .Where(m => m.Id == movieId)
                    .Select(m => new MovieFormInputModel
                    {
                        
                        Title = m.Title,
                        Genre = m.Genre,
                        ReleaseDate = m.ReleaseDate.ToString(AppDateFormat),
                        Director = m.Director,
                        Duration = m.Duration,
                        Description = m.Description,
                        ImageUrl = m.ImageUrl ?? $"~{NoImageUrl}"
                    })
                    .SingleOrDefaultAsync();
            }
            return editableMovie;
        }
        
        

        public async Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(string? id)
        {
            MovieDetailsViewModel? movieDetails = null;
            if (Guid.TryParse(id, out Guid movieId))
            {
                movieDetails = await this.dbContext.Movies
                    .AsNoTracking()
                    .Where(m => m.Id == movieId)
                    .Select(m => new MovieDetailsViewModel
                    {
                        Id = m.Id.ToString(),
                        Title = m.Title,
                        Genre = m.Genre,
                        ReleaseDate = m.ReleaseDate.ToString(AppDateFormat),
                        Director = m.Director,
                        Duration = m.Duration,
                        Description = m.Description,
                        ImageUrl = m.ImageUrl ?? $"~{NoImageUrl}"
                    })
                    .SingleOrDefaultAsync();
            }
            return movieDetails;
        }
    }
}
