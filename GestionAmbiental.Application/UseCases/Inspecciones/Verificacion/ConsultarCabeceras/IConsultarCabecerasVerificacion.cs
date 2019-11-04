using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Verificacion.ConsultarCabeceras
{
    public interface IConsultarCabecerasVerificacion
    {
        Task<List<InspeccionCabecera>> Execute(int pagina, int fpp, DateTime fecha, int gerencia, string codigo);
    }
}
