using StudentLibraryManagement.Data;
using StudentLibraryManagement.Models;
using StudentLibraryManagement.Models.ViewModels;
using StudentLibraryManagement.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace StudentLibraryManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        //UserManager is used as the database class for AspNetUser (CRUD Operations)
        UserManager<ApplicationUser> _userManager;
        //SignInManager contains the build in functions for login, logout, registartion, etc. 
        SignInManager<ApplicationUser> _signInManager;
        //RoleManager is used as the database class for AspNetRoles (CRUD Operations)
        RoleManager<IdentityRole> _roleManager;

        public AccountController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // _signInManager.PasswordSignInAsync matches credentials with database values
                var resut = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (resut.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Login Failed");
            }
            return View(model);
        }

        public async Task<IActionResult> Register(string RoleName)
        {
            // _roleManager.RoleExistsAsync checks whether the value (Helper.Admin) exists in database or not
            if (!_roleManager.RoleExistsAsync(Helper.Admin).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(Helper.Admin));
                await _roleManager.CreateAsync(new IdentityRole(Helper.Student));
            }

            var model = new RegisterViewModel
            {
                RoleName = RoleName ?? "Admin" // Default to Admin if no role is provided
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name
                };

                if(model.RoleName == "Student")
                {
                    var student = new Student
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Department = model.Department,
                        Age = model.Age,
                        Gender = model.Gender
                    };

                    _db.Students.Add(student);
                    await _db.SaveChangesAsync();
                }

                // _userManager.CreateAsync creates the values in the database for AspNetUser
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // _userManager.AddToRoleAsync adds a role for the user i.e., as FK in separate Table 
                    await _userManager.AddToRoleAsync(user, model.RoleName);
                    // _signInManager.SignInAsync signs in the user
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    //if (!User.IsInRole(Helper.Admin))
                    //{
                    //    await _signInManager.SignInAsync(user, isPersistent: false);
                    //}
                    //else
                    //{
                    //    TempData["newAdminSignUp"] = user.Name;
                    //}

                    return RedirectToAction("Index", "Home");
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> LogOff()
        {
            // _signInManager.SignOutAsync is used to logout the user from the database
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}