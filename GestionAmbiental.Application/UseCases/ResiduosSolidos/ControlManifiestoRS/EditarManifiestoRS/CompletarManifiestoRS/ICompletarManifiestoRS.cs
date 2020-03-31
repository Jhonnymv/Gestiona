using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.EditarManifiestoRS.CompletarManifiestoRS
{
    public interface ICompletarManifiestoRS
    {
        Task Execute(
            int Id_Manifiesto_RS,
            string opcion,
            int Serie_Manifiesto,
            string Num_Manifiesto,
            string Fecha_Emision_Manifiesto,
            string Observacion_Manifiesto,
            int Id_Residuo_Solido,
            double Cantidad_RS_Evacuado,
            int Id_EO_RS_Salida,
            string Registro_EO_RS_Salida,
            string Fecha_Vencimiento_Registro_Salida,
            string Num_Autorizacion_Municipal_Salida,
            string Num_Autorizacion_Hoja_Ruta,
            int Id_Conductor,
            string Num_Licencia_Condutor,
            int Id_Vehiculo,
            double Capacidad_Vehiculo,
            int Id_EO_RS_Llegada,
            string Registro_EO_RS_Llegada,
            string Fecha_Vencimiento_Registro_Llegada,
            string Num_Autorizacion_Sanitaria_Llegada,
            string Num_Autorizacion_Municipal_Llegada,
            List<string> Certificado_Trat_Disp,
            double Cantidad_RS_Dispuesto,
            string Fecha_Devolucion, 
            List<string> Informe_Transporte,
            List<string> Manifiesto,
            int IdAsesorMAP,
            string usuario_red);
    }
}
