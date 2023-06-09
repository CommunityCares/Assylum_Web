using CommunityCares_Web.Data;
using CommunityCares_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CommunityCares_Web.Controllers
{
    public class ModelMultiController : Controller
    {
        public readonly ApplicationDbContext _context;

        public ModelMultiController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: ModelMultiController
        public async Task<ActionResult> Index()
        {
            var query = from person in _context.People
                        join user in _context.Users on person.Id equals user.Id
                        join admin in _context.Admins on person.Id equals admin.Id
                        where person.Status==0
                        select new ModelMulti
                        {
                            Person=person,
                            User=user,
                            Admin=admin
                        };
            return View(await query.ToArrayAsync());
        }

        // GET: ModelMultiController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ModelMultiController/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdAssylum"] = new SelectList( _context.Assylums, "Id", "Name");
            var person = new Person();
            var user =new User();   
            var admin=new Admin();

            var model = new ModelMulti
            {
                Person = person,
                User = user,   
                Admin = admin
            };
            return View(model);
        }

        // POST: ModelMultiController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Person person,User user,Admin admin,byte IdAssylum)
        {
            if (ModelState.IsValid)
            {
                using var transaction= await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.People.Add(person);
                    await _context.SaveChangesAsync();

                    user.Id = person.Id;
                    

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    admin.Id = person.Id;
                    admin.IdAssylum=IdAssylum;

                    _context.Admins.Add(admin);

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    ViewData["IdAssylum"] = new SelectList(_context.Assylums, "Id", "Id");
                    return RedirectToAction("Index","ModelMulti");
                } 
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    ViewData["IdAssylum"] = new SelectList(_context.Assylums, "Id", "Id");
                    throw;
                }
            }
            else 
            { 
                return View(); 
            }
         }
        public string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<object>(); // Reemplaza "object" por el tipo de modelo de usuario correspondiente

            string hashedPassword = passwordHasher.HashPassword(null, password);

            return hashedPassword;
        }
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ModelMultiController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ModelMultiController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ModelMultiController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
