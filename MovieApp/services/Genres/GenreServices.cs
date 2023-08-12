using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.Data.Base;
using MovieApp.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.services.Genres
{
    public class GenreServices : EntityBaseRepository<Genre>, IGenreServices
    {
        private readonly ApplicationDbContext _Context;

        public GenreServices(ApplicationDbContext context) : base(context) {
            _Context = context;
        }

    

       async Task<List<Movie>> IGenreServices.GetAllMovieByGenreName(int genreId)
        {
            var data =await  _Context.Movies
                                            .Include(e=>e.Genre)
                                            .Where(e => e.GenreId == genreId)
                                            .OrderByDescending(e => e.Rate)
                                            .ToListAsync();
            return data;
        }
    }
}
