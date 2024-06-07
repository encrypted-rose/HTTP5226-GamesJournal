using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using GamesJournal.Models;
using System.Web.Script.Serialization;


namespace GamesJournal.Controllers
{
    public class GameController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static GameController()
        {
            client= new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44382/api/gamedata/");
        }
        
        
        // GET: Game/List
        public ActionResult List()
        {
            //Comunicate with the game data API to retrive the list of games
            //curl https://localhost:44382/api/gamedata/listgames

            string url = "listgames";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<Game> games = response.Content.ReadAsAsync<IEnumerable<Game>>().Result;

            //Debug.WriteLine("Number of games received");
            //Debug.WriteLine(games.Count());

            return View(games);
        }

        // GET: Game/Details/5
        public ActionResult Details(int id)
        {
            //Comunicate with the game data API to retrive one game
            //curl https://localhost:44382/api/GameData/FindGame/{id}

            string url = "FindGame/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is");
            //Debug.WriteLine(response.StatusCode);

            Game selectedGame = response.Content.ReadAsAsync<Game>().Result;

            //Debug.WriteLine("game received:");
            //Debug.WriteLine(selectedGame.GameTitle);

            return View(selectedGame);
        }

        //Error page
        public ActionResult Error()
        {
            return View();
        }

        // GET: Game/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Game/Create
        [HttpPost]
        public ActionResult Create(Game game)
        {
            //Debug.WriteLine("The game added is:");
            //Debug.WriteLine(game.GameTitle);

            //Add a new game into the system using the API
            //curl -d @game.json -H "Content-Type:application/json" https://localhost:44382/api/gamedata/addgame
            string url = "AddGame";

            string jsonpayload = jss.Serialize(game);

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

        // GET: Game/Edit/2
        public ActionResult Edit(int id)
        {
            //grab the information of a game in the list of games

            //Comunicate with the game data API to retrive one game
            //curl https://localhost:44382/api/GameData/FindGame/{id}

            string url = "FindGame/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is");
            //Debug.WriteLine(response.StatusCode);

            Game selectedGame = response.Content.ReadAsAsync<Game>().Result;

            return View(selectedGame);
        }

        // POST: Game/Update/5
        [HttpPost]
        public ActionResult Update(int id, Game game)
        {
            //After retriving the game data, updates the game data in the system using the API
            //curl -d @game.json -H "Content-Type:application/json" "https://localhost:44382/api/GameData/UpdateGame/7"

            string url = "UpdateGame/" + id;

            string jsonpayload = jss.Serialize(game);

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


        // GET: Game/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            //Comunicate with the game data API to retrive one game
            //curl https://localhost:44382/api/GameData/FindGame/{id}

            string url = "FindGame/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Game selectedGame = response.Content.ReadAsAsync<Game>().Result;
            return View(selectedGame);
        }

        // POST: Game/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //Comunicate with the game data API to delete one game
            //curl https://localhost:44382/api/GameData/DeleteGame/{id}

            string url = "DeleteGame/" + id;

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

        }
    }
}
