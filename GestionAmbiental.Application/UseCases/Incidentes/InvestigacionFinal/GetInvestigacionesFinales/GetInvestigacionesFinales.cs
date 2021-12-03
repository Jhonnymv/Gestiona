using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetRolUsuario;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetInvestigacionesFinales
{
    public class GetInvestigacionesFinales : IGetInvestigacionesFinales
    {
        private readonly IGetInvestigacionFinalReadOnlyRepository _getInvestigacionFinalReadOnlyRepository;
        private readonly IGetRolUsuario _getRolUsuario;

        public GetInvestigacionesFinales(IGetInvestigacionFinalReadOnlyRepository getInvestigacionFinalReadOnlyRepository, IGetRolUsuario getRolUsuario)
        {
            _getInvestigacionFinalReadOnlyRepository = getInvestigacionFinalReadOnlyRepository;
            _getRolUsuario = getRolUsuario;
        }

        public async Task<InvestigacionesFinalesOutput> Execute(string fecha_inicio, string fecha_fin, int[] gerencia, int[] titular, string[] estado, int[] responsable, int[] asesor, string codigo, int pagina, int rpp, string usuarioRed)
        {
            DateTime? fechaInicioFiltro = null;
            DateTime? fechaFinFiltro = null;

            if (fecha_inicio != null)
                fechaInicioFiltro = Convert.ToDateTime(fecha_inicio);
            if (fecha_fin != null)
                fechaFinFiltro = Convert.ToDateTime(fecha_fin);

            var estados = new List<string>();
            if (estado.Length > 0)
            {
                if (estado.Contains("Pendiente"))
                {
                    estados.Add(EstadosIncidentes.Registrado.ToString());
                    estados.Add(EstadosIncidentes.Ampliado.ToString());
                    estados.Add(EstadosIncidentes.Rechazado.ToString());
                }
                if (estado.Contains("Verificar"))
                {
                    estados.Add(EstadosIncidentes.Implementado.ToString());
                }
                if (estado.Contains("Vencido"))
                {
                    estados.Add(EstadosIncidentes.Vencido.ToString());
                }
                if (estado.Contains("Aprobado"))
                {
                    estados.Add(EstadosIncidentes.Aprobado.ToString());
                }
            }

            var data = await _getInvestigacionFinalReadOnlyRepository.GetInvestigacionesFinales(fechaInicioFiltro, fechaFinFiltro, gerencia, titular, estados, responsable, asesor, codigo);

            var output = new InvestigacionesFinalesOutput();

            var rol = await _getRolUsuario.Execute(usuarioRed);
            if (rol.HasValue && rol.Value.Equals(InspeccionRol.Admin))
                output.PermisoAdmin = true;
            else
                output.PermisoAdmin = false;

            output.Investigaciones = new List<InvestigacionFinalOutput>();
            output.NInvestigaciones = data.Count;

            data = data.Skip((pagina - 1) * rpp).Take(rpp).ToList();
            foreach (var item in data)
            {
                var outputItem = new InvestigacionFinalOutput(item);
                output.Investigaciones.Add(outputItem);
            }

            return output;
        }
    }
}
