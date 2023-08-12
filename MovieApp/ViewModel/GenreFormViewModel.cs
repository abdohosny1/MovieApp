using System.ComponentModel.DataAnnotations;

namespace MovieApp.ViewModel
{
    public class GenreFormViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}
