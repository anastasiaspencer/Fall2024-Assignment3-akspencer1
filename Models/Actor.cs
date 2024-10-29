using System;
namespace Fall2024_Assignment3_akspencer1.Models
{
	public class Actor
	{
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string ImdbLink { get; set; }
        public string PhotoUrl { get; set; }
        public List<Movie> Movies { get; set; }

        public Actor()
		{
            Movies = new List<Movie>();
        }
	}
}

