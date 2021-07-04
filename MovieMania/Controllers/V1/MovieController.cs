using Microsoft.AspNetCore.Mvc;
using MovieMania.Contracts.Base;
using MovieMania.Contracts.MovieService;
using MovieMania.Contracts.Routes;
using MovieService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieMania.Controllers.V1
{
    [ApiController]
    [ApiVersion("1")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }


        [HttpGet(ApiRoutes.Movie.GetGenre)]
        public IActionResult GetGenres()
        {
            var result = _movieService.GetGenres();
            return result.Success ? Ok(new ApiOkResponse(result.Data)) : BadRequest(new ApiErrorResponse(result.Message));
        }

        [HttpGet(ApiRoutes.Movie.GetCountry)]
        public IActionResult GetCountries()
        {
            var result = _movieService.GetCountries();
            return result.Success ? Ok(new ApiOkResponse(result.Data)) : BadRequest(new ApiErrorResponse(result.Message));
        }
        [HttpGet(ApiRoutes.Movie.GetAllMovies)]
        public async Task<IActionResult> GetAllMovies(int start,int end)
        {
            var result = await _movieService.GetAllMovies(start, end);
            return result.Success ? Ok(new ApiOkResponse(result.Data)) : BadRequest(new ApiErrorResponse(result.Message));
        }

        [HttpGet(ApiRoutes.Movie.GetUserRatedMovies)]
        public async Task<IActionResult> GetUserRatedMovies()
        {
            var result = await _movieService.GetUserRatedMovies();
            return result.Success ? Ok(new ApiOkResponse(result.Data)) : BadRequest(new ApiErrorResponse(result.Message));
        }

        [HttpGet(ApiRoutes.Movie.GetMovieById)]
        public async Task<IActionResult> GetMovieById([FromRoute]string movieId)
        {
            var result = await _movieService.GetMovieById(movieId);
            return result.Success ? Ok(new ApiOkResponse(result.Data)) : BadRequest(new ApiErrorResponse(result.Message));
        }

        [HttpGet(ApiRoutes.Movie.GetMovieByName)]
        public async Task<IActionResult> GetMovieByName(string movieName)
        {
            var result = await _movieService.GetMovieByName(movieName);
            return result.Success ? Ok(new ApiOkResponse(result.Data)) : BadRequest(new ApiErrorResponse(result.Message));
        }

        [HttpPost(ApiRoutes.Movie.Rate)]
        public async Task<IActionResult> RateMovie([FromRoute]string movieId, [FromBody] int rating)
        {
            var result = await _movieService.RateMovie(movieId,rating);
            return result.Success ? Ok(new ApiOkResponse(result.Data)) : BadRequest(new ApiErrorResponse(result.Message));
        }

        [HttpPost(ApiRoutes.Movie.GetFilteredMovies)]
        public async Task<IActionResult> GetFilteredMovies([FromBody] FilterMoviesRequest filterMoviesRequest)
        {
            var result = await _movieService.GetFilteredMovies(filterMoviesRequest);
            return result.Success ? Ok(new ApiOkResponse(result.Data)) : BadRequest(new ApiErrorResponse(result.Message));
        }

        [HttpDelete(ApiRoutes.Movie.DeleteUserRating)]
        public async Task<IActionResult> DeleteUserRating([FromRoute] string movieId)
        {
            var result = await _movieService.DeleteUserRating(movieId);
            return result.Success ? Ok(new ApiOkResponse(result.Data)) : BadRequest(new ApiErrorResponse(result.Message));
        }
    }
}
