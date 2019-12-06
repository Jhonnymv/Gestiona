using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.ConsultarCabeceras
{
    public interface IConsultarCabecera
    {
        Task<InspeccionCabecera> Execute(int idCabecera);
    }
}
