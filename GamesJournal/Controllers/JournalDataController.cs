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
using GamesJournal.Migrations;
using GamesJournal.Models;

namespace GamesJournal.Controllers
{
    public class JournalDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Returns all the journal entries in the system
        /// </summary>
        /// <return>
        /// HEADERL 200 (OK)
        /// CONTENT: All the journals entries in the database
        /// </return>
        /// <example>
        /// GET: api/JournalData/ListJournalsEntries
        /// </example>

        [HttpGet]
        public IEnumerable<JournalEntryDto> ListJournalsEntries()

        {
            List<Journal> JournalsEntries = db.Journals.ToList();
            List<JournalEntryDto> JournalEntryDtos = new List<JournalEntryDto>();

            JournalsEntries.ForEach(a => JournalEntryDtos.Add(new JournalEntryDto(){
                JournalEntryId = a.JournalEntryId,
                JournalEntryTitle = a.JournalEntryTitle,
                JournalEntry = a.JournalEntry,
                EntryDate = a.EntryDate,
                Username = a.Player.Username,
                GameTitle = a.Game.GameTitle
            }));

            return JournalEntryDtos;
        }

        /// <summary>
        /// Returns a specific Journal Entry, by Entry ID
        /// </summary>
        /// <return>
        /// CONTENT: A Journal Entry and the information of the Player in the system matching the ID provided.
        /// </return>
        /// <param name="id">Entry ID Primary Key</param>
        /// <example>
        /// GET: api/JournalData/FindJournalEntry/2
        /// </example>

        [ResponseType(typeof(Journal))]
        [HttpGet]
        public IHttpActionResult FindJournalEntry(int id)
        {
            Journal Journal = db.Journals.Find(id);
            JournalEntryDto journalEntryDto = new JournalEntryDto()
            {
                JournalEntryId = Journal.JournalEntryId,
                JournalEntryTitle = Journal.JournalEntryTitle,
                JournalEntry = Journal.JournalEntry,
                EntryDate = Journal.EntryDate,
                Username = Journal.Player.Username,
                GameTitle = Journal.Game.GameTitle
            };

            if (Journal == null)
            {
                return NotFound();
            }

            return Ok(journalEntryDto);
        }

        /// <summary>
        /// Updates a journal entry in the system with the POST Data input
        /// </summary>
        /// <param name="id">Represents the journal entry ID primary key</param>
        /// <param name="journal">JSON form Data of a journal entry</param>
        /// <return>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </return>
        /// <example>
        /// POST: api/JournalData/UpdateJournal/5
        /// FORM DATA: journal JSON Object
        /// </example>

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateJournal(int id, Journal journal)
        {
            Debug.WriteLine("I have reached the journal update method");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != journal.JournalEntryId)
            {
                Debug.WriteLine("Incorrect ID");
                Debug.WriteLine("GET parameter" + id);
                return BadRequest();
            }

            db.Entry(journal).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JournalExists(id))
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
        /// Adds a new Journal Entry into the system
        /// </summary>
        /// <param name="journal">JSON form Data of a Journal Entry</param>
        /// <return>
        /// CONTENT: journalEntry ID, Journal Data
        /// or 
        /// HEADER: 400 (Bad Request)
        /// </return>
        /// <example>
        /// POST: api/JournalData/AddJournal
        /// FORM DATA: journal JSON Object
        /// </example>

        [ResponseType(typeof(Journal))]
        [HttpPost]
        public IHttpActionResult AddJournal(Journal journal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Journals.Add(journal);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = journal.JournalEntryId }, journal);
        }

        /// <summary>
        /// Deletes a journal entry from the system by it's ID
        /// </summary>
        /// <param name="id">JSON form Data of a Journal Entry</param>
        /// <return>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </return>
        /// <example>
        /// POST: api/JournalData/DeleteJournalEntry/4
        /// FORM DATA: (empty)
        /// </example>

        [ResponseType(typeof(Journal))]
        [HttpPost]
        public IHttpActionResult DeleteJournalEntry(int id)
        {
            Journal journal = db.Journals.Find(id);
            if (journal == null)
            {
                return NotFound();
            }

            db.Journals.Remove(journal);
            db.SaveChanges();

            return Ok(journal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JournalExists(int id)
        {
            return db.Journals.Count(e => e.JournalEntryId == id) > 0;
        }
    }
}