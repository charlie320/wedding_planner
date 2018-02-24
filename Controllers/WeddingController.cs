using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WeddingPlanner.Models;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ApiCaller;

namespace WeddingPlanner.Controllers
{
    public class WeddingController : Controller
    {

        private WeddingPlannerContext _context;

        public WeddingController(WeddingPlannerContext context) {
            _context = context;
        }

        [HttpGet]
        [Route("wedding/{weddingId}")]
        public IActionResult Wedding(int weddingId)
        {
            if (HttpContext.Session.GetInt32("CurrentUserId") != null) {
                Wedding RetrievedWedding = _context.Weddings.Where(w => w.WeddingId == weddingId).Include(g => g.GuestsAttending).ThenInclude(gA => gA.Guest).SingleOrDefault();
                ViewBag.InitSession = HttpContext.Session.GetInt32("CurrentUserId");
                ViewBag.wedding = RetrievedWedding;
                return View("Wedding");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("newwedding")]
        public IActionResult NewWedding() {
            if (HttpContext.Session.GetInt32("CurrentUserId") != null) {
                ViewBag.InitSession = HttpContext.Session.GetInt32("CurrentUserId");
                return View("NewWedding");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("createwedding")]
        public IActionResult CreateWedding(Wedding model) {
            ViewBag.InitSession = HttpContext.Session.GetInt32("CurrentUserId");
            
            if(ModelState.IsValid && HttpContext.Session.GetInt32("CurrentUserId") != null) {
                JToken AddressCoordinates = SearchAddress(model.WeddingAddress);
                double latitude = (double)AddressCoordinates["lat"];
                double longitude = (double)AddressCoordinates["lng"];
                Wedding wedding = new Wedding {
                    WedderOne = model.WedderOne,
                    WedderTwo = model.WedderTwo,
                    Date = model.Date,
                    WeddingAddress = model.WeddingAddress,
                    Latitude = latitude,
                    Longitude = longitude,
                    PlannerId = (int)HttpContext.Session.GetInt32("CurrentUserId"),
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };

                _context.Add(wedding);
                _context.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }

            // *****One way to extract validation errors*****
            // foreach (var modelState in ModelState.Values) {
            //     foreach (ModelError error in modelState.Errors) {
            //         Console.WriteLine(error.ErrorMessage);
            //     }
            // }

            // *****Another way to extract validation errors.  However, Web app will utilize @Http.ValidationSummary() on client side to display errors*****
            // IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            // ViewBag.errors = allErrors;

            return View("NewWedding");
        }

// **********************************************************************************************************************************************
// Method to obtain GPS coordinates
        public JToken SearchAddress(string address)
        {           
            JObject WeddingLocationData = new JObject();
            
            WebRequest.GetWeddingDataAsync(address, ApiResponse =>
                {
                    WeddingLocationData = ApiResponse;
                }
            ).Wait();

            JToken WeddingCoordinates = new JObject();
            WeddingCoordinates = WeddingLocationData.SelectToken("results[0].geometry.location");
            return WeddingCoordinates;
        }

// **********************************************************************************************************************************************

        [HttpGet]
        [Route("delete/{weddingId}")]
        public IActionResult Delete(int weddingId) {
            if (HttpContext.Session.GetInt32("CurrentUserId") != null) {
                Wedding RetrievedWedding = _context.Weddings.Where(w => w.WeddingId == weddingId).SingleOrDefault();
                _context.Weddings.Remove(RetrievedWedding);

                List<WeddingConfirmation> RetrievedWeddingConfirmations = _context.WeddingConfirmations.Where(w => w.WeddingId == weddingId).ToList();
                foreach (var item in RetrievedWeddingConfirmations) {
                    _context.WeddingConfirmations.Remove(item);
                }
                _context.SaveChanges();

            return RedirectToAction("Dashboard", "Home");
            }
            return ViewBag("Index");
        }

        [HttpGet]
        [Route("allweddings")]
        public IActionResult AllWeddings() {
            if (HttpContext.Session.GetInt32("CurrentUserId") != null) {
                List<Wedding> AllWeddings = _context.Weddings.ToList();
                ViewBag.InitSession = HttpContext.Session.GetInt32("CurrentUserId");                
                ViewBag.allWeddings = AllWeddings;
                return View("AllWeddings");
            }
            return RedirectToAction("Index", "Home");
        }

    }
}