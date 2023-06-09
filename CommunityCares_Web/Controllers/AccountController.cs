using CommunityCares_Web.Data;
using CommunityCares_Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CommunityCares_Web.Controllers
{
    public class AccountController : Controller
    {
        public readonly ApplicationDbContext _context;
        
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
        private bool IsValid(string email, string password)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

                if (user != null)
                {
                    SessionClass.Id = user.Id;
                    // Verificar la contraseña usando el hash
                    //var passwordHasher = new PasswordHasher<User>(); // Reemplaza "User" con tu clase de usuario
                    //var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);

                    //if (result == PasswordVerificationResult.Success)
                    //{
                    //    // La contraseña es válida
                    //    return true;
                    //}
                    return true;
                }

                // Las credenciales son inválidas
                return false;
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir
                Console.WriteLine("Error al verificar las credenciales: " + ex.Message);
                return false;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var userLogin = new LoginUser
            {
                User = new User()
            };
            return View(userLogin);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUser userLogin)
        {
            if (ModelState.IsValid)
            {
                // Aquí puedes verificar las credenciales del usuario y realizar la lógica de inicio de sesión
                var email = userLogin.User.Email;
                var password = userLogin.User.Password;

                if (IsValid(email, password))
                {
                    // Obtener el rol del usuario desde la base de datos
                    var role = GetUserRoleFromDatabase(email);

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, role)
            };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    // Las credenciales son válidas
                    // Realiza las acciones necesarias después de iniciar sesión, como establecer la identidad del usuario
                    // Ejemplo: ClaimsPrincipal principal = BuildClaimsPrincipal(user);
                    // SignIn(principal);

                    return RedirectToAction("Login", "Account"); // Redirecciona a la página principal después del inicio de sesión exitoso
                }
                else
                {
                    ModelState.AddModelError("", "Credenciales inválidas"); // Agrega un mensaje de error al modelo si las credenciales son inválidas
                }
            }

            return View(userLogin); // Si el modelo no es válido, vuelve a mostrar la vista con los mensajes de error
        }

        private string GetUserRoleFromDatabase(string email)
        {
            // Realizar la consulta a la base de datos para obtener el rol del usuario con el email proporcionado
            // Aquí debes implementar tu lógica personalizada de consulta a la base de datos

            // Ejemplo de consulta utilizando Entity Framework Core
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                return user.Role; // Devuelve el rol del usuario encontrado en la base de datos
            }

            // Si no se encuentra el usuario en la base de datos, devolver un valor predeterminado o lanzar una excepción según tu lógica de negocio
            return "DefaultRole";
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
