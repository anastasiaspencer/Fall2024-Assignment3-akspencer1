using System;
using Fall2024_Assignment3_akspencer1.Data;
using Fall2024_Assignment3_akspencer1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fall2024_Assignment3_akspencer1.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context; //Represents the session with the database and is used to
        //interact with the data

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }



        //CRUD Operations:
        //Create: Add new records to the database
        //Read: retrieve data from the db
        //Update: Modifies existing records in the database
        //
        //

       

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }

        public IActionResult AddActors()
        {
            return View();
        }

        public IActionResult Details(string title)
        {
            // Find the movie by title
            var movie = _context.Movies
                .Include(m => m.Actors)
                .FirstOrDefault(m => m.Title == title);
         

            if (movie == null)
            {
                return NotFound(); // Return a 404 if the movie is not found
            }

            return View(movie); // Pass the found movie to the view
        }


        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies
                .Include(m => m.Actors)
                .ToListAsync();

            return View(movies);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(string title)
        {
            // Find the movie by title
            var movie = await _context.Movies.FindAsync(title);

            if (movie == null)
            {
                // Handle the case where the movie doesn't exist
                return NotFound();
            }

            // Remove the movie from the database
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            // Redirect to the Index view after deletion
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit()
        {
            return View(null); // Render the view with no model initially
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
                return View("Edit", null); // Show the title input form again
            }

            // Movie found, pass the movie object to the view for editing
            return View("Edit", movie);
        }

        // POST: Movies/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Movie movie)
        {
            if (!MovieExists(movie.Title))
            {
                return NotFound(); // Return not found if the movie does not exist
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Title))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(movie); // Return the view with the movie details if validation fails
        }



        // Check if the movie exists
        private bool MovieExists(string title)
        {
            return _context.Movies.Any(e => e.Title == title);
        }


        // POST: Movies/Edit/Find
        [HttpPost]
        public async Task<IActionResult> FindMovieforActor(string Title)
        {
            // Find the movie by title
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Title == Title);

            if (movie == null)
            {
                // Movie not found, render the view with a null model
                return View("AddActors", null); // Show the title input form again
            }

            // Movie found, pass the movie object to the view for editing
            return View("AddActors", movie);
        }
        // POST: Movies/AddActor
        [HttpPost]
        public async Task<IActionResult> AddActor(string title, string actorName)
        {
            // Find the movie by title
            var movie = await _context.Movies.Include(m => m.Actors).FirstOrDefaultAsync(m => m.Title == title);

            if (movie == null)
            {
                return View("AddActorNotFound"); // Movie not found
            }

            // Check if actor already exists, if not create new actor
            var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Name == actorName);

            if (actor == null)
            {
                actor = new Actor { Name = actorName };
                _context.Actors.Add(actor);
                await _context.SaveChangesAsync();
            }

            // Add the actor to the movie if not already added
            if (!movie.Actors.Contains(actor))
            {
                movie.Actors.Add(actor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index)); // Redirect after adding
        }


        [HttpPost]
        public async Task<IActionResult> DeleteActor(string title, string actorNameToDelete)
        {
            // Find the movie by title, including its actors
            var movie = await _context.Movies.Include(m => m.Actors).FirstOrDefaultAsync(m => m.Title == title);

            if (movie == null)
            {
                return View("AddActorNotFound"); // Movie not found
            }

            // Find the actor in the movie's actor list
            var actor = movie.Actors.FirstOrDefault(a => a.Name == actorNameToDelete);

            if (actor != null)
            {
                // Remove the actor from the movie's actor list
                movie.Actors.Remove(actor);
                await _context.SaveChangesAsync(); // Save changes to the database
            }
            else
            {
                // Optionally handle the case where the actor was not found
                ModelState.AddModelError(string.Empty, "Actor not found in the movie's actor list.");
            }
            return RedirectToAction(nameof(Index)); // Redirect after deletion
        }
    }
}

  


