using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Reportes.Inspecciones
{
    public class ReportesModel
    {
        public string titulo { get; set; }
        public List<Dimension> data { get; set; }

        public ReportesModel(string titulo, List<Dimension> data)
        {
            this.titulo = titulo;
            this.data = data;
        }
    }
}
