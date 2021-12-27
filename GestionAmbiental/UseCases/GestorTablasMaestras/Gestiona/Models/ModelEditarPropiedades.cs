using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Gestiona.Models
{
    public class ModelEditarPropiedades
    {
        public string Json { get; set; }
        public IFormFile InspectionFile { get; set; }
        public IFormFile RacFile { get; set; }
        public IFormFile ResiduosFile { get; set; }
        public IFormFile IncidentesFile { get; set; }
        public IFormFile FaunaFile { get; set; }
        public IFormFile AguaFile { get; set; }
        public IFormFile SGAFile { get; set; }
        public IFormFile TablasFile { get; set; }
        public IFormFile DashboardFile { get; set; }
        public IFormFile CompromisosFile { get; set; }
    }
}
