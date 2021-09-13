using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ManifiestoRS.GetManifiestosCamion;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ManifiestoRS.GetManifiestosEntreFechas;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ManifiestoRS.GetManifiestosObservacion;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ManifiestoRS.GetManifiestosResiduoSolidoMensual;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ManifiestoRS.GetManifiestosTipoLlegada;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ManifiestoRS.GetManifiestosTipoSalida;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.GetSelectItems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.Reportes.ResiduosSolidos.ControlManifiestoRS
{
    public class ReportesManifiestoRSController : Controller
    {
        private readonly IGetSelectItems _getSelectItems;
        private readonly IGetManifiestosEntreFechas _getManifiestosEntreFechas;
        private readonly IGetManifiestosObservacion _getManifiestosObservacion;
        private readonly IGetManifiestosCamion _getManifiestosCamion;
        private readonly IGetManifiestosTipoLlegada _getManifiestosTipoLlegada;
        private readonly IGetManifiestosTipoSalida _getManifiestosTipoSalida;
        private readonly IGetManifiestosResiduoSolidoMensual _getManifiestosResiduoSolidoMensual;

        public ReportesManifiestoRSController(IGetSelectItems getSelectItems, IGetManifiestosEntreFechas getManifiestosEntreFechas, IGetManifiestosObservacion getManifiestosObservacion, IGetManifiestosCamion getManifiestosCamion, IGetManifiestosTipoLlegada getManifiestosTipoLlegada, IGetManifiestosTipoSalida getManifiestosTipoSalida, IGetManifiestosResiduoSolidoMensual getManifiestosResiduoSolidoMensual)
        {
            _getSelectItems = getSelectItems;
            _getManifiestosEntreFechas = getManifiestosEntreFechas;
            _getManifiestosObservacion = getManifiestosObservacion;
            _getManifiestosCamion = getManifiestosCamion;
            _getManifiestosTipoLlegada = getManifiestosTipoLlegada;
            _getManifiestosTipoSalida = getManifiestosTipoSalida;
            _getManifiestosResiduoSolidoMensual = getManifiestosResiduoSolidoMensual;
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

                var items = await _getSelectItems.Execute();
                var residuo_solido = items.ItemsResiduoSolido.Where(x => x.Tipo_Residuo.Descripcion.Equals("Peligroso")).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Residuo_Solido.ToString(), Text = g.Descripcion }).ToList();                
                residuo_solido.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var eors = items.ItemsEO_RS.OrderBy(x => x.Razon_Social).Select(g => new SelectListItem() { Value = g.Id_EO_RS.ToString(), Text = g.Razon_Social }).ToList();
                eors.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var residuo_solido_mensual = items.ItemsResiduoSolido.Where(x=>x.Tipo_Residuo.Descripcion.Equals("Peligroso")).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Residuo_Solido.ToString(), Text = g.Descripcion }).ToList();

                ViewBag.ResiduoSolido = residuo_solido;
                ViewBag.ResiduoSolidoMensual = residuo_solido_mensual;
                ViewBag.EORS = eors;
                ViewBag.FechaInicio = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                ViewBag.FechaFin = DateTime.Now.ToString("yyyy-MM-dd");
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetRegistrosEntreFechas(string fechaInicio, string fechaFin, string observacion,int residuo_solido,int eors_salida, int eors_llegada, string frecuencia)
        {
            //Observaciones x estado filtradas y ordenadas en funcion al Enum EstadosRAC
            var nRegistros = await _getManifiestosEntreFechas.Execute(fechaInicio,fechaFin,observacion,residuo_solido,eors_salida,eors_llegada, frecuencia);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetRegistrosObservacion(string fechaInicio, string fechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada)
        {
            //Observaciones x estado filtradas y ordenadas en funcion al Enum EstadosRAC
            var nRegistros = await _getManifiestosObservacion.Execute(fechaInicio, fechaFin, observacion, residuo_solido, eors_salida, eors_llegada);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetRegistrosCamion(string fechaInicio, string fechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada)
        {
            //Observaciones x estado filtradas y ordenadas en funcion al Enum EstadosRAC
            var nRegistros = await _getManifiestosCamion.Execute(fechaInicio, fechaFin, observacion, residuo_solido, eors_salida, eors_llegada);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetRegistrosTipoLlegada(string fechaInicio, string fechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada)
        {
            //Observaciones x estado filtradas y ordenadas en funcion al Enum EstadosRAC
            var nRegistros = await _getManifiestosTipoLlegada.Execute(fechaInicio, fechaFin, observacion, residuo_solido, eors_salida, eors_llegada);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetRegistrosTipoSalida(string fechaInicio, string fechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada)
        {
            //Observaciones x estado filtradas y ordenadas en funcion al Enum EstadosRAC
            var nRegistros = await _getManifiestosTipoSalida.Execute(fechaInicio, fechaFin, observacion, residuo_solido, eors_salida, eors_llegada);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetManifiestosResiduoSolidoMensual(string fechaInicio, string fechaFin, string observacion, int[] residuo_solido, int eors_salida, int eors_llegada)
        {
            //Observaciones x estado filtradas y ordenadas en funcion al Enum EstadosRAC
            var nRegistros = await _getManifiestosResiduoSolidoMensual.Execute(fechaInicio, fechaFin, observacion, residuo_solido, eors_salida, eors_llegada);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }

    }
}