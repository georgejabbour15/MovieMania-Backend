
namespace MovieMania.Contracts.Routes
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v{version:apiVersion}";
        public const string Base = Root + "/" + Version;

        public static class Auth {
            public const string AuthBase = Base + "/auth";
            public const string Login = AuthBase + "/login";
            public const string Register = AuthBase + "/register";
            public const string RefreshToken = AuthBase + "/refresh-token";
            public const string Logout = AuthBase + "/logout";
        }
        public static class Movie {
            public const string AuthBase = Base + "/movie";
            public const string GetGenre = AuthBase + "/get-genre";
            public const string GetCountry = AuthBase + "/get-country";
            public const string GetAllMovies = AuthBase + "/get-all-movies";
            public const string Rate = AuthBase + "/{movieId}" + "/rate";
            public const string GetUserRatedMovies = AuthBase + "/get-user-rated-movies";
            public const string GetMovieById = AuthBase + "/{movieId}";
            public const string GetMovieByName = AuthBase + "/get-movie-by-name";
            public const string GetFilteredMovies = AuthBase + "/get-filtered-movies";
            public const string DeleteUserRating = AuthBase +"/{movieId}";
        }

        
    }
}
