using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace GamesJournal.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public string GameTitle { get; set; }
        public string GamePublisher { get; set; }
        
        //Rating of 0 to 10
        public int GameRating { get; set; }
        public string GamePlot { get; set; }

        //A game can have many players
        public ICollection<Player> Players { get; set; }
    }
}