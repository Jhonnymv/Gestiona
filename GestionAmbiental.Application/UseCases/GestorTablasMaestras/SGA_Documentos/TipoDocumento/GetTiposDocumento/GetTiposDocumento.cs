using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.TipoDocumento.GetTiposDocumento
{
    public class GetTiposDocumento : IGetTiposDocumento
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetTiposDocumento(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<TiposDocumentoOutput> Execute(string filtro, int pagina, int rpp)
        {
            var input = await _gestorTablasMaestrasReadOnlyRepository.GetTipoDocumento(filtro);

            var outputs = new List<TipoDocumentoOutput>();
            foreach (var item in input)
            {
                var temp = new TipoDocumentoOutput();
                temp.IdSGATipoDocumento = item.IdSGATipoDocumento;
                temp.Descripcion = item.Descripcion;
                temp.Codigo = item.Codigo;
                temp.Formato = item.Formato;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            var output = new TiposDocumentoOutput
            {
                NTipos = input.Count,
                TiposDocumento = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
