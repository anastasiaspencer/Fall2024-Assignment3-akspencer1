using System;
using System.Net.Http.Headers;
using System.Text;
using Fall2024_Assignment3_akspencer1.Data;
using Fall2024_Assignment3_akspencer1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using VaderSharp2;
using Azure.AI.OpenAI;
namespace Fall2024_Assignment3_akspencer1.Controllers
{
	public class AIController : Controller
	{
        private readonly ApplicationDbContext _context; //Represents the session with the database and is used to
        //interact with the data

        private readonly string apiUrl = "https://fall2024-assignment3-json10-openai.openai.azure.com/openai/deployments/gpt-35-turbo/chat/completions?api-version=2024-08-01-preview";
        private readonly string apiKey = "01837e74cf2a4eb08a3291b1a3732c34";
        private readonly ILogger<AIController> _logger;

    
        public AIController(ApplicationDbContext context, ILogger<AIController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GenerateReviews()
        {
            return View();
        }

        public IActionResult GenerateTweets()
        {
            return View();
        }

        // POST: Movies/Edit/Find
        [HttpPost]
        public async Task<IActionResult> FindActor(string Name)
        {
            // Find the movie by title
            var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Name == Name);

            if (actor == null)
            {
                // Movie not found, render the view with a null model
                return View("GenerateTweets", null); // Show the title input form again
            }
            else
            {
                var actorName = actor.Name;
                var tweets = new MovieReviews();
                tweets.ActorName = actorName;
                // Movie found, pass the movie object to the view for editing
                return View("GenerateTweets", tweets);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerateTweets(string Name)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(m => m.Name == Name);

            var actorTweets = new MovieReviews
            {
                ActorName = actor.Name,
                Tweets = new List<string>() // Initialize the Reviews list
            };


            var prompt = $"Write ten twitter tweets from the actor '{actorTweets.ActorName}'.";

            //Anonymous object 
            var requestBody = new
            {
                //Messages to send to openAI
                messages = new[]
            {
                //Inform system about its role 
                new { role = "system", content = "You are a twitter user." },
                //request for AI
                new { role = "user", content = prompt }
            },  //max number of words the AI can geneerate
                max_tokens = 1000
            };

            try
            {
                using var client = new HttpClient();
                //add key to header for verification
                client.DefaultRequestHeaders.Add("api-key", apiKey);
                //indicates that the client expects to recieve a response in JSON format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                //sends the post request
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Use JsonDocument to parse the response
                    using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                    {
                        // Navigate the JSON structure to get the reviews
                        var choices = doc.RootElement.GetProperty("choices");
                        if (choices.GetArrayLength() > 0)
                        {
                            var reviewsText = choices[0].GetProperty("message").GetProperty("content").GetString();
                            if (!string.IsNullOrWhiteSpace(reviewsText))
                            {
                                var newReviews = reviewsText.Split("\n")
                                                            .Where(review => !string.IsNullOrWhiteSpace(review))
                                                            .ToList();
                                actorTweets.Tweets = newReviews;
                            }
                            _logger.LogError("An error occurred while generating tweets for actor: {ActorName}", actorTweets.ActorName);
                        }
                        else
                        {
                            _logger.LogError("There were items returned", actorTweets.ActorName);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while generating reviews for movie: {MovieTitle}", movieReviews.MovieTitle);
                throw; // Re-throw if you want to handle it further up the pipeline
            }

            // Perform sentiment analysis on the reviews
            var analyzer = new SentimentIntensityAnalyzer();
            double totalScore = 0;
            foreach (var review in actorTweets.Tweets)
            {
                var scores = analyzer.PolarityScores(review);
                totalScore += scores.Compound;
            }

            // Calculate the average sentiment score
            double averageScore = totalScore / actorTweets.Tweets.Count;

            // Determine the general sentiment
            if (averageScore > 0.05)
            {
                actorTweets.Sentiment = "Positive";
            }
            else if (averageScore < -0.05)
            {
                actorTweets.Sentiment = "Negative";
            }
            else
            {
                actorTweets.Sentiment = "Neutral";
            }

            return View("GenerateTweets", actorTweets);
        }
    


// POST: Movies/Edit/Find
[HttpPost]
        public async Task<IActionResult> FindMovie(string Title)
        {
            // Find the movie by title
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Title == Title);

            if (movie == null)
            {
                // Movie not found, render the view with a null model
                return View("GenerateReviews", null); // Show the title input form again
            }
            else
            {
                var movieTitle = movie.Title;
                var reviews = new MovieReviews();
                reviews.MovieTitle = movieTitle;
                // Movie found, pass the movie title object to the view for editing
                return View("GenerateReviews", reviews);
            }
         
        }

      


        [HttpPost]
        public async Task<IActionResult> GenerateReviews(string Title)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Title == Title);

            var movieReviews = new MovieReviews
            {
                MovieTitle = movie.Title,
                Reviews = new List<string>() // Initialize the Reviews list
            };


            var prompt = $"Write ten movie reviews for the movie titled '{movieReviews.MovieTitle}'.";

            //Anonymous object 
            var requestBody = new
            {
                //Messages to send to openAI
                messages = new[]
            {
                //Inform system about its role 
                new { role = "system", content = "You are a movie reviewer." },
                //request for AI
                new { role = "user", content = prompt }
            },  //max number of words the AI can geneerate
                max_tokens = 1000
            };

            try
            {
                using var client = new HttpClient();
                //add key to header for verification
                client.DefaultRequestHeaders.Add("api-key", apiKey);
                //indicates that the client expects to recieve a response in JSON format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                //sends the post request
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Use JsonDocument to parse the response
                    using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                    {
                        // Navigate the JSON structure to get the reviews
                        var choices = doc.RootElement.GetProperty("choices");
                        if (choices.GetArrayLength() > 0)
                        {
                            var reviewsText = choices[0].GetProperty("message").GetProperty("content").GetString();
                            if (!string.IsNullOrWhiteSpace(reviewsText))
                            {
                                var newReviews = reviewsText.Split("\n")
                                                            .Where(review => !string.IsNullOrWhiteSpace(review))
                                                            .ToList();
                                movieReviews.Reviews = newReviews; 
                            }
                            _logger.LogError("An error occurred while generating reviews for movie: {MovieTitle}", movieReviews.MovieTitle);
                        }
                        else
                        {
                            _logger.LogError("There were items returned", movieReviews.MovieTitle);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating reviews for movie: {MovieTitle}", movieReviews.MovieTitle);
                throw; // Re-throw if you want to handle it further up the pipeline
            }

            // Perform sentiment analysis on the reviews
            var analyzer = new SentimentIntensityAnalyzer();
            double totalScore = 0;
            foreach (var review in movieReviews.Reviews)
            {
                var scores = analyzer.PolarityScores(review);
                totalScore += scores.Compound;
            }

            // Calculate the average sentiment score
            double averageScore = totalScore / movieReviews.Reviews.Count;

            // Determine the general sentiment
            if (averageScore > 0.05)
            {
                movieReviews.Sentiment = "Positive";
            }
            else if (averageScore < -0.05)
            {
                movieReviews.Sentiment = "Negative";
            }
            else
            {
                movieReviews.Sentiment = "Neutral";
            }

            return View("GenerateReviews", movieReviews);
        }
    }
}

