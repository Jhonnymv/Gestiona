using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.EditarManifiestoRS.CompletarManifiestoRS
{
    public interface ICompletarManifiestoRS
    {
        Task Execute(int Id_Manifiesto_RS, List<string> Certificado_Trat_Disp, double Cantidad_RS_Dispuesto, string Fecha_Devolucion, List<string> Informe_Transporte, List<string> Manifiesto, int IdAsesorMAP,string usuario_red);
    }
}
