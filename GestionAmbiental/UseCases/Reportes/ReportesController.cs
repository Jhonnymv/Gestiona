using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GestionAmbiental.UseCases.Reportes
{
    public class ReportesController : Controller
    {
        public IActionResult Inspecciones()
        {
            return View();
        }
    }
}