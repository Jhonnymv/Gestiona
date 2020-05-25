using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Incidentes.ReportePreliminar
{
    public class RegistrarReportePreliminarRepository : IRegistrarReportePreliminarReadOnlyRepository
    {
        private Context _context;

        public RegistrarReportePreliminarRepository(Context context)
        {
            _context = context;
        }
        public async Task<int> RegistrarReportePreliminarParte1(Reporte_Preliminar_Incidente reporte)
        {
            var entity = new Entities.Incidentes.ReportePreliminar.Reporte_Preliminar_Incidente();
            entity.Codigo = reporte.Codigo;
            entity.Num_Trabajadores = reporte.Num_Trabajadores;
            entity.Razon_Social_Contratista = reporte.Razon_Social_Contratista;
            entity.RUC_Contratista = reporte.RUC_Contratista;
            entity.Actividad_Contratista = reporte.Actividad_Contratista;
            entity.Domicilio_Contratista = reporte.Domicilio_Contratista;
            entity.Num_Trabajadores_Contratista = reporte.Num_Trabajadores_Contratista;
            entity.Id_Tipo_Incidente = reporte.Tipo_Incidente.Id_Tipo_Incidente;
            entity.Id_Nivel_Incidente = reporte.Nivel_Incidente.Id_Nivel_Incidente;
            entity.Fecha_Incidente = reporte.Fecha_Incidente;
            entity.Producto_Derramado = reporte.Producto_Derramado;
            entity.Cantidad_Derramado = reporte.Cantidad_Derramado;
            entity.Id_Unidad_Cantidad_Derramado = reporte.Unidad_Cantidad_Derramado.Id_Unidad_Medida;
            entity.Area_Afectada = reporte.Area_Afectada;
            entity.Id_Unidad_Area_Afectada = reporte.Unidad_Area_Afectada.Id_Unidad_Medida;
            entity.Lugar_Incidente = reporte.Lugar_Incidente;
            entity.Equipo_Involucrado = reporte.Equipo_Involucrado;
            entity.Id_Componente_Ambiental = reporte.Componente_Ambiental.Id_Componente_Ambiental;
            entity.Descripcion_Incidente = reporte.Descripcion_Incidente;            
            entity.Otra_Causa_Incidente = reporte.Otra_Causa_Incidente;

            entity.Estado = EstadosIncidentes.Pendiente.ToString();

            _context.Reporte_Preliminar_Incidente.Add(entity);            
            await _context.SaveChangesAsync();            

            return entity.Id_Reporte_Preliminar;
        }
        public async Task<int> RegistrarInvolucradoReportePreliminar(Involucrado_Incidente_Reporte involucrado)
        {
            var entity = new Entities.Incidentes.ReportePreliminar.Involucrado_Incidente_Reporte();
            entity.Id_Reporte_Preliminar = involucrado.Id_Reporte_Preliminar;
            entity.Id_Usuario = involucrado.Id_Usuario;
            entity.Nombres = involucrado.Nombres;
            entity.Apellidos = involucrado.Apellidos;
            entity.Email = involucrado.Email;
            entity.Puesto_Trabajo = involucrado.Puesto_Trabajo;
            entity.Celular = involucrado.Celular;
            entity.Turno = involucrado.Turno;
            entity.Tipo_Involucrado = involucrado.Tipo_Involucrado;
            entity.Estado = EstadosIncidentes.Registrado.ToString();

            _context.Involucrado_Incidente_Reporte.Add(entity);
            
            await _context.SaveChangesAsync();

            return entity.Id_Involucrado_Incidente_Reporte;
        }
        public async Task<int> RegistrarAccionCorrectivaReportePreliminar(Accion_Incidente_Reporte accion)
        {
            var entity = new Entities.Incidentes.ReportePreliminar.Accion_Incidente_Reporte();
            entity.Id_Reporte_Preliminar = accion.Id_Reporte_Preliminar;
            entity.Accion_Inmediata = accion.Accion_Inmediata;
            entity.Id_Usuario = accion.Id_Usuario;
            entity.Responsable = accion.Responsable;
            entity.Fecha = accion.Fecha;
            entity.Estatus = accion.Estatus;
            entity.Estado = EstadosIncidentes.Registrado.ToString();

            _context.Accion_Incidente_Reporte.Add(entity);

            await _context.SaveChangesAsync();            

            return entity.Id_Accion_Incidente_Reporte;
        }
        public async Task<int> RegistrarReportePreliminarParte2(Reporte_Preliminar_Incidente reporte)
        {
            var entity = await _context.Reporte_Preliminar_Incidente.FirstOrDefaultAsync(x => x.Id_Reporte_Preliminar.Equals(reporte.Id_Reporte_Preliminar));
            entity.Id_Gerencia_Responsable = reporte.Gerencia_Responsable.IdGerencia;
            entity.Otra_Causa_Incidente = reporte.Otra_Causa_Incidente;
            entity.Evidencia = reporte.Evidencia;
            entity.Comentario_Accion = reporte.Comentario_Accion;
            entity.Id_Supervisor_MA = reporte.Supervisor_MA.IdUsuario;
            entity.Estado = reporte.Estado;

            _context.Reporte_Preliminar_Incidente.Update(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Reporte_Preliminar;
        }
        public async Task<string> GetUltimoCodigo()
        {
            var codigo = await _context.Reporte_Preliminar_Incidente
                .Where(x => x.Estado.Equals(EstadosIncidentes.Difundido.ToString())|| x.Estado.Equals(EstadosIncidentes.PendienteInvestigacion.ToString()))
                .OrderBy(x => x.Codigo).Select(x => x.Codigo)
                .LastOrDefaultAsync();
            if (codigo != null)
                return codigo;
            else
                return "IA-00000000";
        }
        public async Task<int> RegistrarCausaIncidenteReporte(Causa_Incidente_Reporte causa)
        {
            var entity = new Entities.Incidentes.ReportePreliminar.Causa_Incidente_Reporte();
            entity.Id_Reporte_Preliminar = causa.Id_Reporte_Preliminar;
            entity.Id_Acto_Condicion = causa.Id_Acto_Condicion;
            entity.Tipo_Acto_Condicion = causa.Tipo_Acto_Condicion;
            entity.Estado = EstadosIncidentes.Registrado.ToString();

            _context.Causa_Incidente_Reporte.Add(entity);
            
            await _context.SaveChangesAsync();

            return entity.Id_Causa_Incidente_Reporte;
        }
    }
}
