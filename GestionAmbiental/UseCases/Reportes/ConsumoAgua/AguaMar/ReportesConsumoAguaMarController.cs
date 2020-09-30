using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.ConsumoAgua.Items;
using GestionAmbiental.Application.UseCases.Reportes.ConsumoAgua.ConsumoAguaMar.GetVolumenFecha;
using GestionAmbiental.Application.UseCases.Reportes.ConsumoAgua.ConsumoAguaMar.GetVolumenRestante;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.Reportes.ConsumoAgua.AguaMar
{
    public class ReportesConsumoAguaMarController : Controller
    {
        private readonly ISelectItems _selectItems;
        private readonly IGetVolumenFecha _getVolumenFecha;
        private readonly IGetVolumenRestante _getVolumenRestante;

        public ReportesConsumoAguaMarController(ISelectItems selectItems,
            IGetVolumenFecha getVolumenFecha,
            IGetVolumenRestante getVolumenRestante)
        {
            _selectItems = selectItems;
            _getVolumenFecha = getVolumenFecha;
            _getVolumenRestante = getVolumenRestante;
        }

        public async Task<IActionResult> Resumen()
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

                var resoluciones = itemsFiltro.ItemsResoluciones.Where(x => x.FuenteAgua.OrigenAgua.Descripcion.Equals("Agua de Mar")).Select(g => new SelectListItem() { Value = g.IdResolucionConsumoAgua.ToString(), Text = g.Resolucion }).ToList();
                resoluciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Resoluciones = resoluciones;
                ViewBag.FechaInicio = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                ViewBag.FechaFin = DateTime.Now.ToString("yyyy-MM-dd");

                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetVolumenFecha(int resolucion, string fechaInicio, string fechaFin, string frecuencia)
        {
            var registros = await _getVolumenFecha.Execute(resolucion, fechaInicio, fechaFin, frecuencia);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetVolumenRestante(int resolucion)
        {
            var registros = await _getVolumenRestante.Execute(resolucion);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
    }
}
