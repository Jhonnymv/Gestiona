using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GestionAmbiental.UseCases.ConsumoAgua.AguaMar
{
    public class ConsumoAguaMarController : Controller
    {
        public IActionResult Resumen()
        {
            return View();
        }
        public IActionResult NuevoRegistro()
        {
            return View();
        }
    }
}
