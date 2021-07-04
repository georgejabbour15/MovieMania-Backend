using Microsoft.AspNetCore.Mvc;
using MovieMania.Contracts.MovieService;
using MovieMania.Infrastructure;
using MovieMania.Infrastructure.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieService
{
    public interface IMovieService
    {
        ResultModel<List<Genre>> GetGenres();
        ResultModel<List<Country>> GetCountries();
        Task<ResultModel<List<GetAllMoviesResponse>>> GetAllMovies(int start, int end);
        Task<ResultModel<List<GetAllMoviesResponse>>> GetFilteredMovies(FilterMoviesRequest filterMoviesRequest);
        Task<ResultModel<List<GetAllMoviesResponse>>> GetUserRatedMovies ();
        Task<ResultModel<GetAllMoviesResponse>> GetMovieById (string movieId);
        Task<ResultModel<List<GetAllMoviesResponse>>> GetMovieByName(string movieName);
        Task<ResultModel<IActionResult>> RateMovie(string movieId,int rating);
        Task<ResultModel<IActionResult>> DeleteUserRating(string movieId);
    }  
}
