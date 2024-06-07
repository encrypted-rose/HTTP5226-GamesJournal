using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GamesJournal.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        public string Username { get; set; }
        public DateTime UserCreationDate { get; set; }

        //A Player can play many games
        public ICollection<Game> Games { get; set; }
    }
}