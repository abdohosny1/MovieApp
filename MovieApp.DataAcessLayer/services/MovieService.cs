using MovieApp.BusinessLayer.interfaces;
using MovieApp.BusinessLayer.Model;
using MovieApp.DataAcessLayer.EntityBaseRepositories;

namespace MovieApp.DataAcessLayer.services
{
    public class MovieService : EntityBaseRepository<Movie>, IMovieService
    {
        public MovieService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
