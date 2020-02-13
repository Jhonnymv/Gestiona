using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.ConsultarObservacion
{
    public interface IConsultarObservacion
    {
        Task<InspeccionDetalleOutput> Execute(int IdObservacion);
    }
}
