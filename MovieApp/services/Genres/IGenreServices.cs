using MovieApp.Data.Base;
using MovieApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieApp.services.Genres
{
    public interface IGenreServices : IEntityBaseRepository<Genre>
    {

        Task<List<Movie>> GetAllMovieByGenreName(int genreId);
    }
}
