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

        public async Task<CompromisosOuput> Execute(int pagina, int rpp)
        {
            //Consultar registros de BD
            var registros = await _compromisoReadOnlyRepository.ListarCompromisos();

            //Nueva lista en blanco
            var output = new CompromisosOuput();
            output.Registros = new List<CompromisoOuput>();

            //Mapear registros en nueva lista
            foreach(var item in registros)
            {
                

                output.Registros.Add(new CompromisoOuput
                {
                    Id = item.Id,
                    Codigo = item.Codigo,
                    IdCriterio= item.CriterioCMCB.Id,
                    NomCriterio = item.CriterioCMCB.Criterio,
                    Detalle_compromiso=item.Detalle_compromiso,
                    IdEtapa=item.EtapaCMCB.Id,
                    NomEtapa=item.EtapaCMCB.Etapa,
                    //IdAspecto=item.AspectoCMCB.Id,
                    //NomAspecto=item.AspectoCMCB.Aspecto,
                    //IdComponente=item.ComponenteCMCB.Id,
                    //NomComponente=item.ComponenteCMCB.Componente,
                    //IdGerencia=item.GerenciaCMCB.Id,
                    //NomGerencia=item.GerenciaCMCB.Gerencia,
                    //IdArea=item.AreaCMCB.Id,
                    //NomArea=item.AreaCMCB.Area
                    //Texto_compromiso= item.Texto_compromiso,
                }) ;
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
            var ouput=new CompromisoOuput();
            ouput.Id= compromiso.Id;
            ouput.Codigo= compromiso.Codigo;
            //ouput.IdCriterio=compromiso.CriterioCMCB.Id;
            ouput.Detalle_compromiso=compromiso.Detalle_compromiso;
            //ouput.IdEtapa=compromiso.EtapaCMCB.Id;
            ouput.Texto_compromiso = compromiso.Texto_compromiso;
            //ouput.IdOrigen = compromiso.OrigenCMCB.Id;
            return ouput;
        }
    }
}
