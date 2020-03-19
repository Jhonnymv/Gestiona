using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ResiduosSolidos.ControlIngresoSalida
{
    public class GestorIngresoSalidaRSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}