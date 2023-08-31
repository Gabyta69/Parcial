using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Parcial.Models;

namespace Parcial.Controllers
{

    public class EstudianteController : Controller
    {

        private RC101320Entities1 db = new RC101320Entities1();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminEstudiantes()
        {
            var cursos = db.Cursos.ToList();
            return View(cursos);
        }

        public ActionResult Estudiantes()
        {
            var cursos = db.Cursos.ToList();
            return View(cursos);
        }

        public ActionResult AgregarCurso()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarCurso(Curso cursos)
        {
            if (ModelState.IsValid)
            {
                db.Cursos.Add(cursos);
                db.SaveChanges();
                return RedirectToAction("Index", "Estudiantes");
            }
            return View(cursos);
        }
    }
}
