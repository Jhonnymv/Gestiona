using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.DashboardGeneral.ResiduosSolidos.IngresoSalidaRS.GetDestinoxNombreResiudoxIngresoSalida;
using GestionAmbiental.Application.UseCases.DashboardGeneral.ResiduosSolidos.IngresoSalidaRS.GetNombreResiduoxIngresoSalida;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.GetSelectItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GestionAmbiental.UseCases.DashboardGeneral
{
    public class DashboardResiduosController : Controller
    {
        //Ingreso y Salida
        private readonly IGetDestinoxNombreResiduoxIngresoSalida _getDestinoxNombreResiduoxIngresoSalida;
        private readonly IGetNombreResiduoxIngresoSalida _getNombreResiduoxIngresoSalida;

        public DashboardResiduosController(IGetDestinoxNombreResiduoxIngresoSalida getDestinoxNombreResiduoxIngresoSalida, IGetNombreResiduoxIngresoSalida getNombreResiduoxIngresoSalida)
        {
            _getDestinoxNombreResiduoxIngresoSalida = getDestinoxNombreResiduoxIngresoSalida;
            _getNombreResiduoxIngresoSalida = getNombreResiduoxIngresoSalida;
        }
    }
}