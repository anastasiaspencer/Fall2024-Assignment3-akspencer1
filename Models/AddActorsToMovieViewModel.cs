namespace Fall2024_Assignment3_akspencer1.Models
{
    public class AddActorsToMovieViewModel
    {
        public Movie Movie { get; set; } // The movie to which actors will be added
        public List<Actor> AvailableActors { get; set; } // List of available actors

        public AddActorsToMovieViewModel()
        {
            Movie = new Movie(); // Initialize the movie
            AvailableActors = new List<Actor>(); // Initialize the list of actors
        }
    }
}
