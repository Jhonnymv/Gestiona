using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using GestionAmbiental.Domain.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.EditarManifiestoRS.CompletarManifiestoRS
{
    public class CompletarManifiestoRS : ICompletarManifiestoRS
    {
        private readonly IEditarManifiestoRSReadOnlyRepository _editarManifiestoRSReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public CompletarManifiestoRS(IEditarManifiestoRSReadOnlyRepository editarManifiestoRSReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _editarManifiestoRSReadOnlyRepository = editarManifiestoRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarManifiestoRSReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(
            int Id_Manifiesto_RS,
            string opcion,
            int Serie_Manifiesto,
            string Num_Manifiesto,
            DateTime Fecha_Emision_Manifiesto,
            string Observacion_Manifiesto,
            int Id_Residuo_Solido,
            double Cantidad_RS_Evacuado,
            int Id_EO_RS_Salida,
            string Registro_EO_RS_Salida,
            DateTime Fecha_Vencimiento_Registro_Salida,
            string Num_Autorizacion_Municipal_Salida,
            string Num_Autorizacion_Hoja_Ruta,
            int Id_Conductor,
            string Num_Licencia_Condutor,
            int Id_Vehiculo,
            double Capacidad_Vehiculo,
            int Id_EO_RS_Llegada,
            string Registro_EO_RS_Llegada,
            DateTime Fecha_Vencimiento_Registro_Llegada,
            string Num_Autorizacion_Sanitaria_Llegada,
            string Num_Autorizacion_Municipal_Llegada,
            List<string> Certificado_Trat_Disp,
            double Cantidad_RS_Dispuesto,
            string Fecha_Devolucion,
            List<string> Informe_Transporte,
            List<string> Manifiesto,
            int IdAsesorMAP,
            string usuario_red)
        {
            var fecha_format = DateTime.Parse(Fecha_Devolucion);
            var usuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuario_red);
            var input = new Domain.ResiduosSolidos.Manifiesto_RS();

            switch (opcion)
            {
                case "Completar":
                    input.Estado = EstadosResiduosSolidos.Cerrado.ToString();
                    break;
                case "Pendiente":
                    input.Estado = EstadosResiduosSolidos.Registrado.ToString();
                    break;
            }
            input.Id_Manifiesto_RS = Id_Manifiesto_RS;

            input.Serie_Manifiesto = Serie_Manifiesto;
            input.Num_Manifiesto = Num_Manifiesto;
            input.Fecha_Emision_Manifiesto = Fecha_Emision_Manifiesto;
            input.Observacion_Manifiesto = Observacion_Manifiesto;
            input.Residuo_Solido = new Domain.ValueObjects.Residuo_Solido();
            input.Residuo_Solido.Id_Residuo_Solido = Id_Residuo_Solido;
            input.Cantidad_RS_Evacuado = Cantidad_RS_Evacuado;
            input.EO_RS_Salida = new Domain.ValueObjects.EO_RS();
            input.EO_RS_Salida.Id_EO_RS = Id_EO_RS_Salida;
            input.Registro_EO_RS_Salida = Registro_EO_RS_Salida;
            input.Fecha_Vencimiento_Registro_Salida = Fecha_Vencimiento_Registro_Salida;
            input.Num_Autorizacion_Municipal_Salida = Num_Autorizacion_Municipal_Salida;
            input.Num_Autorizacion_Hoja_Ruta = Num_Autorizacion_Hoja_Ruta;
            input.Conductor = new Domain.ValueObjects.Conductor_RS();
            input.Conductor.Id_Conductor_RS = Id_Conductor;
            input.Num_Licencia_Condutor = Num_Licencia_Condutor;
            input.Vehiculo = new Domain.ValueObjects.Vehiculo_RS();
            input.Vehiculo.Id_Vehiculo_RS = Id_Vehiculo;
            input.Capacidad_Vehiculo = Capacidad_Vehiculo;
            input.EO_RS_Llegada = new Domain.ValueObjects.EO_RS();
            input.EO_RS_Llegada.Id_EO_RS = Id_EO_RS_Llegada;
            input.Registro_EO_RS_Llegada = Registro_EO_RS_Llegada;
            input.Fecha_Vencimiento_Registro_Llegada = Fecha_Vencimiento_Registro_Llegada;
            input.Num_Autorizacion_Sanitaria_Llegada = Num_Autorizacion_Sanitaria_Llegada;
            input.Num_Autorizacion_Municipal_Llegada = Num_Autorizacion_Municipal_Llegada;

            input.Certificado_Trat_Disp = JsonConvert.SerializeObject(Certificado_Trat_Disp);
            input.Cantidad_RS_Dispuesto = Cantidad_RS_Dispuesto;
            input.Fecha_Devolucion = fecha_format;
            input.Informe_Transporte = JsonConvert.SerializeObject(Informe_Transporte);
            input.Manifiesto = JsonConvert.SerializeObject(Manifiesto);
            input.Asesor_MAP = new Domain.ValueObjects.Usuario { IdUsuario = IdAsesorMAP };
            await _editarManifiestoRSReadOnlyRepository.CompletarManifiestoRS(input, usuario);
        }
    }
}
