using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.CategoriaTipo
{
    public class CategoriaTipoOutput
    {
        public int IdSGACategoriaTipo { get; set; }
        public int IdSGACategoriaDocumento { get; set; }
        public string SGACategoriaDocumento { get; set; }
        public int IdSGATipoDocumento { get; set; }
        public string SGATipoDocumento { get; set; }
        public string Estado { get; set; }
        public CategoriaTipoOutput() { }
        public CategoriaTipoOutput(Domain.SGA_Documentos.CategoriaTipo categoriaTipo) {
            IdSGACategoriaTipo = categoriaTipo.IdSGACategoriaTipo;
            IdSGACategoriaDocumento = categoriaTipo.CategoriaDocumento.Id_SGA_Categoria_Documento;
            SGACategoriaDocumento = categoriaTipo.CategoriaDocumento.Descripcion;
            IdSGATipoDocumento = categoriaTipo.TipoDocumento.IdSGATipoDocumento;
            SGATipoDocumento = categoriaTipo.TipoDocumento.Descripcion;
            Estado = categoriaTipo.Estado;
        }
    }
}
