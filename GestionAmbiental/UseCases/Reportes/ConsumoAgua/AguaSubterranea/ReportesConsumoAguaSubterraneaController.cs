using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.ConsumoAgua.Items;
using GestionAmbiental.Application.UseCases.Reportes.ConsumoAgua.ConsumoAguaSubterranea.GetCaudalPromedioFecha;
using GestionAmbiental.Application.UseCases.Reportes.ConsumoAgua.ConsumoAguaSubterranea.GetTiempoBombeo;
using GestionAmbiental.Application.UseCases.Reportes.ConsumoAgua.ConsumoAguaSubterranea.GetVolumenFecha;
using GestionAmbiental.Application.UseCases.Reportes.ConsumoAgua.ConsumoAguaSubterranea.GetVolumenRestante;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.Reportes.ConsumoAgua.AguaSubterranea
{
    public class ReportesConsumoAguaSubterraneaController : Controller
    {
        private readonly ISelectItems _selectItems;
        private readonly IGetVolumenFecha _getVolumenFecha;
        private readonly IGetVolumenRestante _getVolumenRestante;
        private readonly IGetTiempoBombeo _getTiempoBombeo;
        private readonly IGetCaudalPromedioFecha _getCaudalPromedioFecha;

        public ReportesConsumoAguaSubterraneaController(
            ISelectItems selectItems, 
            IGetVolumenFecha getVolumenFecha,
            IGetVolumenRestante getVolumenRestante,
            IGetTiempoBombeo getTiempoBombeo,
            IGetCaudalPromedioFecha getCaudalPromedioFecha)
        {
            _selectItems = selectItems;
            _getVolumenFecha = getVolumenFecha;
            _getVolumenRestante = getVolumenRestante;
            _getCaudalPromedioFecha = getCaudalPromedioFecha;
            _getTiempoBombeo = getTiempoBombeo;
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

                var resoluciones = itemsFiltro.ItemsResoluciones.Where(x => x.FuenteAgua.OrigenAgua.Descripcion.Equals("Agua Subterranea")).Select(g => new SelectListItem() { Value = g.IdResolucionConsumoAgua.ToString(), Text = g.Actividad + " (" + g.Resolucion + ")" }).ToList();
                resoluciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Resoluciones = resoluciones; 
                ViewBag.Limites = JsonConvert.SerializeObject(itemsFiltro.ItemsResoluciones.Select(x => new { IdResolucion = x.IdResolucionConsumoAgua, LimiteDiario = x.LimiteDiario, LimiteSemanal = x.LimiteSemanal, LimiteMensual = x.LimiteMensual }));
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
        [HttpPost]
        public async Task<IActionResult> GetCaudalPromedioFecha(int resolucion, string fechaInicio, string fechaFin, string frecuencia)
        {
            var registros = await _getCaudalPromedioFecha.Execute(resolucion, fechaInicio, fechaFin,frecuencia);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetTiempoBombeoFecha(int resolucion, string fechaInicio, string fechaFin)
        {
            var registros = await _getTiempoBombeo.Execute(resolucion, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
    }
}
