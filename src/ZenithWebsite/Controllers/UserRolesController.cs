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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace ZenithWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UserRolesController(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: UserRoles
        public async Task<ActionResult> Index()
        {
            // Get users 
            var users = _userManager.Users;

            // Convert into a view model 
            var usersView = new List<UserRolesViewModel>();
            foreach (ApplicationUser usr in users)
            {
                var roles = await _userManager.GetRolesAsync(usr);
                var userView = new UserRolesViewModel()
                {
                    Username = usr.UserName,
                    Email = usr.Email,
                    Roles = roles
                };
                usersView.Add(userView);
            }

            return View(usersView);
        }

        // GET: UserRoles/AddRole/5
        public async Task<ActionResult> AddRole(string id)
        {
            // Get user 
            var user = await _userManager.FindByNameAsync(id);
            var usersRoles = await _userManager.GetRolesAsync(user);

            // Convert into a view model 
            var viewModel = new EditUserRoleViewModel()
            {
                Username = user.UserName,
                Email = user.Email,
                Roles = usersRoles
            };

            ViewData["AllRoles"] = new SelectList(_roleManager.Roles, "Name", "Name");
            return View(viewModel);
        }

        // POST: UserRoles/AddRole/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRole(string id, EditUserRoleViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                var roleToAdd = viewModel.SelectedRole;
                var user = await _userManager.FindByNameAsync(id);
                var result = await _userManager.AddToRoleAsync(user, roleToAdd);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If here, then error occured 
            ViewData["AllRoles"] = new SelectList(_roleManager.Roles, "Name", "Name");
            return View(viewModel);
        }

        // GET: UserRoles/DeleteRole/Username
        public async Task<ActionResult> DeleteRole(string id)
        {
            // Get user 
            var user = await _userManager.FindByNameAsync(id);
            var usersRoles = await _userManager.GetRolesAsync(user);

            // Convert into a view model 
            var viewModel = new EditUserRoleViewModel()
            {
                Username = user.UserName,
                Email = user.Email,
                Roles = usersRoles
            };

            ViewData["AllRoles"] = new SelectList(_roleManager.Roles, "Name", "Name");
            return View(viewModel);
        }

        // POST: UserRoles/DeleteRole/Username
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRole(string id, EditUserRoleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Fast exit if trying to modify user 'a' or role 'admin'
                if (viewModel.Username == "a" && viewModel.SelectedRole.ToUpper() == "ADMIN")
                {
                    ModelState.AddModelError(string.Empty, "User 'a' cannot be removed from Admin");
                    ViewData["AllRoles"] = new SelectList(_roleManager.Roles, "Name", "Name");
                    return View(viewModel);
                }

                var roleToDelete = viewModel.SelectedRole;
                var user = await _userManager.FindByNameAsync(viewModel.Username);
                var result = await _userManager.RemoveFromRoleAsync(user, roleToDelete);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If here, then error occured 
            ViewData["AllRoles"] = new SelectList(_roleManager.Roles, "Name", "Name");
            return View(viewModel);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}