using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GestionAmbiental.UseCases.ConsumoAgua.AguaSubterranea
{
    public class ConsumoAguaSubterraneaController : Controller
    {
        public IActionResult Resumen()
        {
            return View();
        }
    }
}
