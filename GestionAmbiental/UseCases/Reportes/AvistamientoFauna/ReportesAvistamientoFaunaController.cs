using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.Items;
using GestionAmbiental.Application.UseCases.Reportes.AvistamientoFauna.GetNAvistamientosFauna;
using GestionAmbiental.Application.UseCases.Reportes.AvistamientoFauna.GetNEstadoFauna;
using GestionAmbiental.Application.UseCases.Reportes.AvistamientoFauna.GetNNombreComun;
using GestionAmbiental.Application.UseCases.Reportes.AvistamientoFauna.GetNTipoAvistamiento;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.Reportes.AvistamientoFauna
{
    public class ReportesAvistamientoFaunaController : Controller
    {
        private readonly IGetItems _getItems;
        private readonly IGetNAvistamientoFauna _getNAvistamientoFauna;
        private readonly IGetNEstadoFauna _getNEstadoFauna;
        private readonly IGetNNombreComun _getNNombreComun;
        private readonly IGetNTipoAvistamiento _getNTipoAvistamiento;

        public ReportesAvistamientoFaunaController(IGetItems getItems, IGetNAvistamientoFauna getNAvistamientoFauna, IGetNEstadoFauna getNEstadoFauna, IGetNNombreComun getNNombreComun, IGetNTipoAvistamiento getNTipoAvistamiento)
        {
            _getItems = getItems;
            _getNAvistamientoFauna = getNAvistamientoFauna;
            _getNEstadoFauna = getNEstadoFauna;
            _getNNombreComun = getNNombreComun;
            _getNTipoAvistamiento = getNTipoAvistamiento;
        }

        public async Task<IActionResult> Resumen()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var itemsFiltro = await _getItems.Execute();

                var nombresComunes = itemsFiltro.NombresComunes.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdNombreComunFauna.ToString(), Text = g.Descripcion }).ToList();
                nombresComunes.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposAvistamiento = itemsFiltro.TiposAvistamiento.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoAvistamiento.ToString(), Text = g.Descripcion }).ToList();
                tiposAvistamiento.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var estadosFauna = itemsFiltro.EstadosFauna.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdEstadoFauna.ToString(), Text = g.Descripcion }).ToList();
                estadosFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var lugaresReferencia = itemsFiltro.LugaresReferencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdLugarReferencia.ToString(), Text = g.Descripcion }).ToList();
                lugaresReferencia.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.FiltroNombresComunes = nombresComunes;
                ViewBag.FiltroTiposAvistamiento = tiposAvistamiento;
                ViewBag.FiltroEstadosFauna = estadosFauna;
                ViewBag.FiltroLugaresReferencia = lugaresReferencia;
                ViewBag.FechaInicio = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                ViewBag.FechaFin = DateTime.Now.ToString("yyyy-MM-dd");

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetAvistamientosFaunaFechas(int nombre_comun, int tipo_avistamiento, int estado_fauna, int lugar_referencia, string fechaInicio, string fechaFin, string frecuencia)
        {
            var registros = await _getNAvistamientoFauna.Execute(nombre_comun, tipo_avistamiento, estado_fauna, lugar_referencia, fechaInicio, fechaFin, frecuencia);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetAvistamientosNombreComun(int nombre_comun, int tipo_avistamiento, int estado_fauna, int lugar_referencia, string fechaInicio, string fechaFin)
        {
            var registros = await _getNNombreComun.Execute(nombre_comun, tipo_avistamiento, estado_fauna, lugar_referencia, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetAvistamientosEstadoFauna(int nombre_comun, int tipo_avistamiento, int estado_fauna, int lugar_referencia, string fechaInicio, string fechaFin)
        {
            var registros = await _getNEstadoFauna.Execute(nombre_comun, tipo_avistamiento, estado_fauna, lugar_referencia, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetAvistamientosTipoAvistamiento(int nombre_comun, int tipo_avistamiento, int estado_fauna, int lugar_referencia, string fechaInicio, string fechaFin)
        {
            var registros = await _getNTipoAvistamiento.Execute(nombre_comun, tipo_avistamiento, estado_fauna, lugar_referencia, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
    }
}
