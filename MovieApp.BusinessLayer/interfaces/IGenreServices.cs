using MovieApp.BusinessLayer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieApp.BusinessLayer.interfaces
{
    public interface IGenreServices : IEntityBaseRepository<Genre>
    {

        Task<List<Movie>> GetAllMovieByGenreName(int genreId);
    }
}

