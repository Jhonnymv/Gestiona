using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.RegistrarInspeccion
{
    public class RegistrarInspeccionCabecera : IRegistrarInspeccionCabecera
    {
        private readonly IRegistarInspeccionReadOnlyRepository _registarInspeccionReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;
        public RegistrarInspeccionCabecera(IRegistarInspeccionReadOnlyRepository registarInspeccionReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _registarInspeccionReadOnlyRepository = registarInspeccionReadOnlyRepository;
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository;
        }


        public async Task<int> Execute(string objetivo, string usuario_red, int gerenciaMarc, int empresaMarc, string lugar, string fechaInspeccion, string hora, string tipoInspeccion, int responsableLugar, string responsableInspeccion, string inspectores, string opcion)
        {
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            var cabecera = new NuevaInspeccionCabecera();
            cabecera.Id_Gerencia_Ejecutora = gerenciaMarc;
            cabecera.Id_Registrado_Por = usuario.Id_Usuario;
            cabecera.Id_Empresa_Contratista = empresaMarc;
            cabecera.Fecha_Inspeccion = Convert.ToDateTime(fechaInspeccion + " " + hora);
            cabecera.Lugar_Inspeccionado = lugar;
            cabecera.Tipo_Inspeccion = tipoInspeccion;
            cabecera.Id_Responsable_Lugar = responsableLugar;
            cabecera.Responsable_Inspeccion = responsableInspeccion;
            cabecera.Inspectores = inspectores;
            cabecera.Objetivo = objetivo;
            switch (opcion)
            {
                case "Observacion": cabecera.Estado = EstadosInspeccion.Pendiente.ToString(); cabecera.Codigo = "TBD"; break;
                case "Guardar": cabecera.Estado = EstadosInspeccion.Registrado.ToString(); cabecera.Codigo = "CÓDIGO"; break;
                case "GuardarEstado": cabecera.Estado = EstadosInspeccion.Pendiente.ToString(); cabecera.Codigo = "TBD"; break;
            }

            int idCabecera = await _registarInspeccionReadOnlyRepository.RegistrarInspeccionCabecera(cabecera);
            return idCabecera;
        }
    }
}
