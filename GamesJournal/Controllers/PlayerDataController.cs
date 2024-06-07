using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GamesJournal.Models;

namespace GamesJournal.Controllers
{
    public class PlayerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all the players in the system
        /// </summary>
        /// <return>
        /// HEADERL 200 (OK)
        /// CONTENT: All the players in the database
        /// </return>
        /// <example>
        /// GET: api/PlayerData/ListPlayers
        /// </example>

        [HttpGet]
        public IEnumerable<Player> ListPlayers()
        {
            return db.Player;
        }

        /// <summary>
        /// Returns a specific Player, by Player ID
        /// </summary>
        /// <return>
        /// CONTENT: A Player and the information of the Player in the system matching the ID provided.
        /// </return>
        /// <param name="id">Player ID Primary Key</param>
        /// <example>
        /// GET: api/PlayerData/FindPlayer/2
        /// </example>

        [ResponseType(typeof(Player))]
        [HttpGet]
        public IHttpActionResult FindPlayer(int id)
        {
            Player player = db.Player.Find(id);
            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        /// <summary>
        /// Updates a player in the system with the POST Data input
        /// </summary>
        /// <param name="id">Represents the player ID  primary key</param>
        /// <param name="player">JSON form Data of a game</param>
        /// <return>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </return>
        /// <example>
        /// POST: api/PlayerData/UpdatePlayer/2
        /// FORM DATA: Player JSON Object
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePlayer(int id, Player player)
        {
            Debug.WriteLine("I have reached the game update method");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != player.PlayerId)
            {
                return BadRequest();
            }

            db.Entry(player).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
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
        /// Adds a new Player into the system
        /// </summary>
        /// <param name="player">JSON form Data of a Player</param>
        /// <return>
        /// CONTENT: Player ID, Player Data
        /// or 
        /// HEADER: 400 (Bad Request)
        /// </return>
        /// <example>
        /// POST: api/PlayerData/AddPlayer
        /// FORM DATA: Player JSON Object
        /// </example>

        [ResponseType(typeof(Player))]
        [HttpPost]
        public IHttpActionResult AddPlayer(Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Player.Add(player);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = player.PlayerId }, player);
        }

        /// <summary>
        /// Deletes a player from the system by it's ID
        /// </summary>
        /// <param name="id">JSON form Data of a player</param>
        /// <return>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </return>
        /// <example>
        /// POST: api/PlayerData/DeletePlayer/4
        /// FORM DATA: (empty)
        /// </example>

        [ResponseType(typeof(Player))]
        [HttpPost]

        public IHttpActionResult DeletePlayer(int id)
        {
            Player player = db.Player.Find(id);
            if (player == null)
            {
                return NotFound();
            }

            db.Player.Remove(player);
            db.SaveChanges();

            return Ok(player);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayerExists(int id)
        {
            return db.Player.Count(e => e.PlayerId == id) > 0;
        }
    }
}