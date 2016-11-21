using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ZenithWebsite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ZenithWebsite.Data;
using ZenithWebsite.Models.UserRolesViewModel;

namespace ZenithWebsite.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        /*
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        */
        public UserRolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;

            _context = context;
        }

        // GET: UserRoles
        public async Task<ActionResult> Index()
        {
            // Eager loading 
            var users = _userManager.Users;
            var usersc = _context.Users;
            var usersView = new List<UserRolesViewModel>();
            // .Include(e => e.Activity)
            // .SingleOrDefaultAsync(m => m.EventId == id);
            var f = users.First().Roles;
            var f1 = usersc.First().Roles;
            foreach (ApplicationUser usr in users)
            {
                var test = usr.Roles;
                var roles = await _userManager.GetRolesAsync(usr);
                var userView = new UserRolesViewModel()
                {
                    Username = usr.UserName,
                    Email = usr.Email,
                    Roles = roles
                };
            }
            return View(users);
        }

        // GET: UserRoles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserRoles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserRoles/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserRoles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserRoles/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserRoles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}