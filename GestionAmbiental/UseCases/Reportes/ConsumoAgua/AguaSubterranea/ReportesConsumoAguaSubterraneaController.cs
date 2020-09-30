using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.ConsumoAgua.Items;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.Reportes.ConsumoAgua.AguaSubterranea
{
    public class ReportesConsumoAguaSubterraneaController : Controller
    {
        private readonly ISelectItems _selectItems;

        public ReportesConsumoAguaSubterraneaController(ISelectItems selectItems)
        {
            _selectItems = selectItems;
        }

        public async Task <IActionResult> Resumen()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var itemsFiltro = await _selectItems.GetSelectItems();

                var resoluciones = itemsFiltro.ItemsResoluciones.Where(x => x.FuenteAgua.OrigenAgua.Descripcion.Equals("Agua de Pozo")).Select(g => new SelectListItem() { Value = g.IdResolucionConsumoAgua.ToString(), Text = g.Resolucion }).ToList();
                resoluciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Resoluciones = resoluciones;
                ViewBag.FechaInicio = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                ViewBag.FechaFin = DateTime.Now.ToString("yyyy-MM-dd");

                return View();
            }
        }
    }
}
