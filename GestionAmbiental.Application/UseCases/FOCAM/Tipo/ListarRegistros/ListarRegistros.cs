using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.FOCAM.Tipo;

namespace GestionAmbiental.Application.UseCases.FOCAM.Tipo.ListarRegistros
{
    public class ListarRegistros: IListarRegistros
    {
        private readonly ITipoReadOnlyRepository _tipoReadOnlyRepository;

        public ListarRegistros(ITipoReadOnlyRepository tipoReadOnlyRepository)
        {
            _tipoReadOnlyRepository = tipoReadOnlyRepository;
        }

        public async Task<TipoOuput> BuscarById(int idTipo)
        {
            var tipo = await _tipoReadOnlyRepository.BuscarPorID(idTipo);
            var ouput = new TipoOuput();

            ouput.Id = tipo.Id;
            ouput.Nombre= tipo.Nombre;
            ouput.Descripcion= tipo.Descripcion;
            return ouput;
        }

        public async Task<TiposOuput> Execute(int pagina, int rpp)
        {
            var registros = await _tipoReadOnlyRepository.ListTipos();
            var ouput = new TiposOuput();
            ouput.Registros = new List<TipoOuput>();
            foreach (var item in registros)
            {
                var tipo = new TipoOuput();
                tipo.Id = item.Id;
                tipo.Nombre= item.Nombre;
                tipo.Descripcion= item.Descripcion;
                ouput.Registros.Add(tipo);
            }
            ouput.NRegistros = ouput.Registros.Count;
            ouput.Registros = ouput.Registros.Skip((pagina - 1) * rpp).Take(rpp).ToList();
            return ouput;
        }

        public async Task<TiposOuput> SelectItems()
        {
            var registros = await _tipoReadOnlyRepository.ListTipos();
            var ouput = new TiposOuput();
            ouput.Registros = new List<TipoOuput>();
            foreach (var item in registros)
            {
                var tipo = new TipoOuput();
                tipo.Id = item.Id;
                tipo.Nombre = item.Nombre;
                tipo.Descripcion = item.Descripcion;
                ouput.Registros.Add(tipo);
            }
            ouput.NRegistros = ouput.Registros.Count;
            return ouput;
        }
    }
}