using Microsoft.Extensions.Logging;
using Parcial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.WebControls;
using Parcial.ViewModel;

namespace Parcial.Controllers
{
    public class HomeController : Controller
    {
        private RC101320Entities1 db = new RC101320Entities1();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Usuarios()
        {
            var usuarios = db.Usuarios.ToList();
            var roles = db.Roles.ToList();
            ViewBag.Roles = new SelectList(roles, "IdRol", "Rol");

            return View(usuarios);
        }

        [HttpPost]
        public ActionResult CambiarRol(int userId, int nuevoRol)
        {
            var usuario = db.Usuarios.Find(userId);
            if (usuario != null)
            {
                usuario.Rol = nuevoRol;
                db.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public ActionResult AdminEstudiantes()
        {
            var usuarios = db.Usuarios.ToList();
            var cursosDisponibles = db.Cursos.ToList();

            var usuariosConCursos = new List<UsuarioConCursosViewModel>();

            foreach (var usuario in usuarios)
            {
                var cursosUsuario = db.InscripcionMaterias
                                      .Where(uc => uc.UserID == usuario.Id)
                                      .Select(uc => uc.Curso)
                                      .ToList();

                usuariosConCursos.Add(new UsuarioConCursosViewModel
                {
                    Usuario = usuario,
                    Cursos = cursosUsuario
                });
            }

            ViewBag.UsuariosConCursos = usuariosConCursos;
            ViewBag.Cursos = cursosDisponibles;

            return View();
        }

        public ActionResult Estudiante()
        {
            int userID = (int)Session["UserID"]; // Recupera el UserID de la sesión

            var cursosUsuario = db.InscripcionMaterias
                                  .Where(uc => uc.UserID == userID)
                                  .Select(uc => uc.Curso)
                                  .ToList();

            var cursosDisponibles = db.Cursos.ToList();
            ViewBag.Cursos = cursosDisponibles;

            return View(cursosUsuario);
        }       

        [HttpPost]
        public ActionResult AgregarCurso(int cursoSeleccionado)
        {
            int userID = (int)Session["UserID"]; // Recupera el UserID de la sesión

            // Verifica si el usuario ya tiene el curso asociado
            if (!db.InscripcionMaterias.Any(uc => uc.UserID == userID && uc.CursoID == cursoSeleccionado))
            {
                var inscripcion = new InscripcionMateria
                {
                    UserID = userID,
                    CursoID = cursoSeleccionado
                };

                db.InscripcionMaterias.Add(inscripcion);
                db.SaveChanges();
            }

            return RedirectToAction("Estudiante", "Home");
        }

        [HttpPost]
        public ActionResult AsignarCurso(int cursoID, int cursoSeleccionado)
        {
            var curso = db.Cursos.Find(cursoSeleccionado);
            var usuarioCurso = new InscripcionMateria
            {
                CursoID = cursoID,
                UserID = cursoSeleccionado
            };
            db.InscripcionMaterias.Add(usuarioCurso);
            db.SaveChanges();

            return RedirectToAction("AdminEstudiantes", "Home");
        }
    }
}