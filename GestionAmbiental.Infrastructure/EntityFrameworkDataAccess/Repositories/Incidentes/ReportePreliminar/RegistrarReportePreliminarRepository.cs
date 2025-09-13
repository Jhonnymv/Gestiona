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
        private readonly Context _context;

        public RegistrarReportePreliminarRepository(Context context)
        {
            _context = context;
		}
		public async Task<int> RegistrarReportePreliminarDesdeFlashReport(Reporte_Preliminar_Incidente reporte, int Id_Flash_Report)
		{
            var producto = await _context.Producto_Incidente.FirstOrDefaultAsync(x => x.Id_Producto_Incidente.Equals(Convert.ToInt32(reporte.Producto_Derramado)));
            var entity = new Entities.Incidentes.ReportePreliminar.Reporte_Preliminar_Incidente
			{
				Codigo = reporte.Codigo,
				//Num_Trabajadores = reporte.Num_Trabajadores,
				//Razon_Social_Contratista = reporte.Razon_Social_Contratista,
				//RUC_Contratista = reporte.RUC_Contratista,
				//Actividad_Contratista = reporte.Actividad_Contratista,
				//Domicilio_Contratista = reporte.Domicilio_Contratista,
				//Num_Trabajadores_Contratista = reporte.Num_Trabajadores_Contratista,
				//Id_Tipo_Incidente = reporte.Tipo_Incidente.Id_Tipo_Incidente,
				Id_Nivel_Incidente = reporte.Nivel_Incidente.Id_Nivel_Incidente,
                Id_Gerencia_Responsable = reporte.Gerencia_Responsable.IdGerencia,
				Fecha_Incidente = reporte.Fecha_Incidente,
				Producto_Derramado = producto.Descripcion,
                Cantidad_Derramado = reporte.Cantidad_Derramado,
				Id_Unidad_Cantidad_Derramado = reporte.Unidad_Cantidad_Derramado.Id_Unidad_Medida,
				//Area_Afectada = reporte.Area_Afectada,
				//Id_Unidad_Area_Afectada = reporte.Unidad_Area_Afectada.Id_Unidad_Medida,
				Lugar_Incidente = reporte.Lugar_Incidente,
				//Equipo_Involucrado = reporte.Equipo_Involucrado,
				Id_Componente_Ambiental = reporte.Componente_Ambiental.Id_Componente_Ambiental,
                Descripcion_Incidente = reporte.Descripcion_Incidente,
                Id_Supervisor_MA = reporte.Supervisor_MA.IdUsuario,
				//Otra_Causa_Incidente = reporte.Otra_Causa_Incidente,
				Estado = EstadosIncidentes.Pendiente.ToString(),
                Id_Registrado_Por = reporte.Id_Registrado_Por
            };

            _context.Reporte_Preliminar_Incidente.Add(entity);
            await _context.SaveChangesAsync();

            var acciones = await _context.Accion_Flash_Report.Where(x => x.Id_Flash_Report.Equals(Id_Flash_Report) && x.Estado.Equals(EstadosIncidentes.Registrado.ToString())).ToListAsync();
            var accionesEntities = new List<Entities.Incidentes.ReportePreliminar.Accion_Incidente_Reporte>();

            foreach (var accion in acciones)
            {
                accionesEntities.Add(new Entities.Incidentes.ReportePreliminar.Accion_Incidente_Reporte
                {
                    Id_Reporte_Preliminar = entity.Id_Reporte_Preliminar,
                    Accion_Inmediata = accion.Descripcion,
                    Id_Usuario = accion.Id_Usuario,
                    Responsable = accion.Responsable,
                    Fecha = accion.Fecha,
                    Estatus = accion.Estatus,
                    Estado = EstadosIncidentes.Registrado.ToString(),
                    Tipo_Accion = "Acción Inmediata"
                });
            }
            _context.Accion_Incidente_Reporte.AddRange(accionesEntities);
            await _context.SaveChangesAsync();

            return entity.Id_Reporte_Preliminar;
		}
		public async Task<int> RegistrarReportePreliminarParte1(Reporte_Preliminar_Incidente reporte)
        {
            var entity = new Entities.Incidentes.ReportePreliminar.Reporte_Preliminar_Incidente
            {
                Codigo = reporte.Codigo,
                Num_Trabajadores = reporte.Num_Trabajadores,
                Razon_Social_Contratista = reporte.Razon_Social_Contratista,
                RUC_Contratista = reporte.RUC_Contratista,
                Actividad_Contratista = reporte.Actividad_Contratista,
                Domicilio_Contratista = reporte.Domicilio_Contratista,
                Num_Trabajadores_Contratista = reporte.Num_Trabajadores_Contratista,
                Id_Tipo_Incidente = reporte.Tipo_Incidente.Id_Tipo_Incidente,
                Id_Nivel_Incidente = reporte.Nivel_Incidente.Id_Nivel_Incidente,
                Fecha_Incidente = reporte.Fecha_Incidente,
                Producto_Derramado = reporte.Producto_Derramado,
                Cantidad_Derramado = reporte.Cantidad_Derramado,
                Id_Unidad_Cantidad_Derramado = reporte.Unidad_Cantidad_Derramado.Id_Unidad_Medida,
                Area_Afectada = reporte.Area_Afectada,
                Id_Unidad_Area_Afectada = reporte.Unidad_Area_Afectada.Id_Unidad_Medida,
                Lugar_Incidente = reporte.Lugar_Incidente,
                Equipo_Involucrado = reporte.Equipo_Involucrado,
                Id_Componente_Ambiental = reporte.Componente_Ambiental.Id_Componente_Ambiental,
                Descripcion_Incidente = reporte.Descripcion_Incidente,
                Otra_Causa_Incidente = reporte.Otra_Causa_Incidente,
                Estado = EstadosIncidentes.Pendiente.ToString()
            };

            _context.Reporte_Preliminar_Incidente.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id_Reporte_Preliminar;
        }
        public async Task<int> RegistrarInvolucradoReportePreliminar(Involucrado_Incidente_Reporte involucrado)
        {
            var entity = new Entities.Incidentes.ReportePreliminar.Involucrado_Incidente_Reporte
            {
                Id_Reporte_Preliminar = involucrado.Id_Reporte_Preliminar,
                Id_Usuario = involucrado.Id_Usuario,
                Nombres = involucrado.Nombres,
                Apellidos = involucrado.Apellidos,
                Email = involucrado.Email,
                Puesto_Trabajo = involucrado.Puesto_Trabajo,
                Celular = involucrado.Celular,
                Turno = involucrado.Turno,
                Tipo_Involucrado = involucrado.Tipo_Involucrado,
                Estado = EstadosIncidentes.Registrado.ToString()
            };

            _context.Involucrado_Incidente_Reporte.Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id_Involucrado_Incidente_Reporte;
        }
        public async Task<int> RegistrarAccionCorrectivaReportePreliminar(Accion_Incidente_Reporte accion)
        {
            var entity = new Entities.Incidentes.ReportePreliminar.Accion_Incidente_Reporte();
            entity.Id_Reporte_Preliminar = accion.Id_Reporte_Preliminar;
            entity.Tipo_Accion = accion.Tipo_Accion;
            entity.Accion_Inmediata = accion.Accion_Inmediata;
            entity.Id_Usuario = accion.Id_Usuario;
            entity.Id_Usuario2 = accion.Id_Usuario2;
            entity.Responsable = accion.Responsable;
            entity.Fecha = accion.Fecha;
            entity.Comentarios_Verificacion = accion.Comentarios_Verificacion;
            entity.Fecha_Verificacion = accion.Fecha_Verificacion;
            entity.Estatus = accion.Estatus;
            entity.Estado = EstadosIncidentes.Registrado.ToString();

            _context.Accion_Incidente_Reporte.Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id_Accion_Incidente_Reporte;
        }
        public async Task<int> RegistrarReportePreliminarParte2(Reporte_Preliminar_Incidente reporte)
        {
            var entity = await _context.Reporte_Preliminar_Incidente.FirstOrDefaultAsync(x => x.Id_Reporte_Preliminar.Equals(reporte.Id_Reporte_Preliminar));
            entity.Id_Gerencia_Responsable = reporte.Gerencia_Responsable?.IdGerencia;
            entity.Otra_Causa_Incidente = reporte.Otra_Causa_Incidente;
            entity.Evidencia = reporte.Evidencia;
            entity.Nombres_Archivos = reporte.NombresArchivos;
            entity.Comentario_Accion = reporte.Comentario_Accion;
            entity.Id_Supervisor_MA = reporte.Supervisor_MA?.IdUsuario;
            entity.Estado = reporte.Estado;

            _context.Reporte_Preliminar_Incidente.Update(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Reporte_Preliminar;
        }
        public async Task<string> GetUltimoCodigo()
        {
			var codigo = await _context.Reporte_Preliminar_Incidente
				.Where(x => !x.Codigo.Equals("TBD") && (x.Estado.Equals(EstadosIncidentes.Difundido.ToString()) || x.Estado.Equals(EstadosIncidentes.PendienteInvestigacion.ToString())))
				.OrderBy(x => x.Codigo).Select(x => x.Codigo)
                .LastOrDefaultAsync();
            if (codigo != null)
                return codigo;
            else
                return "IA-00000000";
        }
        public async Task<int> RegistrarCausaIncidenteReporte(Causa_Incidente_Reporte causa)
        {
            var entity = new Entities.Incidentes.ReportePreliminar.Causa_Incidente_Reporte
            {
                Id_Reporte_Preliminar = causa.Id_Reporte_Preliminar,
                Id_Acto_Condicion = causa.Id_Acto_Condicion,
                Tipo_Acto_Condicion = causa.Tipo_Acto_Condicion,
                Id_Factor_Causa_Incidente = causa.Id_Factor_Causa_Incidente,
                Descripcion = causa.Descripcion,
                Estado = EstadosIncidentes.Registrado.ToString()
            };

            _context.Causa_Incidente_Reporte.Add(entity);
            
            await _context.SaveChangesAsync();

            return entity.Id_Causa_Incidente_Reporte;
        }
    }
}
