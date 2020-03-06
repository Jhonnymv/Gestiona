using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.RegistrarManifiestoRS
{
    public class RegistrarManifiestoRS : IRegistrarManifiestoRS
    {
        private readonly IRegistrarManifiestoRSReadOnlyRepository _registrarManifiestoRSReadOnlyRepository;
        private readonly IGetManifiestosRSReadOnlyRepository _getManifiestosRSReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public RegistrarManifiestoRS(
            IRegistrarManifiestoRSReadOnlyRepository registrarManifiestoRSReadOnlyRepository, 
            IGetManifiestosRSReadOnlyRepository getManifiestosRSReadOnlyRepository, 
            IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _registrarManifiestoRSReadOnlyRepository = registrarManifiestoRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarManifiestoRSReadOnlyRepository));
            _getManifiestosRSReadOnlyRepository = getManifiestosRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(getManifiestosRSReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task<int> Execute(
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
            string Nombre_Conductor, 
            string Num_Licencia_Condutor, 
            string Placa_Vehiculo, 
            double Capacidad_Vehiculo,
            int Id_EO_RS_Llegada,
            string Registro_EO_RS_Llegada,
            DateTime Fecha_Vencimiento_Registro_Llegada, 
            string Num_Autorizacion_Sanitaria_Llegada,
            string Num_Autorizacion_Municipal_Llegada,
            string usuario_red)
        {
            var input = new Domain.ResiduosSolidos.Manifiesto_RS();

            
            switch (opcion)
            {
                case "Registrar":
                    input.Estado = EstadosResiduosSolidos.Registrado.ToString(); 
                    input.Item = (Convert.ToInt32(await _getManifiestosRSReadOnlyRepository.GetLastItem()) + 1).ToString();
                    break;
                case "GuardarEstado":
                    input.Estado = EstadosResiduosSolidos.Pendiente.ToString();
                    input.Item = "TBD";
                    break;
            }

            input.Id_Registrado_Por = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuario_red);
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
            input.Nombre_Conductor = Nombre_Conductor;
            input.Num_Licencia_Condutor = Num_Licencia_Condutor;
            input.Placa_Vehiculo = Placa_Vehiculo;
            input.Capacidad_Vehiculo = Capacidad_Vehiculo;
            input.EO_RS_Llegada = new Domain.ValueObjects.EO_RS();
            input.EO_RS_Llegada.Id_EO_RS = Id_EO_RS_Llegada;
            input.Registro_EO_RS_Llegada = Registro_EO_RS_Llegada;
            input.Fecha_Vencimiento_Registro_Llegada = Fecha_Vencimiento_Registro_Llegada;
            input.Num_Autorizacion_Sanitaria_Llegada = Num_Autorizacion_Sanitaria_Llegada;
            input.Num_Autorizacion_Municipal_Llegada = Num_Autorizacion_Municipal_Llegada;

            return await _registrarManifiestoRSReadOnlyRepository.RegistrarManifiestosRS(input);
        }
    }
}
