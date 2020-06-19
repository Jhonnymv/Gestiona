using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ControlIngresoSalida.GetCantidadResiduoNoPeligroso;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ControlIngresoSalida.GetCantidadResiduoPeligroso;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ControlIngresoSalida.GetRegistrosACR_CMRS;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ControlIngresoSalida.GetRegistrosEntreFechas;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ControlIngresoSalida.GetRegistrosIngresoSalida;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ControlIngresoSalida.GetRegistrosMotivo;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ControlIngresoSalida.GetRegistrosTipoResiduo;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.GetSelectItems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.Reportes.ResiduosSolidos.ControlIngresoSalida
{
    public class ReportesIngresoSalidaController : Controller
    {
        private readonly IGetSelectItems _getSelectItems;
        private readonly IGetRegistroEntreFechas _getRegistroEntreFechas;
        private readonly IGetRegistrosMotivo _getRegistrosMotivo;
        private readonly IGetRegistrosTipoResiduo _getRegistrosTipoResiduo;
        private readonly IGetRegistroACR_CMRS _getRegistroACR_CMRS;
        private readonly IGetRegistrosIngresoSalida _getRegistrosIngresoSalida;
        private readonly IGetCantidadResiduoNoPeligroso _getCantidadResiduoNoPeligroso;
        private readonly IGetCantidadResiduoPeligroso _getCantidadResiduoPeligroso;

        public ReportesIngresoSalidaController(IGetSelectItems getSelectItems, IGetRegistroEntreFechas getRegistroEntreFechas, IGetRegistrosMotivo getRegistrosMotivo, IGetRegistrosTipoResiduo getRegistrosTipoResiduo, IGetRegistroACR_CMRS getRegistroACR_CMRS, IGetRegistrosIngresoSalida getRegistrosIngresoSalida, IGetCantidadResiduoNoPeligroso getCantidadResiduoNoPeligroso, IGetCantidadResiduoPeligroso getCantidadResiduoPeligroso)
        {
            _getSelectItems = getSelectItems;
            _getRegistroEntreFechas = getRegistroEntreFechas;
            _getRegistrosMotivo = getRegistrosMotivo;
            _getRegistrosTipoResiduo = getRegistrosTipoResiduo;
            _getRegistroACR_CMRS = getRegistroACR_CMRS;
            _getRegistrosIngresoSalida = getRegistrosIngresoSalida;
            _getCantidadResiduoNoPeligroso = getCantidadResiduoNoPeligroso;
            _getCantidadResiduoPeligroso = getCantidadResiduoPeligroso;
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

                var items = await _getSelectItems.Execute();
                var acr_cmrs = items.ItemsACR_CMRS.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_ACR_CMRS.ToString(), Text = g.Descripcion }).ToList();
                acr_cmrs.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var ingreso_salida = items.ItemsIngresoSalida.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Ingreso_Salida.ToString(), Text = g.Descripcion }).ToList();
                ingreso_salida.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var motivo = items.ItemsMotivoIngresoSalida.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Motivo.ToString(), Text = g.Descripcion }).ToList();
                motivo.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var tipo_residuo = items.ItemsTipoResiduo.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Tipo_Residuo.ToString(), Text = g.Descripcion }).ToList();
                tipo_residuo.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var empresa = items.ItemsEmpresa.OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                empresa.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var gerencias = items.ItemsGerencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                gerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var residuosp = items.ItemsResiduoSolido.Where(x=>x.Tipo_Residuo.Descripcion.Equals("Peligroso")).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Residuo_Solido.ToString(), Text = g.Descripcion }).ToList();
                var residuosnp = items.ItemsResiduoSolido.Where(x=>x.Tipo_Residuo.Descripcion.Equals("No Peligroso")).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Residuo_Solido.ToString(), Text = g.Descripcion }).ToList();

                ViewBag.ACR_CMRS = acr_cmrs;
                ViewBag.IngresoSalida = ingreso_salida;
                ViewBag.Motivo = motivo;
                ViewBag.TipoResiduo = tipo_residuo;
                ViewBag.Empresa = empresa;
                ViewBag.Gerencias = gerencias;
                ViewBag.FechaInicio = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                ViewBag.FechaFin = DateTime.Now.ToString("yyyy-MM-dd");
                ViewBag.ResiduosPeligrosos = residuosp;
                ViewBag.ResiduosNoPeligrosos = residuosnp;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetRegistrosEntreFechas(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int gerencia, int empresa, string frecuencia)
        {
            var nRegistros = await _getRegistroEntreFechas.Execute(acr_cmrs,ingreso_salida,tipo_residuo,motivo, fechaInicio, fechaFin, gerencia,empresa, frecuencia);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetRegistrosACR_CRMS(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int gerencia, int empresa)
        {
            var nRegistros = await _getRegistroACR_CMRS.Execute(acr_cmrs, ingreso_salida, tipo_residuo, motivo, fechaInicio, fechaFin, gerencia, empresa);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetRegistrosIngresoSalida(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int gerencia, int empresa)
        {
            var nRegistros = await _getRegistrosIngresoSalida.Execute(acr_cmrs, ingreso_salida, tipo_residuo, motivo, fechaInicio, fechaFin, gerencia, empresa);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetRegistrosTipoResiduo(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int gerencia, int empresa)
        {
            
            var nRegistros = await _getRegistrosTipoResiduo.Execute(acr_cmrs, ingreso_salida, tipo_residuo, motivo, fechaInicio, fechaFin, gerencia, empresa);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetRegistrosMotivo(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int gerencia, int empresa)
        {
            var nRegistros = await _getRegistrosMotivo.Execute(acr_cmrs, ingreso_salida, tipo_residuo, motivo, fechaInicio, fechaFin, gerencia, empresa);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetCantidadResiduoNoPeligroso(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int gerencia, int empresa, int[] residuos)
        {
            var nRegistros = await _getCantidadResiduoNoPeligroso.Execute(acr_cmrs, ingreso_salida, tipo_residuo, motivo, fechaInicio, fechaFin, gerencia, empresa,residuos);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetCantidadResiduoPeligroso(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int gerencia, int empresa, int[] residuos)
        {
            var nRegistros = await _getCantidadResiduoPeligroso.Execute(acr_cmrs, ingreso_salida, tipo_residuo, motivo, fechaInicio, fechaFin, gerencia, empresa,residuos);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }

    }
}