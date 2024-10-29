using System;
namespace Fall2024_Assignment3_akspencer1.Models
{
	public class MovieReviews
	{

        public string MovieTitle { get; set; }
        public List<string> Reviews { get; set; }
        public List<string> Tweets { get; set; }
        public string ActorName { get; set; }
        public string Sentiment { get; set; }

        public MovieReviews()
		{
		}
	}
}

