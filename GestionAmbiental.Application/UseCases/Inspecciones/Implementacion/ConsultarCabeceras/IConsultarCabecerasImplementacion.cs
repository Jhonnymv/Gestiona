using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ConsultarCabeceras
{
    public interface IConsultarCabecerasImplementacion
    {
        Task<InspeccionesCabeceraOutput> Execute(int pagina, int fpp, DateTime fecha, int empresa,int reg_por, string codigo,string usuario_red);
    }
}
