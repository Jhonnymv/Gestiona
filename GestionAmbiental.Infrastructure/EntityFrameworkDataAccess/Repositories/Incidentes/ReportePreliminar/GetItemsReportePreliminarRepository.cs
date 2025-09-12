using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
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

        public async Task<List<Involucrado_Incidente_Reporte>> GetInvolucradosReporte(int IdReportePreliminar)
        {
            var involucrados = await _context.Involucrado_Incidente_Reporte.Where(x => x.Id_Reporte_Preliminar.Equals(IdReportePreliminar)).ToListAsync();
            var output = new List<Involucrado_Incidente_Reporte>();
            foreach(var item in involucrados)
            {
                var outputItem = new Involucrado_Incidente_Reporte();
                outputItem.Id_Involucrado_Incidente_Reporte = item.Id_Involucrado_Incidente_Reporte;
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
        public async Task<List<Accion_Incidente_Reporte>> GetAccionesReporte(int IdReportePreliminar)
        {
            var involucrados = await _context.Accion_Incidente_Reporte.Where(x => x.Id_Reporte_Preliminar.Equals(IdReportePreliminar)).ToListAsync();
            var output = new List<Accion_Incidente_Reporte>();
            foreach (var item in involucrados)
            {
                var outputItem = new Accion_Incidente_Reporte();

                outputItem.Id_Accion_Incidente_Reporte = item.Id_Accion_Incidente_Reporte;
                outputItem.Id_Reporte_Preliminar = item.Id_Reporte_Preliminar;
                outputItem.Tipo_Accion = item.Tipo_Accion;
                outputItem.Accion_Inmediata = item.Accion_Inmediata;
                outputItem.Id_Usuario = item.Id_Usuario;
                outputItem.Id_Usuario2 = item.Id_Usuario2;
                outputItem.Responsable = item.Responsable;
                outputItem.Fecha = item.Fecha;
                outputItem.Estatus = item.Estatus;
                outputItem.Fecha_Verificacion = item.Fecha_Verificacion;
                outputItem.Comentarios_Verificacion = item.Comentarios_Verificacion;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }
            return output;
        }
        public async Task<List<Causa_Incidente_Reporte>> GetCausasReporte(int IdReportePreliminar)
        {
            var causas = await _context.Causa_Incidente_Reporte.Where(x => x.Id_Reporte_Preliminar.Equals(IdReportePreliminar)).ToListAsync();
            var output = new List<Causa_Incidente_Reporte>();
            foreach (var item in causas)
            {
                var outputItem = new Causa_Incidente_Reporte();

                outputItem.Id_Causa_Incidente_Reporte = item.Id_Causa_Incidente_Reporte;
                outputItem.Id_Reporte_Preliminar = item.Id_Reporte_Preliminar;
                outputItem.Id_Acto_Condicion = item.Id_Acto_Condicion;
                outputItem.Tipo_Acto_Condicion = item.Tipo_Acto_Condicion;
                outputItem.Id_Factor_Causa_Incidente = item.Id_Factor_Causa_Incidente;
                outputItem.Descripcion = item.Descripcion;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }
            return output;
        }
        public async Task<Accion_Incidente_Reporte> GetAccionReporte(int IdAccionCorrectiva)
        {
            var entity = await _context.Accion_Incidente_Reporte.FirstOrDefaultAsync(x => x.Id_Accion_Incidente_Reporte.Equals(IdAccionCorrectiva));
            var output = new Accion_Incidente_Reporte();
            output.Id_Accion_Incidente_Reporte = entity.Id_Accion_Incidente_Reporte;
            output.Id_Reporte_Preliminar = entity.Id_Reporte_Preliminar;
            output.Accion_Inmediata = entity.Accion_Inmediata;
            output.Tipo_Accion = entity.Tipo_Accion;
            output.Id_Usuario = entity.Id_Usuario;
            output.Id_Usuario2 = entity.Id_Usuario2;
            output.Responsable = entity.Responsable;
            output.Fecha = entity.Fecha;
            output.Estatus = entity.Estatus;
            output.Evidencia = entity.Evidencia;
            output.Fecha_Implementacion = entity.Fecha_Implementacion;
            output.Comentarios_Implementacion = entity.Comentarios_Implementacion;
            output.Fecha_Verificacion = entity.Fecha_Verificacion;
            output.Comentarios_Verificacion = entity.Comentarios_Verificacion;
            output.Estado = entity.Estado;
            return output;
        }
        public async Task<Causa_Incidente_Reporte> GetCausaReporte(int IdCausaIncidente)
        {
            var entity = await _context.Causa_Incidente_Reporte.FirstOrDefaultAsync(x => x.Id_Causa_Incidente_Reporte.Equals(IdCausaIncidente));
            var output = new Causa_Incidente_Reporte();
            output.Id_Causa_Incidente_Reporte = entity.Id_Causa_Incidente_Reporte;
            output.Id_Reporte_Preliminar = entity.Id_Reporte_Preliminar;
            output.Id_Acto_Condicion = entity.Id_Acto_Condicion;
            output.Tipo_Acto_Condicion = entity.Tipo_Acto_Condicion;
            output.Descripcion = entity.Descripcion;
            output.Id_Factor_Causa_Incidente = entity.Id_Factor_Causa_Incidente;
            output.Estado = entity.Estado;
            return output;
        }
        public async Task<Involucrado_Incidente_Reporte> GetInvolucradoReporte(int IdInvolucradoIncidente)
        {
            var entity = await _context.Involucrado_Incidente_Reporte.FirstOrDefaultAsync(x => x.Id_Involucrado_Incidente_Reporte.Equals(IdInvolucradoIncidente));
            var output = new Involucrado_Incidente_Reporte();
            output.Id_Involucrado_Incidente_Reporte = entity.Id_Involucrado_Incidente_Reporte;
            output.Id_Reporte_Preliminar = entity.Id_Reporte_Preliminar;
            output.Id_Usuario = entity.Id_Usuario;
            output.Nombres = entity.Nombres;
            output.Apellidos = entity.Apellidos;
            output.Email = entity.Email;
            output.Puesto_Trabajo = entity.Puesto_Trabajo;
            output.Celular = entity.Celular;
            output.Turno = entity.Turno;
            output.Tipo_Involucrado = entity.Tipo_Involucrado;
            output.Estado = entity.Estado;

            return output;
        }
    }
}
