using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.ConsultarManifiestosRS
{
    public interface IConsultarManifiestosRS
    {
        Task<ManifiestosRSOutput> Execute(string nManifiesto, string fechaEmision, string observacion, int residuoSolido, int eors_salida, int eors_llegada, int asesorMAP, int pagina, int rpp);
    }
}
