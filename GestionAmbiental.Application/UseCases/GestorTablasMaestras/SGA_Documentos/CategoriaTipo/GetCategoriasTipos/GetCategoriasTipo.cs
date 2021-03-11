using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.CategoriaTipo.GetCategoriasTipos
{
    public class GetCategoriasTipo : IGetCategoriasTipo
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;
        public GetCategoriasTipo(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }
        public async Task<CategoriasTiposOutput> Execute(string filtro, int pagina, int rpp)
        {
            var input = await _gestorTablasMaestrasReadOnlyRepository.GetCategoriaTipo(filtro);

            var outputs = new List<CategoriaTipoOutput>();
            foreach (var item in input)
            {
                var temp = new CategoriaTipoOutput();
                temp.IdSGACategoriaTipo = item.IdSGACategoriaTipo;
                temp.IdSGACategoriaDocumento = item.CategoriaDocumento.Id_SGA_Categoria_Documento;
                temp.SGACategoriaDocumento = item.CategoriaDocumento.Descripcion;
                temp.IdSGATipoDocumento = item.TipoDocumento.IdSGATipoDocumento;
                temp.SGATipoDocumento = item.TipoDocumento.Descripcion;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            var output = new CategoriasTiposOutput
            {
                NCategorias = input.Count,
                CategoriasTipo = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }

        public async Task<CategoriasTiposOutput> Execute()
        {
            var input = await _gestorTablasMaestrasReadOnlyRepository.GetCategoriaTipo(null);

            var outputs = new List<CategoriaTipoOutput>();
            foreach (var item in input)
            {
                var temp = new CategoriaTipoOutput();
                temp.IdSGACategoriaDocumento = item.CategoriaDocumento.Id_SGA_Categoria_Documento;
                temp.SGACategoriaDocumento = item.CategoriaDocumento.Descripcion;
                temp.IdSGATipoDocumento = item.TipoDocumento.IdSGATipoDocumento;
                temp.SGATipoDocumento = item.TipoDocumento.Descripcion;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            var output = new CategoriasTiposOutput
            {
                NCategorias = input.Count,
                CategoriasTipo = outputs
            };

            return output;
        }
    }
}
