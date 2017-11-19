using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Data.Entity;
using FinalProject.Models;
using System.Data.Entity.Infrastructure;

namespace FinalProject.Controllers
{
	public class ChatBotController : Controller
	{
		private MarkovDBEntities1 db = new MarkovDBEntities1();
		static Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();

		// GET: ChatBot
		public ActionResult Index()
        {
			dictionary.Clear(); // Prevents memory from building up.
			return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Generate(bool bibble, bool smash, bool pdf, HttpPostedFileBase customFile, string customText)
		{
			dictionary.Clear();
			// Get Data from Index View

			string[] fileEntries;
			if (bibble == true)
			{
				fileEntries = Directory.GetFiles(Server.MapPath("~/App_Data/Content/bibble"));
				foreach (string fileName in fileEntries)
				{
					string input = System.IO.File.ReadAllText(fileName);
					Add(input);
				}
			}
			if (smash == true)
			{
				fileEntries = Directory.GetFiles(Server.MapPath("~/App_Data/Content/fanfic"));
				foreach (string fileName in fileEntries)
				{
					string input = System.IO.File.ReadAllText(fileName);
					Add(input);
				}
			}
			if (pdf == true)
			{
				fileEntries = Directory.GetFiles(Server.MapPath("~/App_Data/Content/Books"));
				foreach (string fileName in fileEntries)
				{
					string input = System.IO.File.ReadAllText(fileName);
					Add(input);
				}
			}

			string path = Server.MapPath("~/App_Data/Uploads/");
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			if (customFile != null)
			{
				string fileName = System.IO.Path.GetFileName(customFile.FileName);
				var allowedFileTypes = new[] { ".txt", ".pdf" };
				var extension = System.IO.Path.GetExtension(fileName);
				if (!allowedFileTypes.Contains(extension))
				{

					ViewBag.Message += $"{fileName} denied. Incorrect File Type.";
				}
				else
				{
					customFile.SaveAs(path + fileName);
					ViewBag.Message += $"{fileName} uploaded.";
				}
			}

			// For custom files uploaded
			fileEntries = Directory.GetFiles(Server.MapPath("~/App_Data/Uploads/"));
			foreach (string fileName in fileEntries)
			{
				string input = System.IO.File.ReadAllText(fileName);
				Add(input);
			}

			if (!String.IsNullOrWhiteSpace(customText))
			{
				Add(customText);
			}

			ViewBag.feedBibble = bibble;
			ViewBag.feedSmash = smash;
			ViewBag.feedPDF = pdf;
			ViewBag.feedCustomText = customText;
			ViewBag.feedFile = customFile;

			TempData["GeneratedText"] = ViewData["GeneratedText"] = Print();
			return View();
		}

		// GET: ChatBot/Details
		public ActionResult Details(int? id)
		{
			if(id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Favorites fav = db.Favorites.Find(id);
			if(fav == null)
			{
				return HttpNotFound();
			}
			return View(fav);
		}

		public ActionResult HallOfQuotes()
		{
			var model = from favs in db.Favorites
						select new DBViewModel
						{
							ID = favs.Id,
							Added = (DateTime)favs.Added,
							Quote = favs.Quote
						};
				return View(model.ToList());
		}

		// POST: ChatBot/AddFav
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddFav([Bind(Include = "Id,Added,Quote")] Favorites fav)
		{
			string temp = TempData["GeneratedText"].ToString();
			if (ModelState.IsValid)
			{
				// Add new fav to fav lsit with the information gathered from ChatBot view
				fav.Quote = temp;
				fav.Added = DateTime.Now;
				db.Favorites.Add(fav);
				//db.Favorites.Add(new Favorites() { Id = fav.Id, Added = DateTime.Now, Quote = temp });
				
				// Save addition
				db.SaveChanges();

				// Return to index page
				return RedirectToAction("Index");
			}
			// Returns view
			return View(temp);
		}

		/*
		//GET: ChatBot/Edit
		public ActionResult Edit(int? id)
		{
			if(id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Favorites fav = db.Favorites.Find(id);
			if (fav == null)
			{
				return HttpNotFound();
			}
			return View(fav);
		}
		// POST: ChatBot/Edit
		[HttpPost, ActionName("Edit")]
		[ValidateAntiForgeryToken]
		public ActionResult EditPost(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var favToUpdate = db.Favorites.Find(id);
			if (TryUpdateModel(favToUpdate, "",
			   new string[] { "ID", "Quote" }))
			{
				try
				{
					db.SaveChanges();

					return RedirectToAction("HallOfQuotes");
				}
				catch (RetryLimitExceededException)
				{
					//Log the error (uncomment dex variable name and add a line here to write a log.
					ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
				}
			}
			return View(favToUpdate);
		}
		*/

		// GET: ChatBot/Delete
		public ActionResult Delete(int? id)
		{

			// Error handling
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			// Create new computer object and populate with data from db
			Favorites fav = db.Favorites.Find(id);

			// More error handling
			if (fav == null)
			{
				return HttpNotFound();
			}

			// Return view
			return View(fav);
		}

		// POST: ChatBot/Delete
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int? id)
		{
			// Create new computer and device objects and populate with db data
			Favorites fav = db.Favorites.Find(id);

			// Remove computer
			db.Favorites.Remove(fav);
			db.SaveChanges();

			// Return to index
			return RedirectToAction("HallOfQuotes");
		}

		protected override void Dispose(bool disposing)
		{
			// Garbage collection
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		public void Add(string input)
		{
			input = Regex.Replace(input, @"[\t\r\n]", " ");
			input = Regex.Replace(input, $"Convert.ToChar(15)", " ");
			input = Regex.Replace(input, @"[ ]{2,}", " ");
			string currentGram = "";
			string nextGram = "";
			string[] splitInput = input.Split(' ').ToArray();
			for (int i = 0; i < splitInput.Length; i++)
			{
				currentGram = splitInput[i];
				currentGram = Regex.Replace(currentGram, @"\\", "");
				if (splitInput.Length == 1)
				{
					if (dictionary.ContainsKey(currentGram)) { return; }
				}
				if (i < splitInput.Length - 1)
				{
					nextGram = splitInput[i + 1];
					nextGram = Regex.Replace(nextGram, @"\\", "");
				}
				if (dictionary.ContainsKey(currentGram))
				{
					dictionary[currentGram].Add(nextGram);
				}
				else
				{
					List<string> list = new List<string>();
					list.Add(nextGram);
					dictionary.Add(currentGram, list);
				}
			}
		}

		public string Print()
		{
			Random rnd = new Random();
			string result = "";
			int nextIndex = rnd.Next(dictionary.Count);
			string currentGram = dictionary.ElementAt(nextIndex).Key;

			string nextGram = dictionary[currentGram][rnd.Next(dictionary[currentGram].Count)];
			int wordcount = 0;

			while (!nextGram.Contains('.') && !nextGram.Contains('?') && !nextGram.Contains('!') || wordcount < 10)
			{
				result += $"{currentGram} ";
				nextGram = dictionary[currentGram][rnd.Next(dictionary[currentGram].Count)];
				currentGram = nextGram;
				wordcount++;
			}
			/*
			for (int i = 0; i < 15; i++)
			{
				result += $"{currentGram} ";
				string nextGram = dictionary[currentGram][rnd.Next(dictionary[currentGram].Count)];
				currentGram = nextGram;
			}
			*/
			result += $"{currentGram}";
			//Console.WriteLine(result);
			return result;
		}
	}
}