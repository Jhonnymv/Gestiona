using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ControlIngresoSalida.GetRegistrosACR_CMRS;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ControlIngresoSalida.GetRegistrosEntreFechas;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ControlIngresoSalida.GetRegistrosIngresoSalida;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ControlIngresoSalida.GetRegistrosMotivo;
using GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ControlIngresoSalida.GetRegistrosTipoResiduo;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.GetSelectItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public ReportesIngresoSalidaController(IGetSelectItems getSelectItems, IGetRegistroEntreFechas getRegistroEntreFechas, IGetRegistrosMotivo getRegistrosMotivo, IGetRegistrosTipoResiduo getRegistrosTipoResiduo, IGetRegistroACR_CMRS getRegistroACR_CMRS, IGetRegistrosIngresoSalida getRegistrosIngresoSalida)
        {
            _getSelectItems = getSelectItems;
            _getRegistroEntreFechas = getRegistroEntreFechas;
            _getRegistrosMotivo = getRegistrosMotivo;
            _getRegistrosTipoResiduo = getRegistrosTipoResiduo;
            _getRegistroACR_CMRS = getRegistroACR_CMRS;
            _getRegistrosIngresoSalida = getRegistrosIngresoSalida;
        }

        public async Task<IActionResult> Resumen()
        {
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
            var usuarios = items.ItemsUsuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
            usuarios.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.ACR_CMRS = acr_cmrs;
            ViewBag.IngresoSalida = ingreso_salida;
            ViewBag.Motivo = motivo;
            ViewBag.TipoResiduo = tipo_residuo;
            ViewBag.Empresa = empresa;
            ViewBag.Usuarios = usuarios;
            ViewBag.FechaInicio = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            ViewBag.FechaFin = DateTime.Now.ToString("yyyy-MM-dd");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetRegistrosEntreFechas(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int responsable, int empresa, string frecuencia)
        {
            //Observaciones x estado filtradas y ordenadas en funcion al Enum EstadosRAC
            var nRegistros = await _getRegistroEntreFechas.Execute(acr_cmrs,ingreso_salida,tipo_residuo,motivo, fechaInicio, fechaFin, responsable,empresa, frecuencia);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetRegistrosACR_CRMS(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int responsable, int empresa)
        {
            //Observaciones x estado filtradas y ordenadas en funcion al Enum EstadosRAC
            var nRegistros = await _getRegistroACR_CMRS.Execute(acr_cmrs, ingreso_salida, tipo_residuo, motivo, fechaInicio, fechaFin, responsable, empresa);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetRegistrosIngresoSalida(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int responsable, int empresa)
        {
            //Observaciones x estado filtradas y ordenadas en funcion al Enum EstadosRAC
            var nRegistros = await _getRegistrosIngresoSalida.Execute(acr_cmrs, ingreso_salida, tipo_residuo, motivo, fechaInicio, fechaFin, responsable, empresa);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetRegistrosTipoResiduo(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int responsable, int empresa)
        {
            //Observaciones x estado filtradas y ordenadas en funcion al Enum EstadosRAC
            var nRegistros = await _getRegistrosTipoResiduo.Execute(acr_cmrs, ingreso_salida, tipo_residuo, motivo, fechaInicio, fechaFin, responsable, empresa);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetRegistrosMotivo(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int responsable, int empresa)
        {
            //Observaciones x estado filtradas y ordenadas en funcion al Enum EstadosRAC
            var nRegistros = await _getRegistrosMotivo.Execute(acr_cmrs, ingreso_salida, tipo_residuo, motivo, fechaInicio, fechaFin, responsable, empresa);

            dynamic response = new
            {
                nRegistros = nRegistros
            };
            return Ok(response);
        }
    }
}