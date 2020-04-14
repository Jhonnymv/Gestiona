using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Incidentes.ReportePreliminar
{
    public class GetItemsReportePreliminarRepository : IGetItemsReportePreliminarReadOnlyRepository
    {
        private Context _context;

        public GetItemsReportePreliminarRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Involucrado_Incidente>> GetInvolucradosReporte(int IdReportePreliminar)
        {
            var involucrados = await _context.Involucrado_Incidente.Where(x => x.Id_Reporte_Preliminar.Equals(IdReportePreliminar) && x.Estado.Equals(EstadosIncidentes.Registrado.ToString())).ToListAsync();
            var output = new List<Involucrado_Incidente>();
            foreach(var item in involucrados)
            {
                var outputItem = new Involucrado_Incidente();
                outputItem.Id_Involucrado_Incidente = item.Id_Involucrado_Incidente;
                outputItem.Id_Reporte_Preliminar = item.Id_Reporte_Preliminar;
                outputItem.Tipo_Involucrado = item.Tipo_Involucrado;
                outputItem.Id_Usuario = item.Id_Usuario;
                outputItem.Nombres = item.Nombres;
                outputItem.Apellidos = item.Apellidos;
                outputItem.Email = item.Email;
                outputItem.Puesto_Trabajo = item.Puesto_Trabajo;
                outputItem.Celular = item.Celular;
                outputItem.Turno = item.Turno;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }
            return output;
        }
        public async Task<List<Accion_Correctiva_Incidente>> GetAccionesReporte(int IdReportePreliminar)
        {
            var involucrados = await _context.Accion_Correctiva_Incidente.Where(x => x.Id_Reporte_Preliminar.Equals(IdReportePreliminar) && x.Estado.Equals(EstadosIncidentes.Registrado.ToString())).ToListAsync();
            var output = new List<Accion_Correctiva_Incidente>();
            foreach (var item in involucrados)
            {
                var outputItem = new Accion_Correctiva_Incidente();

                outputItem.Id_Accion_Correctiva_Incidente = item.Id_Accion_Correctiva_Incidente;
                outputItem.Id_Reporte_Preliminar = item.Id_Reporte_Preliminar;
                outputItem.Accion_Inmediata = item.Accion_Inmediata;
                outputItem.Id_Usuario = item.Id_Usuario;
                outputItem.Responsable = item.Responsable;
                outputItem.Fecha = item.Fecha;
                outputItem.Estatus = item.Estatus;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }
            return output;
        }
    }
}
