using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GamesJournal.Models;
using System.Diagnostics;

namespace GamesJournal.Controllers
{

    public class GameDataController : ApiController
    {
        /// <summary>
        /// Returns all the games in the System
        /// </summary>
        /// <return>
        /// HEADERL 200 (OK)
        /// CONTENT: All the games in the database
        /// </return>
        /// <example>
        /// GET: api/GameData/ListGames
        /// </example>

        private ApplicationDbContext db = new ApplicationDbContext();
        [HttpGet]
        public IEnumerable<Game> ListGames()
         {
            return db.Games;
        }


        /// <summary>
        /// Returns a specific game, by Game ID
        /// </summary>
        /// <return>
        /// CONTENT: A game and the information of the game in the system matching the ID provided.
        /// </return>
        /// <param name="id">Game ID Primary Key</param>
        /// <example>
        /// GET: api/GameData/FindGame/7
        /// </example>

        [ResponseType(typeof(Game))]
        [HttpGet]
        public IHttpActionResult FindGame(int id)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        /// <summary>
        /// Updates a game in the system with the POST Data input
        /// </summary>
        /// <param name="id">Represents the game ID  primary key</param>
        /// <param name="game">JSON form Data of a game</param>
        /// <return>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </return>
        /// <example>
        /// POST: api/GameData/UpdateGame/7
        /// FORM DATA: Game JSON Object
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateGame(int id, Game game)
        {
            Debug.WriteLine("I have reached the game update method");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model stat is invalid");
                return BadRequest(ModelState);
            }

            if (id != game.GameId)
            {
                Debug.WriteLine("Incorrect ID");
                Debug.WriteLine("GET parameter"+id);
                return BadRequest();
            }

            db.Entry(game).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    Debug.WriteLine("Game not located");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Debug.WriteLine("No condition triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a new game into the system
        /// </summary>
        /// <param name="game">JSON form Data of a game</param>
        /// <return>
        /// CONTENT: Game ID, Game Data
        /// or 
        /// HEADER: 400 (Bad Request)
        /// </return>
        /// <example>
        /// POST: api/GameData/AddGame
        /// FORM DATA: Game JSON Object
        /// </example>

        [ResponseType(typeof(Game))]
        [HttpPost]
        public IHttpActionResult AddGame(Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Games.Add(game);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = game.GameId }, game);
        }

        /// <summary>
        /// Deletes a game from the system by it's ID
        /// </summary>
        /// <param name="id">JSON form Data of a game</param>
        /// <return>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </return>
        /// <example>
        /// POST: api/GameData/DeleteGame/5
        /// FORM DATA: (empty)
        /// </example>

        [ResponseType(typeof(Game))]
        [HttpPost]
        public IHttpActionResult DeleteGame(int id)
        {
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return NotFound();
            }

            db.Games.Remove(game);
            db.SaveChanges();

            return Ok(game);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GameExists(int id)
        {
            return db.Games.Count(e => e.GameId == id) > 0;
        }
    }
}