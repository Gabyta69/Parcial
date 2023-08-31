using Parcial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parcial.ViewModel
{
    public class UsuarioConCursosViewModel
    {
        public Usuario Usuario { get; set; }
        public List<Curso> Cursos { get; set; }
    }
}