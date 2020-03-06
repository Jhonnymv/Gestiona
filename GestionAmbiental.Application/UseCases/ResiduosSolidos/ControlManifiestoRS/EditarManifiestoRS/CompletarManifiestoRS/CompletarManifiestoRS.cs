using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.EditarManifiestoRS.CompletarManifiestoRS
{
    public class CompletarManifiestoRS : ICompletarManifiestoRS
    {
        private readonly IEditarManifiestoRSReadOnlyRepository _editarManifiestoRSReadOnlyRepository;

        public async Task Execute(int Id_Manifiesto_RS, string Certificado_Trat_Disp, double Cantidad_RS_Dispuesto, string Fecha_Devolucion, string Informe_Transporte, string Manifiesto, int IdAsesorMAP)
        {
            var fecha_format = DateTime.Parse(Fecha_Devolucion);
            var input = new Domain.ResiduosSolidos.Manifiesto_RS();
            input.Id_Manifiesto_RS = Id_Manifiesto_RS;
            input.Certificado_Trat_Disp = Certificado_Trat_Disp;
            input.Cantidad_RS_Dispuesto = Cantidad_RS_Dispuesto;
            input.Fecha_Devolucion = fecha_format;
            input.Informe_Transporte = Informe_Transporte;
            input.Manifiesto = Manifiesto;
            input.Asesor_MAP = new Domain.ValueObjects.Usuario { IdUsuario = IdAsesorMAP };
            await _editarManifiestoRSReadOnlyRepository.CompletarManifiestoRS(input);
        }
    }
}
