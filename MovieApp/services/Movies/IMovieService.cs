using MovieApp.Data.Base;
using MovieApp.Models;

namespace MovieApp.services.Movies
{
    public interface IMovieService : IEntityBaseRepository<Movie>
    {
    }
}
