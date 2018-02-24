using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WeddingPlanner.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private WeddingPlannerContext _context;

        public HomeController(WeddingPlannerContext context) {
            _context = context;
        }

// ******************************************************************************************************************************
// ******************************************************************************************************************************
// Login Registration Section

        [HttpGet]
        [Route("")]
        [Route("index")]
        public IActionResult Index() {
            HttpContext.Session.Clear();
            // ViewBag.InitSession = HttpContext.Session;
            ViewBag.InitSession = HttpContext.Session.GetInt32("CurrentUserId");            
            return View("Index");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(User LoginUser) {
            
            User RetrievedUser = _context.Users.SingleOrDefault(user => user.Email == LoginUser.Email);
            if (RetrievedUser != null && LoginUser.Password != null) {
                var Hasher = new PasswordHasher<User>();
                if (0 != Hasher.VerifyHashedPassword(RetrievedUser, RetrievedUser.Password, LoginUser.Password)) {
                    HttpContext.Session.SetInt32("CurrentUserId", RetrievedUser.UserId);  // Set session
                    ViewBag.InitSession = HttpContext.Session.GetInt32("CurrentUserId");
                    return RedirectToAction("Dashboard");
                }
            }
            ViewBag.error = "Login information is invalid.";
            return View("Index");
        }

        [HttpGet]
        [Route("registerform")]
        public IActionResult RegisterForm() {
            HttpContext.Session.Clear();
            ViewBag.InitSession = HttpContext.Session.GetInt32("CurrentUserId");
            return View("Register");
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel model) {
            
            if(ModelState.IsValid) {
                User user = new User {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, model.Password);

                _context.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
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

            return View("Index");
        }

// End Login Registration section
// ******************************************************************************************************************************
// ******************************************************************************************************************************

        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard() {
            User CurrentUser = _context.Users.SingleOrDefault(u => u.UserId == HttpContext.Session.GetInt32("CurrentUserId"));
            if (CurrentUser != null) {
                List<Wedding> AllWeddings = _context.Weddings.Include(w => w.GuestsAttending).OrderBy(wc => wc.Date).ToList();
                ViewBag.InitSession = HttpContext.Session.GetInt32("CurrentUserId");
                ViewBag.allWeddings = AllWeddings;
                ViewBag.currentUser = CurrentUser;
            return View("Dashboard");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("CreateConfirmation")]
        public IActionResult CreateConfirmation(int weddingId) {
            if (HttpContext.Session.GetInt32("CurrentUserId") != null) {
                WeddingConfirmation confirm = new WeddingConfirmation {
                    GuestId = (int)HttpContext.Session.GetInt32("CurrentUserId"),
                    WeddingId = weddingId
                };
                _context.Add(confirm);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("cancel/{weddingId}")]
        public IActionResult Cancel(int weddingId) {
            WeddingConfirmation RetrievedWeddingConfirmation = _context.WeddingConfirmations.Where(w => w.WeddingId == weddingId).Where(p => p.GuestId == HttpContext.Session.GetInt32("CurrentUserId")).SingleOrDefault();
            _context.WeddingConfirmations.Remove(RetrievedWeddingConfirmation);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("allusers")]
        public IActionResult AllUsers() {
            if (HttpContext.Session.GetInt32("CurrentUserId") != null) {
                ViewBag.InitSession = HttpContext.Session.GetInt32("CurrentUserId");
                List<User> AllUsers = _context.Users.ToList();
                ViewBag.allUsers = AllUsers;
                return View("AllUsers");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("allweddingconfirmations")]
        public IActionResult AllWeddingConfirmations() {
            if (HttpContext.Session.GetInt32("CurrentUserId") != null) {
                List<WeddingConfirmation> AllConfirmations = _context.WeddingConfirmations.OrderBy(wc => wc.WeddingId).ToList();
                ViewBag.InitSession = HttpContext.Session.GetInt32("CurrentUserId");                
                ViewBag.allConfirmations = AllConfirmations;
                return View("AllConfirmations");
            }
            return RedirectToAction("Index");
        }
    }
}