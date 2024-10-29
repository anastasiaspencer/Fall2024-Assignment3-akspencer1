using System;
using System.Collections.Generic; // Required for List<T>

namespace Fall2024_Assignment3_akspencer1.Models
{
    public class Movie
    {
        public string Title { get; set; }          // Title of the movie
        public string ImdbLink { get; set; }       // Link to the movie's IMDb page
        public string Genre { get; set; }          // Genre of the movie
        public int YearOfRelease { get; set; }     // Year the movie was released
        public string PosterUrl { get; set; }      // URL for the movie poster
       public List<Actor> Actors { get; set; }    // List of actors in the movie

        public Movie()
        {
           Actors = new List<Actor>(); 
        }
    }
}
