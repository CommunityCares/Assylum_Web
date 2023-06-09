using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CommunityCares_Web.Data;
using CommunityCares_Web.Models;

using Firebase.Storage;

using System.Text;
using Firebase.Auth;

namespace CommunityCares_Web.Controllers
{
    public class ImageController : Controller
    {
        private static string ApiKey = "AIzaSyA4DjsG7hKblkmS8RO9JluoKCS4Gk_y_f8";
        private static string Bucket = "taskmanager-62211.appspot.com";
        private static string AuthEmail = "apalamarcelo@gmail.com";
        private static string AuthPassword = "Admin_123";
        private readonly ApplicationDbContext _context;

        public ImageController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int id)
        {
            IQueryable<Image> image = from imag in _context.Images.Include(a => a.IdCampaignNavigation).Include(x=>x.IdCampaignNavigation.IdAssylumNavigation)
                                             
                                             where imag.IdCampaign == id
                                             select imag;


            return View(image.ToList());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .Include(i => i.IdCampaignNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }
        private static async Task<string> SubirImagen(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Autenticación con Firebase
                var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

                var authResult = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

                // Crear instancia de FirebaseStorage
                var storage = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(authResult.FirebaseToken),
                        ThrowOnCancel = true // Cuando se cancela la carga, se lanza una excepción. Por defecto, no se lanza ninguna excepción.
                    });

                // Ruta en la que deseas almacenar la imagen en Firebase Storage
                string storagePath = file.FileName;

                // Subir la imagen a Firebase Storage
                using (var stream = file.OpenReadStream())
                {
                    var task = storage.Child(storagePath).PutAsync(stream);
                    try
                    {
                        // Obtener la URL de descarga de la imagen
                        var downloadUrl = await task;

                        // Retornar la URL de descarga de la imagen
                        return downloadUrl;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }

            return null; // Si no se pudo subir la imagen, retorna null
        }
        public IActionResult Create()
        {
            ViewData["IdCampaign"] = new SelectList(_context.Campaigns, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(int id,[Bind("Url,IdCampaign")] Image image, IFormFile file)
        {
            if (ModelState.IsValid)
            {

               
                
                // Aquí llamas al método para subir la imagen a Firebase Storage
                var imageUrl = await SubirImagen(file);

                // Asignar la URL de descarga de la imagen al campo Url del modelo Image
                image.Url = imageUrl;

                // Resto del código para guardar el modelo en la base de datos
                image.IdCampaign = id;
                _context.Add(image);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCampaign"] = new SelectList(_context.Campaigns, "Id", "Id", image.IdCampaign);
            return RedirectToAction("Index" , "Campaign");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            ViewData["IdCampaign"] = new SelectList(_context.Campaigns, "Id", "Id", image.IdCampaign);
            return View(image);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Url,IdCampaign")] Image image)
        {
            if (id != image.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(image);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(image.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCampaign"] = new SelectList(_context.Campaigns, "Id", "Id", image.IdCampaign);
            return View(image);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .Include(i => i.IdCampaignNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Images == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Images'  is null.");
            }
            var image = await _context.Images.FindAsync(id);
            if (image != null)
            {
                _context.Images.Remove(image);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageExists(int id)
        {
          return (_context.Images?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
