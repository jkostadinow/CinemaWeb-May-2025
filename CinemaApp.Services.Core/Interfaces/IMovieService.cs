﻿using CinemaApp.Web.ViewModels.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Core.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<AllMoviesIndexViewModel>> GetAllMoviesAsync();

        Task AddMovieAsync(MovieFormInputModel inputModel);

        Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(string id);

        Task<MovieFormInputModel> GetMovieForEditByIdAsync(string id);
    }
}
