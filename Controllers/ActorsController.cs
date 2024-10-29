using System;
using Fall2024_Assignment3_akspencer1.Data;
using Fall2024_Assignment3_akspencer1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fall2024_Assignment3_akspencer1.Controllers
{
    public class ActorsController : Controller
    {
        private readonly ApplicationDbContext _context; //Represents the session with the database and is used to
        //interact with the data

        public ActorsController(ApplicationDbContext context)
        {
            _context = context;
        }

    

        public async Task<IActionResult> Index()
        {
            var actors = await _context.Actors
                .Include(a => a.Movies)
                .ToListAsync();

            return View(actors);
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult AddMovies()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(Actor actor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);

        }

        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string name)
        {
            // Find the movie by title
            var actor = await _context.Actors.FindAsync(name);

            if (name == null)
            {
                // Handle the case where the movie doesn't exist
                return NotFound();
            }

            // Remove the movie from the database
            _context.Actors.Remove(actor);
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
        public async Task<IActionResult> FindActor(string Name)
        {
            // Find the movie by title
            var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Name == Name);

            if (actor == null)
            {
                // Movie not found, render the view with a null model
                return View("Edit", null); // Show the title input form again
            }

            // Movie found, pass the movie object to the view for editing
            return View("Edit", actor);
        }

        // POST: Movies/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Actor actor)
        {
            if (!ActorExists(actor.Name))
            {
                return NotFound(); // Return not found if the movie does not exist
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.Name))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(actor); // Return the view with the movie details if validation fails
        }



        // Check if the movie exists
        private bool ActorExists(string name)
        {
            return _context.Actors.Any(e => e.Name == name);
        }

        // POST: Movies/Edit/Find
        [HttpPost]
        public async Task<IActionResult> FindActorforMovie(string Name)
        {
            // Find the movie by title
            var actor = await _context.Actors.FirstOrDefaultAsync(m => m.Name == Name);

            if (actor == null)
            {
                // Movie not found, render the view with a null model
                return View("AddMovies", null); // Show the title input form again
            }

            // Movie found, pass the movie object to the view for editing
            return View("AddMovies", actor);
        }
        // POST: Movies/AddActor
        [HttpPost]
        public async Task<IActionResult> AddMovie(string name, string movieTitle)
        {
            // Find the actor by name, including their movies
            var actor = await _context.Actors.Include(a => a.Movies).FirstOrDefaultAsync(a => a.Name == name);

            if (actor == null)
            {
                return View("AddMovieNotFound"); // Actor not found
            }

            // Check if the movie already exists
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Title == movieTitle);

            if (movie == null)
            {
                // Create a new movie instance
                movie = new Movie { Title = movieTitle };

                // Add the new movie to the context
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync(); // Save to persist the new movie
            }

            // Add the movie to the actor's list of movies if not already added
            if (!actor.Movies.Contains(movie))
            {
                actor.Movies.Add(movie);
                await _context.SaveChangesAsync(); // Save changes to the actor's movie list
            }

            return RedirectToAction(nameof(Index)); // Redirect after adding
        }



        [HttpPost]
        public async Task<IActionResult> DeleteMovie(string name, string movieTitleToDelete)
        {
            // Find the movie by title, including its actors
            var actor = await _context.Actors.Include(m => m.Movies).FirstOrDefaultAsync(m => m.Name == name);

            if (actor == null)
            {
                return View("AddMovieNotFound"); // Movie not found
            }

            // Find the actor in the movie's actor list
            var movie = actor.Movies.FirstOrDefault(a => a.Title == movieTitleToDelete);

            if (movie != null)
            {
                // Remove the actor from the movie's actor list
                actor.Movies.Remove(movie);
                await _context.SaveChangesAsync(); // Save changes to the database
            }
            else
            {
             //handle the case where the actor was not found
                ModelState.AddModelError(string.Empty, "Movie not found in the actors movie list.");
            }

            return RedirectToAction(nameof(Index)); // Redirect after deletion
        }


    }

}

