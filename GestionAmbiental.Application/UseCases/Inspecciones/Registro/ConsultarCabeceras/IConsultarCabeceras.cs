using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.ConsultarCabeceras
{
    public interface IConsultarCabeceras
    {
        Task<InspeccionesCabeceraOutput> Execute(int pagina, int fpp, DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] empresaFiltro, int[] registradoPorFiltro, string[] estadoFiltro, int[] responsableFiltro, int[] asesorFiltro,string codigo);

    }
}
