using MovieApp.Data;
using MovieApp.Data.Base;
using MovieApp.Models;

namespace MovieApp.services.Movies
{
    public class MovieService : EntityBaseRepository<Movie>, IMovieService
    {
        public MovieService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
