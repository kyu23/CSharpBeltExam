using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RegistrationLogin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RegistrationLogin.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context { get; set; }

        private PasswordHasher<User> regHasher = new PasswordHasher<User>();
        private PasswordHasher<LoginUser> logHasher = new PasswordHasher<LoginUser>();
        
        public HomeController(MyContext context)
        {
            _context = context;
        }


        [HttpGet("")]        
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User u)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.FirstOrDefault(usr => usr.Email == u.Email) != null) //if not null, email exists
                {
                    ModelState.AddModelError("Email", "Email is already in use, try logging in!");
                    return View("Index");
                }
                string hash = regHasher.HashPassword(u, u.Password);
                u.Password = hash;
                _context.Users.Add(u);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("userId", u.UserId); //saves userID into session
                return Redirect("/home");
            }
            return View("Index");
        }
        
        [HttpPost("login")]
        public IActionResult Login(LoginUser lu)
        {
            if(ModelState.IsValid)
            {
                User userInDB = _context.Users.FirstOrDefault(u => u.Email == lu.LoginEmail);
                if(userInDB == null)
                {
                    ModelState.AddModelError("Email", "Email not found. Check your spelling?");
                    return View("Index");
                } 
                var result = logHasher.VerifyHashedPassword(lu, userInDB.Password, lu.LoginPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("Password", "Invalid Email or Password jackass!");
                }
                HttpContext.Session.SetInt32("userId", userInDB.UserId);
                return Redirect($"/home");
            }
            return View("Index");
        }

        public User GetUser()
        {
            return _context.Users.FirstOrDefault( u =>  u.UserId == HttpContext.Session.GetInt32("userId"));
        }

        [HttpGet("home")]
        public IActionResult Home()
        {
            User current = GetUser();
            if (current == null)
            {
                return Redirect ("/");
            }
            ViewBag.User = current;

            List<Models.Activity> AllActivities = _context.Activities
                                                    .Include(a => a.Organizer)
                                                    .Include( a => a.Guests )
                                                    .ThenInclude( wp => wp.ActivityGoer )
                                                    .Where( a => a.StartDate >= DateTime.Now ) 
                                                    /* .OrderBy( m => m.ScreeningTime ) adding this line broke everything */ 
                                                    .ToList();
            return View(AllActivities);
        }

        [HttpGet("activity/new")]
        public IActionResult AddActivity()
        {
            return View();
        }

        [HttpPost("activity/create")]
        public IActionResult CreateActivity(Models.Activity newActivity)
        {
            User current = GetUser();
            if (current == null)
            {
                return Redirect ("/");
            }
            if(ModelState.IsValid)
            {
                newActivity.UserId = current.UserId;
                _context.Activities.Add(newActivity);
                _context.SaveChanges();
                return RedirectToAction("Home");
            }
            return View("AddActivity");
        }

        [HttpGet("activity/{activityId}/delete")]
        public IActionResult DeleteActivity(int activityId)
        {
            Models.Activity remove = _context.Activities.FirstOrDefault( a => a.ActivityId == activityId );
            _context.Activities.Remove(remove);
            _context.SaveChanges();
            return RedirectToAction("Home");
        }

        [HttpGet("activity/{activityId}/{status}")]
        public IActionResult ToggleParty(int activityId, string status)
        {
            User current = GetUser();
                if (current == null)
                {
                    return Redirect ("/");
                }

            if(status == "join")
            {
                WatchParty newParty = new WatchParty();
                newParty.UserId = current.UserId;
                newParty.ActivityId = activityId;
                _context.Parties.Add(newParty);
            }
            else if (status == "leave")
            {
                WatchParty backout = _context.Parties.FirstOrDefault( wp => wp.UserId == current.UserId && wp.ActivityId == activityId );
                _context.Parties.Remove(backout);
            }
            _context.SaveChanges();
            return RedirectToAction("Home");
        }
        
        [HttpGet("activity/{activityId}")]
        public IActionResult DisplayActivity(int activityId)
        {
            User current = GetUser();
                if (current == null)
                {
                    return Redirect ("/");
                }
                ViewBag.User = current;
                Models.Activity showing = _context.Activities
                                                    .Include( a => a.Guests )
                                                    .ThenInclude( w => w.ActivityGoer )
                                                    .Include( a => a.Organizer )
                                                    .FirstOrDefault( a => a.ActivityId == activityId);
                return View(showing);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
