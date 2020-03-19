using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ACR_CMRS.AgregarACR_CMRS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ACR_CMRS.EditarACR_CMRS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ACR_CMRS.GetACR_CMRS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ACR_CMRS.GetACR_CMRSs;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.MotivoIngresoSalida.AgregarMotivo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.MotivoIngresoSalida.EditarMotivo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.MotivoIngresoSalida.GetMotivo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.MotivoIngresoSalida.GetMotivos;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido.AgregarResiduoSolido;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido.EditarResiduoSolido;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido.GetResiduoSolido;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido.GetResiduosSolidos;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Responsable_ACR_CMRS.AgregarResponsable_ACR_CMRS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Responsable_ACR_CMRS.EditarResponsable_ACR_CMRS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Responsable_ACR_CMRS.GetResponsable_ACR_CMRS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Responsable_ACR_CMRS.GetResponsables_ACR_CMRS;
using Microsoft.AspNetCore.Mvc;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ResiduosSolidos.ControlIngresoSalida
{
    public class GestorIngresoSalidaRSController : Controller
    {
        //ACR_CMRS
        private readonly IAgregarACR_CMRS _agregarACR_CMRS;
        private readonly IEditarACR_CMRS _editarACR_CMRS;
        private readonly IGetACR_CMRS _getACR_CMRS;
        private readonly IGetACR_CMRSs _getACR_CMRSs;

        //MotivoIngresoSalida
        private readonly IAgregarMotivo _agregarMotivo;
        private readonly IEditarMotivo _editarMotivo;
        private readonly IGetMotivo _getMotivo;
        private readonly IGetMotivos _getMotivos;

        //Tipo Residuo
        private readonly IAgregarResiduoSolido _agregarResiduoSolido;
        private readonly IEditarResiduoSolido _editarResiduoSolido;
        private readonly IGetResiduoSolido _getResiduoSolido;
        private readonly IGetResiduosSolidos _getResiduosSolidos;

        //Responsable ACR_CMRS
        private readonly IAgregarResponsableACR_CMRS _agregarResponsableACR_CMRS;
        private readonly IEditarResponsableARC_CMRS _editarResponsableARC_CMRS;
        private readonly IGetResponsableACR_CMRS _getResponsableACR_CMRS;
        private readonly IGetResponsablesACR_CMRS _getResponsablesACR_CMRS;

        public IActionResult Resumen()
        {
            return View();
        }
        public async Task<IActionResult> FiltrarARC_CMRS(string filtro, int pagina, int rpp)
        {
            var data = await _getACR_CMRSs.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla+="<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Código</th>";
         /*       </tr>
            </thead>
            <tbody>
            </tbody>
        </table>";
            var filasTabla = "";
            if (data.nACR_CMRSs > 0)
            {
                foreach (var fila in data.ACR_CMRSs)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    /*filasTabla += "<tr>";
                    var obsAvance = fila.NObsImplementadas + fila.NObsRechazadas + fila.NObsAprobadas;
                    double avance = obsAvance != 0 ? Math.Round((float)obsAvance / fila.NObservaciones * 100, 1) : 0;
                    filasTabla += "<td><a href=\"/Inspecciones/EditarInspeccion?IdCabecera=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                    filasTabla += "<td>" + fila.Gerencia + "</td>";
                    filasTabla += "<td>" + fila.Empresa + "</td>";
                    filasTabla += "<td>" + fila.AreaInspeccionada + "</td>";
                    filasTabla += "<td>" + fila.FechaInspeccion.ToString("dd/MM/yyyy HH:mm") + "</td>";
                    filasTabla += "<td>" + fila.NObservaciones + "</td>";
                    filasTabla += "<td><div class=\"progress\"><div class=\"progress-bar bg-info\" role=\"progressbar\" style=\"width:" + avance + "%; color: black;font-weight: bold;\" aria-valuemin=\"0\" aria-valuemax=\"100\">" + avance + "%</div></div></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                for (int i = 1; i <= nPaginas; i++)
                {
                    if (i == pagina)
                        paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                    else
                        paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                }
                paginador += "</ul>";
            }*/
            dynamic response = new
            {
                
            };
            return Ok(response);

        }
    }
}