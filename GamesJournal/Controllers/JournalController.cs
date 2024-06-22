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
    public class JournalController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static JournalController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44382/api/journaldata/");
        }


        // GET: Journal/list
        public ActionResult List()
        {
            //Comunicate with the journal data API to retrive the list of journal entries
            //curl https://localhost:44382/api/journaldata/ListJournalsEntries

            string url = "ListJournalsEntries";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<JournalEntryDto> journals = response.Content.ReadAsAsync<IEnumerable<JournalEntryDto>>().Result;

            //Debug.WriteLine("Number of journal entries received");
            //Debug.WriteLine(journals.Count());

            return View(journals);
        }

        // GET: Journal/Details/5
        public ActionResult Details(int id)
        {
            //Comunicate with the Journal data API to retrive one journal entry
            //curl https://localhost:44382/api/JournalData/FindJournalEntry/{id}

            string url = "FindJournalEntry/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is");
            //Debug.WriteLine(response.StatusCode);

            JournalEntryDto selectedJournalEntry = response.Content.ReadAsAsync<JournalEntryDto>().Result;

            //Debug.WriteLine("Entry received:");
            //Debug.WriteLine(selectedJournalEntry.JournalEntryTitle);

            return View(selectedJournalEntry);
        }

        public ActionResult Error()
        {
            return View();
        }


        // GET: Journal/New
        public ActionResult New()
        {

            //Display information about the games and players in the system to showcase as a list. Due to time constrains and a few attempts, that broke the code I was not able to complete this. Reverted back to previous version.

            return View();
        }

        // POST: Journal/Create
        [HttpPost]
        public ActionResult Create(Journal journal)
        {
            //Debug.WriteLine("The journal entry added is:");
            //Debug.WriteLine(journal.JournalEntryTitle);

            //Add a new journal entry into the system using the API
            //curl -d @journalentry.json -H "Content-Type:application/json" https://localhost:44382/api/JournalData/AddJournal
            string url = "AddJournal";

            string jsonpayload = jss.Serialize(journal);

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

        // GET: Journal/Edit/5
        public ActionResult Edit(int id)
        {
            //grab the information of a journal entry in the list of games

            //Comunicate with the Journal data API to retrive one journal entry
            //curl https://localhost:44382/api/JournalData/FindJournalEntry/{id}

            string url = "FindJournalEntry/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is");
            //Debug.WriteLine(response.StatusCode);

            JournalEntryDto selectedEntry = response.Content.ReadAsAsync<JournalEntryDto>().Result;

            return View(selectedEntry);
        }

        // POST: Journal/Update/5
        [HttpPost]
        public ActionResult Update(int id, Journal journal)
        {
            //After retriving the journal entry data, updates the entry data in the system using the API
            //curl -d @game.json -H "Content-Type:application/json" "https://localhost:44382/api/JournalData/UpdateJournal/5

            string url = "UpdateJournal/" + id;

            string jsonpayload = jss.Serialize(journal);

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

        // GET: Journal/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            //Comunicate with the Journal data API to retrive one journal entry
            //curl https://localhost:44382/api/JournalData/FindJournalEntry/{id}

            string url = "FindJournalEntry/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            JournalEntryDto selectedEntry = response.Content.ReadAsAsync<JournalEntryDto>().Result;
            return View(selectedEntry);
        }

        // POST: Journal/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //Comunicate with the Journal data API to delete one journal entry
            //curl https://localhost:44382/api/JournalData/DeleteJournalEntry/{id}

            string url = "DeleteJournalEntry/" + id;

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

            //Same error as in with the Player update feature. I'm not sure what's going on.
        }
    }
}
