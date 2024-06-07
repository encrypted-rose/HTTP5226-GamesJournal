using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using GamesJournal.Models;
using System.Web.Script.Serialization;
using System.Net.Http;
using GamesJournal.Migrations;

namespace GamesJournal.Controllers
{
    public class PlayerController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static PlayerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44382/api/playerdata/");
        }

        // GET: Player/List
        public ActionResult List()
        {
            //Comunicate with the player data API to retrive the list of players/users
            //curl https://localhost:44382/api/playerdata/listplayers

            string url = "listplayers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<Player> players = response.Content.ReadAsAsync<IEnumerable<Player>>().Result;

            //Debug.WriteLine("Number of players received");
            //Debug.WriteLine(players.Count());

            return View(players);

        }


        // GET: Player/Details/2
        public ActionResult Details(int id)
        {
            //Comunicate with the player data API to retrive one player/user
            //curl https://localhost:44382/api/PlayerData/FindPlayer/{id}

            string url = "FindPlayer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is");
            //Debug.WriteLine(response.StatusCode);

            Player selectedPlayer = response.Content.ReadAsAsync<Player>().Result;

            //Debug.WriteLine("player received:");
            //Debug.WriteLine(selectedPlayer.Username);

            return View(selectedPlayer);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Player/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Player/Create
        [HttpPost]
        public ActionResult Create(Player player)
        {
            //Debug.WriteLine("The player added is:");
            //Debug.WriteLine(player.Username);

            //Add a new player into the system using the API
            //curl -d @player.json -H "Content-Type:application/json" https://localhost:44382/api/playerdata/addplayer
            string url = "AddPlayer";

            string jsonpayload = jss.Serialize(player);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Player/Edit/5
        public ActionResult Edit(int id)
        {
            //grab the information of a game in the list of players/users

            //Comunicate with the game data API to retrive one player
            //curl https://localhost:44382/api/PlayerData/FindPlayer/{id}

            string url = "FindPlayer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is");
            //Debug.WriteLine(response.StatusCode);

            Player selectedPlayer = response.Content.ReadAsAsync<Player>().Result;

            return View(selectedPlayer);
        }

        // POST: Player/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Player player)
        {
            //After retriving the player data, updates the player data in the system using the API
            //curl -d @player.json -H "Content-Type:application/json" "https://localhost:44382/api/PlayerData/UpdatePlayer/2"

            string url = "UpdatePlayer/" + id;

            string jsonpayload = jss.Serialize(player);

            HttpContent content = new StringContent(jsonpayload);

            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details/" + id);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Player/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            //Comunicate with the game data API to retrive one player
            //curl https://localhost:44382/api/PlayerData/FindPlayer/{id}

            string url = "FindGame/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Player selectedPlayer = response.Content.ReadAsAsync<Player>().Result;
            return View(selectedPlayer);
        }

        // POST: Player/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //Comunicate with the player data API to delete one User/Player
            //curl https://localhost:44382/api/PlayerData/DeletePlayer/{id}

            string url = "DeletePlayer/" + id;

            HttpContent content = new StringContent("");

            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
            //Not sure what's going on. It's gathering the data, but once I hit the confirmation to confirm it's getting redirected into the Error page. Probably not getting the correct status(?)
        }
    }
}
