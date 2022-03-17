using GestionAmbiental.Application.Repositories.CompromisosMCB.CompromisoCMCB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.CompromisoCMCB.ListarRegistros
{
    public class ListarRegistros : IListarRegistros
    {
        private readonly ICompromisoReadOnlyRepository _compromisoReadOnlyRepository;

        public ListarRegistros(ICompromisoReadOnlyRepository compromisoReadOnlyRepository)
        {
            _compromisoReadOnlyRepository = compromisoReadOnlyRepository ?? throw new ArgumentNullException(nameof(compromisoReadOnlyRepository));
        }

        public async Task<CompromisosOuput> Execute(int pagina, int rpp, int[] gerencias, int[] areas, int[] criterios, string[] estados, int[] etapas)
        {
            //Consultar registros de BD
            var registros = await _compromisoReadOnlyRepository.ListarCompromisos(gerencias, areas, criterios, estados,etapas);

            //Nueva lista en blanco
            var output = new CompromisosOuput();
            output.Registros = new List<CompromisoOuput>();

            //Mapear registros en nueva lista
            foreach (var item in registros)
            {

                var compItem = new CompromisoOuput();
                compItem.Id = item.Id;
                compItem.Codigo = item.Codigo;
                if (item.CriterioCMCB != null)
                {
                    compItem.IdCriterio = item.CriterioCMCB.Id;
                    compItem.NomCriterio = item.CriterioCMCB.Criterio;
                }
                compItem.Detalle_compromiso = item.Detalle_compromiso;
                if (item.EtapaCMCB != null)
                {
                    compItem.IdEtapa = item.EtapaCMCB.Id;
                    compItem.NomEtapa = item.EtapaCMCB.Etapa;
                }
                compItem.Texto_compromiso = item.Texto_compromiso;
                if (item.OrigenCMCB != null)
                {
                    compItem.IdOrigen = item.OrigenCMCB.Id;
                    compItem.NomOrigen = item.OrigenCMCB.Origen;
                };
                compItem.Tomo = item.Tomo;
                compItem.Capitulo = item.Capitulo;
                compItem.Seccion = item.Seccion;
                compItem.Pagina = item.Pagina;
                compItem.Resolucion = item.Resolucion;
                if (item.AutoridadCMCB != null)
                {
                    compItem.IdAutoridad = item.AutoridadCMCB.Id;
                    compItem.NomAutoridad = item.AutoridadCMCB.Autoridad;
                };
                compItem.Fecha = item.Fecha;
                if (item.AspectoCMCB != null)
                {
                    compItem.IdAspecto = item.AspectoCMCB.Id;
                    compItem.NomAspecto = item.AspectoCMCB.Aspecto;
                }
                if (item.ComponenteCMCB != null)
                {
                    compItem.IdComponente = item.ComponenteCMCB.Id;
                    compItem.NomComponente = item.ComponenteCMCB.Componente;
                }
                if (item.GerenciaCMCB != null)
                {
                    compItem.IdGerencia = item.GerenciaCMCB.Id;
                    compItem.NomGerencia = item.GerenciaCMCB.Gerencia;
                };
                if (item.AreaCMCB != null)
                {
                    compItem.IdArea = item.AreaCMCB.Id;
                    compItem.NomArea = item.AreaCMCB.Area;
                };
                compItem.Frecuencia = item.Frecuencia;
    
                if (item.Responsable1 != null)
                {
                    compItem.IdResponsable1 = item.Responsable1.IdUsuario;
                    compItem.NomResponsable1 = item.Responsable1.Nombres + " - " + item.Responsable1.DNI;
                };
                if (item.Responsable2 != null)
                {
                    compItem.IdResponsable2 = item.Responsable2.IdUsuario;
                    compItem.NomResponsable2 = item.Responsable2.Nombres + " - " + item.Responsable2.DNI;
                };
                if (item.Aprueba != null)
                {
                    compItem.Aprueba = item.Aprueba.IdUsuario;
                    compItem.NomAprueba = item.Aprueba.Nombres + " - " + item.Responsable2.DNI;
                };
                compItem.Frecuencia_reporte = item.Frecuencia_reporte;
                compItem.Sustentos = item.Sustentos;
                compItem.Estado = item.Estado;

                output.Registros.Add(compItem);
            }
            //Contar registros
            output.NRegistros = output.Registros.Count;
            //Filtrar registros en funcion a la pagina y registros por pagina
            output.Registros = output.Registros.Skip((pagina - 1) * rpp).Take(rpp).ToList();
            //Retornar registros
            return output;
        }
        public async Task<CompromisoOuput> BuscarPorID(int idCompromiso)
        {
            var compromiso = await _compromisoReadOnlyRepository.BuscarPorID(idCompromiso);
            var ouput = new CompromisoOuput();
            ouput.Id = compromiso.Id;
            ouput.Codigo = compromiso.Codigo;
            ouput.Texto_compromiso = compromiso.Texto_compromiso;

            if (compromiso.OrigenCMCB != null)
            {
                ouput.IdOrigen = compromiso.OrigenCMCB.Id;
                ouput.NomOrigen = compromiso.OrigenCMCB.Origen;
            };
            ouput.Tomo = compromiso.Tomo;
            ouput.Capitulo = compromiso.Capitulo;
            ouput.Seccion = compromiso.Seccion;
            ouput.Pagina = compromiso.Pagina;
            ouput.Resolucion = compromiso.Resolucion;
            if (compromiso.AutoridadCMCB != null)
            {
                ouput.IdAutoridad = compromiso.AutoridadCMCB.Id;
                ouput.NomAutoridad = compromiso.AutoridadCMCB.Autoridad;
            };
            ouput.Fecha = compromiso.Fecha;


            return ouput;
        }
    }
}
