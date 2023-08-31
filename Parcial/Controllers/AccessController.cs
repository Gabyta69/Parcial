using Parcial.Models;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Parcial.Controllers
{
    public class AccessController : Controller
    {
        // GET: Access
        private RC101320Entities1 db = new RC101320Entities1();
       
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string user, string password)
        {
            var usuario = db.Usuarios.FirstOrDefault(u => u.User == user);

            if (usuario != null && BCrypt.Net.BCrypt.Verify(password, usuario.Password))
            {
                if (usuario.Rol == 0)
                {
                    return RedirectToAction("Usuarios", "Home");
                }
                else 
                {
                    if (usuario.Rol == 1)
                    {
                        Session["UserID"] = usuario.Id;

                        return RedirectToAction("Estudiante", "Home");
                    }                    
                }                
            }

            ViewBag.Error = "El usuario " + user + " no existe en la base de datos";
            return RedirectToAction("Login", "Access", new { error = "El usuario " + user + " no existe en la base de datos" });
        }

        public ActionResult Register(string nuevoUsuario, string nuevaContraseña)
        {
            // Verificar si el usuario ya existe
            if (db.Usuarios.Any(u => u.User == nuevoUsuario))
            {
                ViewBag.RegistrationError = "El usuario ya existe";
                return View("Login", "Access"); // Redirige a la vista de registro
            }

            // Ejecutar el procedimiento almacenado para insertar el nuevo usuario
            db.Database.ExecuteSqlCommand("InsertarUsuario @NuevoUsuario, @NuevaContraseña",
                new SqlParameter("NuevoUsuario", nuevoUsuario),
                new SqlParameter("NuevaContraseña", BCrypt.Net.BCrypt.HashPassword(nuevaContraseña)));

            return RedirectToAction("Login", "Access"); // Redirige a la vista de registro
        }
                
        private bool VerificarContrasena(string passwordIngresada, string passwordGuardada, string salt)
        {
            var hashed = BCrypt.Net.BCrypt.HashPassword(passwordIngresada, salt);
            return hashed == passwordGuardada;
        }

    }
}
