using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales
{
    public class InspeccionesController : Controller
    {
        public IActionResult Resumen()
        {
            return View();
        }
    }
}