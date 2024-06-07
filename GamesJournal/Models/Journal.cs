using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace GamesJournal.Models
{
    public class Journal
    {
        [Key]
        public int JournalEntryId { get; set; }
        public string JournalEntryTitle { get; set; }
        public string JournalEntry { get; set; }
        public DateTime EntryDate { get; set; }

        //A game can have many entries of different users
        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }

        //A journal can have many entries of one game
        [ForeignKey("Game")]
        public int GameiId { get; set; }
        public virtual Game Game { get; set; }

    }

    public class JournalEntryDto
    {
        public int JournalEntryId { get; set; }
        public string JournalEntryTitle { get; set; }
        public string JournalEntry { get; set; }
        public DateTime EntryDate { get; set; }
        public string Username { get; set; }
        public string GameTitle { get; set; }

    }
}