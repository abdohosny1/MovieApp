using Microsoft.EntityFrameworkCore;
using MovieApp.BusinessLayer.interfaces;
using MovieApp.BusinessLayer.Model;
using MovieApp.DataAcessLayer.EntityBaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DataAcessLayer.services
{
    public class GenreServices : EntityBaseRepository<Genre>, IGenreServices
    {
        private readonly ApplicationDbContext _Context;

        public GenreServices(ApplicationDbContext context) : base(context)
        {
            _Context = context;
        }



        async Task<List<Movie>> IGenreServices.GetAllMovieByGenreName(int genreId)
        {
            var data = await _Context.Movies
                                            .Include(e => e.Genre)
                                            .Where(e => e.GenreId == genreId)
                                            .OrderByDescending(e => e.Rate)
                                            .ToListAsync();
            return data;
        }
    }
}
