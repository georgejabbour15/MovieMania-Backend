using CurrentUserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MovieMania.Contracts.MovieService;
using MovieMania.Data.DataContext;
using MovieMania.Infrastructure;
using MovieMania.Infrastructure.Domains;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieService
{
    public class MovieService:IMovieService
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        public MovieService(UserManager<User> userManager,
            IConfiguration configuration,
            ApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _currentUserService = currentUserService;
        }

        public ResultModel<List<Genre>> GetGenres()
        {
            List<Genre> genres = _context.Genres.ToList();
            return new ResultModel<List<Genre>>(genres, true, message: "All Genres");
        }
        public ResultModel<List<Country>> GetCountries()
        {
           
            List<Country> countries = _context.Countries.ToList();
            return new ResultModel<List<Country>>(countries, true, message: "All Countries");
        }
        public async Task<ResultModel<List<GetAllMoviesResponse>>> GetAllMovies(int start,int end)
        {
            var movies = _context.Movies.Select(m => new GetAllMoviesResponse
            {
                Id = m.Id,
                Name = m.Name,
                Descritpion = m.Descritpion,
                ReleaseDate = m.ReleaseDate,
                IMBDLink = m.IMBDLink,
                IMBDId = m.IMBDId,
                ThumbnailUrl = m.ThumbnailUrl,
                Genres = m.MovieGenres.Select(mg => mg.Genre).ToList(),
                Countries = m.MovieCountries.Select(mc => mc.Country).ToList(),
            }).Skip(start).Take(end).ToList();


            return new ResultModel<List<GetAllMoviesResponse>>(movies, true, message: "All Movies ");

        }
        public async Task<ResultModel<List<GetAllMoviesResponse>>> GetFilteredMovies(FilterMoviesRequest filterMoviesRequest)
        {
            var Ids = _context.MovieGenres.Where(mg => filterMoviesRequest.Genres.Contains(mg.GenreId)).Select(mg => mg.MovieId).ToList();
            Ids.AddRange(_context.MovieCountries.Where(mg => filterMoviesRequest.Countries.Contains(mg.CountryId)).Select(mg => mg.MovieId).ToList());

            var movies = _context.Movies.Where(m => Ids.Distinct().ToList().Contains(m.Id)).Select(m=>new GetAllMoviesResponse {
                Id = m.Id,
                Name = m.Name,
                Descritpion = m.Descritpion,
                ReleaseDate = m.ReleaseDate,
                IMBDLink = m.IMBDLink,
                IMBDId = m.IMBDId,
                ThumbnailUrl = m.ThumbnailUrl,
                Genres = m.MovieGenres.Select(mc => mc.Genre).ToList(),
                Countries = m.MovieCountries.Select(mc => mc.Country).ToList(),
            }).ToList();
            return new ResultModel<List<GetAllMoviesResponse>>(movies, true, message: "Filtered Movies ");
        }
        public async Task<ResultModel<List<GetAllMoviesResponse>>> GetUserRatedMovies()
        {
            var data = await _currentUserService.GetCurrentUser();
            User user = data.Data.User;

            if (user is null)
                return new ResultModel<List<GetAllMoviesResponse>>(null, false, StatusCodes.Status400BadRequest, "User doesn't exist");

            var movies = _context.Ratings.Where(r=>r.UserId==user.Id).Select(r => new GetAllMoviesResponse
            {
                Id =r.MovieId,
                Rating=r.Rate,
                Name = r.Movie.Name,
                Descritpion = r.Movie.Descritpion,
                ReleaseDate = r.Movie.ReleaseDate,
                IMBDLink = r.Movie.IMBDLink,
                IMBDId = r.Movie.IMBDId,
                ThumbnailUrl = r.Movie.ThumbnailUrl,
                Genres = r.Movie.MovieGenres.Select(mg => mg.Genre).ToList(),
                Countries = r.Movie.MovieCountries.Select(mc => mc.Country).ToList(),
            }).ToList();


            return new ResultModel<List<GetAllMoviesResponse>>(movies, true, message: "All Movies ");

        }
        public async Task<ResultModel<GetAllMoviesResponse>> GetMovieById(string movieId)
        {
            var data = await _currentUserService.GetCurrentUser();
            User user = data.Data.User;

            if (user is null)
                return new ResultModel<GetAllMoviesResponse>(null, false, StatusCodes.Status400BadRequest, "User doesn't exist");

            var movies = _context.Movies.Where(r => r.Id == movieId).Select(r => new GetAllMoviesResponse
            {
                Id = r.Id,
                Name = r.Name,
                Descritpion = r.Descritpion,
                ReleaseDate = r.ReleaseDate,
                IMBDLink = r.IMBDLink,
                IMBDId = r.IMBDId,
                Rating = r.Ratings.Where(m=>m.UserId==user.Id).Select(r => r.Rate).FirstOrDefault(),
                ThumbnailUrl = r.ThumbnailUrl,
                Genres = r.MovieGenres.Select(mg => mg.Genre).ToList(),
                Countries = r.MovieCountries.Select(mc => mc.Country).ToList(),
            }).FirstOrDefault();

            return new ResultModel<GetAllMoviesResponse>(movies, true, message: "All Movies ");
        }
        public async Task<ResultModel<List<GetAllMoviesResponse>>> GetMovieByName(string movieName)
        {
            var data = await _currentUserService.GetCurrentUser();
            User user = data.Data.User;

            if (user is null)
                return new ResultModel<List<GetAllMoviesResponse>>(null, false, StatusCodes.Status400BadRequest, "User doesn't exist");

            var movies = _context.Movies.Where(m => m.Name.Contains(movieName.ToLower())).Select(m => new GetAllMoviesResponse
            {
                Id = m.Id,
                Name = m.Name,
                Descritpion = m.Descritpion,
                ReleaseDate = m.ReleaseDate,
                IMBDLink = m.IMBDLink,
                IMBDId = m.IMBDId,
                ThumbnailUrl = m.ThumbnailUrl,
                Genres = m.MovieGenres.Select(mg => mg.Genre).ToList(),
                Countries = m.MovieCountries.Select(mc => mc.Country).ToList(),
            }).ToList();

            return new ResultModel<List<GetAllMoviesResponse>>(movies, true, message: "All Movies ");

        }
        public async Task<ResultModel<IActionResult>> RateMovie(string movieId,int rating) {
            var data = await _currentUserService.GetCurrentUser();
            User user = data.Data.User;

            if (user is null)
                return new ResultModel<IActionResult>(null, false, StatusCodes.Status400BadRequest, "User doesn't exist");

            Rating r =  _context.Ratings.Where(r => r.UserId == user.Id && r.MovieId == movieId).FirstOrDefault();
            if (r != null) {
                r.Rate = rating;
                _context.Ratings.Update(r);
            }
            else
            {
                Rating rate = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Rate = rating,
                    MovieId = movieId,
                    UserId = user.Id,
                };
                _context.Ratings.Add(rate);
            }
            var result = await _context.SaveChangesAsync();
            if (result < 0)
                return new ResultModel<IActionResult>(null, false, StatusCodes.Status500InternalServerError, "Internal server error");
            return new ResultModel<IActionResult>(null, true, message: "Rating succeeded");
        }
        public async Task<ResultModel<IActionResult>> DeleteUserRating(string movieId) {
            var data = await _currentUserService.GetCurrentUser();
            User user = data.Data.User;

            if (user is null)
                return new ResultModel<IActionResult>(null, false, StatusCodes.Status400BadRequest, "User doesn't exist");

            Rating rating = _context.Ratings.Where(r => r.UserId == user.Id && r.MovieId == movieId).FirstOrDefault();

            _context.Ratings.Remove(rating);
            var result = await _context.SaveChangesAsync();
            if (result < 0)
                return new ResultModel<IActionResult>(null, false, StatusCodes.Status500InternalServerError, "Internal server error");
            return new ResultModel<IActionResult>(null, true, message: "Rating removes Successfully");

        }

    }
}
