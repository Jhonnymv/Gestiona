using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;

namespace GestionAmbiental.Application.UseCases.Inspecciones.RegistrarInspeccion
{
    public class RegistrarInspeccionCabecera : IRegistrarInspeccionCabecera
    {
        private readonly IRegistarInspeccionReadOnlyRepository _registarInspeccionReadOnlyRepository;

        public RegistrarInspeccionCabecera(IRegistarInspeccionReadOnlyRepository registarInspeccionReadOnlyRepository)
        {
            _registarInspeccionReadOnlyRepository = registarInspeccionReadOnlyRepository;
        }

        public async Task<int> Execute(string objetivo, int gerenciaMarc, int empresaMarc, int gerenciaEPCM, int empresaEPCM, string lugar, string fechaInspeccion, string hora, int responsableLugar, string inspectores, string opcion)
        {
            var cabecera = new NuevaInspeccionCabecera();
            cabecera.Id_Gerencia_Ejecutora = gerenciaMarc;
            cabecera.Id_Gerencia_EPCM = gerenciaEPCM;
            cabecera.Id_Empresa_Contratista = empresaMarc;
            cabecera.Id_Empresa_Contratista_EPCM = empresaEPCM;
            cabecera.Fecha_Inspeccion = Convert.ToDateTime(fechaInspeccion + " " + hora);
            cabecera.Lugar_Inspeccionado = lugar;
            cabecera.Id_Responsable_Lugar = responsableLugar;
            cabecera.Inspectores = inspectores;
            cabecera.Objetivo = objetivo;
            switch (opcion)
            {
                case "Observacion": cabecera.Estado = EstadosInspeccion.Pendiente.ToString();break;
                case "Guardar": cabecera.Estado = EstadosInspeccion.Registrado.ToString(); break;
                case "GuardarEstado": cabecera.Estado = EstadosInspeccion.Pendiente.ToString();break;
            }

            int idCabecera = await _registarInspeccionReadOnlyRepository.RegistrarInspeccionCabecera(cabecera);
            return idCabecera;
        }
    }
}
