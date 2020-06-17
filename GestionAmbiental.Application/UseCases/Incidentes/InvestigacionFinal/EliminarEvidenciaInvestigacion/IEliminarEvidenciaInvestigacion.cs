using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.EliminarEvidenciaInvestigacion
{
    public interface IEliminarEvidenciaInvestigacion
    {
        Task Execute(int IdInvestigacion, string Tipo, string NombreArchivo,string usuario_red);
    }
}
