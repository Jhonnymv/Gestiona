using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ConsultarCabeceras
{
    public interface IConsultarCabeceraImplementacion
    {
        Task<InspeccionCabecera> Execute(int idCabecera);
    }
}
