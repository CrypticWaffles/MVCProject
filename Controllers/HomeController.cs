using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MVCProject.Models;
using System.Diagnostics;

using MVCProject.DAL;

namespace MVCProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _config = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Page2(UserInfo a)
        {
            //Send to database
            Person person = new Person(_config);
            string id = person.InsertPerson(a);

            //Save ID in session
            HttpContext.Session.SetString("PID", id);

            //Get id from session (testing)
            //string? str = HttpContext.Session.GetString("PID");

            //viewbag
            ViewBag.Message = "Everything is good";

            return View(a);
        }

        public IActionResult Page3()
        {
            string? id = HttpContext.Session.GetString("PID");

            // get animal from db
            Person person = new Person(_config);
            UserInfo a = person.GetUser(id);

            return View(a);
        }

        public IActionResult UpdateUser(UserInfo userInfo)
        {
            string? id = HttpContext.Session.GetString("PID");

            // get animal from db
            Person person = new Person(_config);
            bool isUpdated = person.UpdateUser(userInfo, id);

            //get animal from db
            userInfo = person.GetUser(id);

            return View("Page2", userInfo);
        }

        public IActionResult DeleteUser()
        {
            //Get id from session (testing)
            string? id = HttpContext.Session.GetString("PID");

            // get animal from db
            Person person = new Person(_config);
            bool isDeleted = person.DeleteUser(id);

            if (isDeleted)
            {
                ViewBag.status = "Entry Deleted";
            }
            else
            {
                ViewBag.status = "Failed to delete";
            }

            return View();
        }
    }
}
