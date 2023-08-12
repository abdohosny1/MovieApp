using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MovieApp.BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DataAcessLayer.SeedData
{
    public class ApplicatioContextSeeding
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                try
                {
                    if (!context.Genres.Any())
                    {
                        context.Genres.AddRange(new List<Genre>()
                            {
                                new Genre()
                            {
                                Name = "Adventure",
                            },
                               new Genre()
                            {
                                Name = "Affairs",
                            },
                               new Genre()
                            {
                                Name = "Fantasy",
                            },
                              new Genre()
                            {
                                Name = "History",
                            },
                             new Genre()
                            {
                                Name = "Transport",
                            },
                             new Genre()
                            {
                                Name = "Puzzles",
                            },

                            });



                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    //var logger = loggerFactory.CreateLogger<ApplicatioContextSeeding>();
                    //logger.LogError(ex.Message);
                }
            }
        }
    }
}
