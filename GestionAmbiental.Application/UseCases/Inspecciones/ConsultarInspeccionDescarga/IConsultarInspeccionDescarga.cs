using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.ConsultarInspeccionDescarga
{
    public interface IConsultarInspeccionDescarga
    {
        Task<InspeccionCabecera> Execute(int idCabecera);
        Task<List<InspeccionCabecera>> Execute(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin,string estado, int idGerenciaInspeccionada);
    }
}
