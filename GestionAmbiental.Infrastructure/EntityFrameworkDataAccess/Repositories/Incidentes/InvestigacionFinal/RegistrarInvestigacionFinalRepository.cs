using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Incidentes.InvestigacionFinal
{
    public class RegistrarInvestigacionFinalRepository : IRegistrarInvestigacionFinalReadOnlyRepository
    {
        private readonly Context _context;

        public RegistrarInvestigacionFinalRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<int> RegistrarDesdeFlashReport(Investigacion_Final_Incidente investigacion, int Id_Flash_Report)
        {
            var entity = new Entities.Incidentes.InvestigacionFinal.Investigacion_Final_Incidente
            {
                //entity.Id_Reporte_Preliminar = reporte.Id_Reporte_Preliminar;
                Codigo = investigacion.Codigo,
                //contratista
                //entity.Num_Trabajadores = reporte.Num_Trabajadores;
                //entity.Razon_Social_Contratista = reporte.Razon_Social_Contratista;
                //entity.RUC_Contratista = reporte.RUC_Contratista;
                //entity.Actividad_Contratista = reporte.Actividad_Contratista;
                //entity.Domicilio_Contratista = reporte.Domicilio_Contratista;
                //entity.Num_Trabajadores_Contratista = reporte.Num_Trabajadores_Contratista;
                //incidente
                //entity.Id_Tipo_Incidente = reporte.Id_Tipo_Incidente;
                Id_Nivel_Incidente = investigacion.Nivel_Incidente.Id_Nivel_Incidente,
                Lugar_Incidente = investigacion.Lugar_Incidente,
                Fecha_Incidente = investigacion.Fecha_Incidente,
                Descripcion_Incidente = investigacion.Descripcion_Incidente,
                Id_Gerencia_Responsable = investigacion.Gerencia_Responsable.IdGerencia,
                Id_Supervisor_MA = investigacion.Supervisor_MA.IdUsuario,
                Estado = EstadosIncidentes.Pendiente.ToString(),
                Id_Registrado_Por = investigacion.Id_Registrado_Por
            };
            _context.Investigacion_Final_Incidente.Add(entity);
            await _context.SaveChangesAsync();

            var acciones = await _context.Accion_Flash_Report.Where(x => x.Id_Flash_Report.Equals(Id_Flash_Report) && x.Estado.Equals(EstadosIncidentes.Registrado.ToString())).ToListAsync();

            var accionesEntities = new List<Entities.Incidentes.InvestigacionFinal.Accion_Incidente_Investigacion>();

            foreach ( var accion in acciones)
            {
                accionesEntities.Add(new Entities.Incidentes.InvestigacionFinal.Accion_Incidente_Investigacion
                {
                    Id_Investigacion_Final = entity.Id_Investigacion_Final,
                    Tipo_Accion = "Acción Inmediata",
                    Accion_Inmediata = accion.Descripcion,
                    Id_Usuario = accion.Id_Usuario,
                    Responsable = accion.Responsable,
                    Fecha = accion.Fecha,
                    Estatus = accion.Estatus,
                    Estado = EstadosIncidentes.Registrado.ToString()
                });
            }
            _context.Accion_Incidente_Investigacion.AddRange(accionesEntities);
            await _context.SaveChangesAsync();

            return entity.Id_Investigacion_Final;
        }
        public async Task<int> RegistrarDeReportePreliminar(int IdReporte)
        {
            var reporte = await _context.Reporte_Preliminar_Incidente.FirstOrDefaultAsync(x => x.Id_Reporte_Preliminar.Equals(IdReporte));

            var entity = new Entities.Incidentes.InvestigacionFinal.Investigacion_Final_Incidente();
            entity.Id_Reporte_Preliminar = reporte.Id_Reporte_Preliminar;
            entity.Codigo = reporte.Codigo;
            //contratista
            entity.Num_Trabajadores = reporte.Num_Trabajadores;
            entity.Razon_Social_Contratista = reporte.Razon_Social_Contratista;
            entity.RUC_Contratista = reporte.RUC_Contratista;
            entity.Actividad_Contratista = reporte.Actividad_Contratista;
            entity.Domicilio_Contratista = reporte.Domicilio_Contratista;
            entity.Num_Trabajadores_Contratista = reporte.Num_Trabajadores_Contratista;
            //incidente
            entity.Id_Tipo_Incidente = reporte.Id_Tipo_Incidente;
            entity.Id_Nivel_Incidente = reporte.Id_Nivel_Incidente;
            entity.Lugar_Incidente = reporte.Lugar_Incidente;
            entity.Fecha_Incidente = reporte.Fecha_Incidente;
            entity.Descripcion_Incidente = reporte.Descripcion_Incidente;
            entity.Id_Gerencia_Responsable = reporte.Id_Gerencia_Responsable;
            entity.Id_Supervisor_MA = reporte.Id_Supervisor_MA;
            entity.Estado = EstadosIncidentes.PendienteInvestigacion.ToString();
            entity.Id_Registrado_Por = reporte.Id_Registrado_Por;

            _context.Investigacion_Final_Incidente.Add(entity);

            reporte.Estado = EstadosIncidentes.PendienteInvestigacion.ToString();
            _context.Reporte_Preliminar_Incidente.Update(reporte);

            await _context.SaveChangesAsync();
            var IdInvestigacion = entity.Id_Investigacion_Final;

            //Involucrados
            var involucrados = await _context.Involucrado_Incidente_Reporte.Where(x => x.Id_Reporte_Preliminar.Equals(IdReporte) && x.Estado.Equals(EstadosIncidentes.Registrado.ToString())).ToListAsync();
            var listInvolucrados = new List<Entities.Incidentes.InvestigacionFinal.Involucrado_Incidente_Investigacion>();
            involucrados.ForEach(x =>
            listInvolucrados.Add(new Entities.Incidentes.InvestigacionFinal.Involucrado_Incidente_Investigacion
            {
                Id_Investigacion_Final = IdInvestigacion,
                Id_Usuario = x.Id_Usuario,
                Nombres = x.Nombres,
                Apellidos = x.Apellidos,
                Email = x.Email,
                Celular = x.Celular,
                Puesto_Trabajo = x.Puesto_Trabajo,
                Turno = x.Turno,
                Tipo_Involucrado = x.Tipo_Involucrado,
                Estado = EstadosIncidentes.Registrado.ToString()
            }));
            _context.Involucrado_Incidente_Investigacion.UpdateRange(listInvolucrados);

            //Causas
            var causas = await _context.Causa_Incidente_Reporte.Where(x => x.Id_Reporte_Preliminar.Equals(IdReporte) && x.Estado.Equals(EstadosIncidentes.Registrado.ToString())).ToListAsync();
            var listCausas = new List<Entities.Incidentes.InvestigacionFinal.Causa_Incidente_Investigacion>();
            causas.ForEach(x =>
            listCausas.Add(new Entities.Incidentes.InvestigacionFinal.Causa_Incidente_Investigacion
            {
                Id_Investigacion_Final = IdInvestigacion,
                Id_Acto_Condicion = x.Id_Acto_Condicion,
                Tipo_Acto_Condicion = x.Tipo_Acto_Condicion,
                Estado = EstadosIncidentes.Registrado.ToString()
            }));
            _context.Causa_Incidente_Investigacion.UpdateRange(listCausas);

            //Acciones
            var acciones = await _context.Accion_Incidente_Reporte.Where(x => x.Id_Reporte_Preliminar.Equals(IdReporte) && x.Estado.Equals(EstadosIncidentes.Registrado.ToString())).ToListAsync();
            var listAcciones = new List<Entities.Incidentes.InvestigacionFinal.Accion_Incidente_Investigacion>();
            acciones.ForEach(x => listAcciones.Add(new Entities.Incidentes.InvestigacionFinal.Accion_Incidente_Investigacion
            {
                Id_Investigacion_Final = IdInvestigacion,
                Tipo_Accion = "Acción Inmediata",
                Accion_Inmediata = x.Accion_Inmediata,
                Id_Usuario = x.Id_Usuario,
                Responsable = x.Responsable,
                Fecha = x.Fecha,
                Estatus = x.Estatus,
                Estado = EstadosIncidentes.Registrado.ToString()
            }));
            _context.Accion_Incidente_Investigacion.UpdateRange(listAcciones);

            await _context.SaveChangesAsync();

            return IdInvestigacion;
        }
        public async Task<int> RegistrarImpactoAmbiental(int IdInvestigacion)
        {
            var entity = new Entities.Incidentes.InvestigacionFinal.Impacto_Ambiental_Incidente();
            entity.Id_Investigacion_Final = IdInvestigacion;

            _context.Impacto_Ambiental_Incidente.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id_Impacto_Ambiental_Incidente;
        }
        public async Task<int> RegistrarProductoDerramado(Producto_Derramado_Incidente producto)
        {
            var entity = new Entities.Incidentes.InvestigacionFinal.Producto_Derramado_Incidente();

            entity.Id_Investigacion_Final = producto.Id_Investigacion_Final;
            entity.Id_Producto_Incidente = producto.Producto_Incidente.Id_Producto_Incidente;
            entity.Id_Tipo_Producto = producto.Tipo_Producto.Id_Tipo_Producto_Incidente;
            entity.Estado_Producto = producto.Estado_Producto;
            entity.Cantidad_Derramada = producto.Cantidad_Derramada;
            entity.Id_Unidad = producto.Unidad.Id_Unidad_Medida;
            entity.Equipo_Responsable = producto.Equipo_Responsable;
            entity.Id_Estado_Equipo_Responsable = producto.Estado_Equipo_Responsable.Id_Estado_Equipo_Incidente;
            entity.Cantidad_Recuperado = producto.Cantidad_Recuperado;
            entity.Cantidad_No_Recuperado = producto.Cantidad_No_Recuperado;
            entity.Estado = producto.Estado;

            _context.Producto_Derramado_Incidente.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id_Producto_Derramado_Incidente;
        }
        public async Task<int> RegistrarInvolucradoInvestigacion(Involucrado_Incidente_Investigacion involucrado)
        {
            var entity = new Entities.Incidentes.InvestigacionFinal.Involucrado_Incidente_Investigacion();
            entity.Id_Investigacion_Final = involucrado.Id_Investigacion_Final;
            entity.Id_Usuario = involucrado.Id_Usuario;
            entity.Nombres = involucrado.Nombres;
            entity.Apellidos = involucrado.Apellidos;
            entity.Email = involucrado.Email;
            entity.Doc_Identidad = involucrado.Doc_Identidad;
            entity.Edad = involucrado.Edad;
            entity.Puesto_Trabajo = involucrado.Puesto_Trabajo;
            entity.Empresa = involucrado.Empresa;
            entity.Celular = involucrado.Celular;
            entity.Turno = involucrado.Turno;
            entity.Tipo_Involucrado = involucrado.Tipo_Involucrado;
            entity.Estado = EstadosIncidentes.Registrado.ToString();

            _context.Involucrado_Incidente_Investigacion.Add(entity);

            await _context.SaveChangesAsync();
            return entity.Id_Involucrado_Incidente_Investigacion;
        }
        public async Task<int> RegistrarAccionCorrectivaInvestigacion(Accion_Incidente_Investigacion accion)
        {
            var entity = new Entities.Incidentes.InvestigacionFinal.Accion_Incidente_Investigacion();
            entity.Id_Investigacion_Final = accion.Id_Investigacion_Final;
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

            _context.Accion_Incidente_Investigacion.Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id_Accion_Incidente_Investigacion;
        }
        public async Task<int> RegistrarCausaIncidenteInvestigacion(Causa_Incidente_Investigacion causa)
        {
            var entity = new Entities.Incidentes.InvestigacionFinal.Causa_Incidente_Investigacion();
            entity.Id_Investigacion_Final = causa.Id_Investigacion_Final;
            entity.Id_Acto_Condicion = causa.Id_Acto_Condicion;
            entity.Tipo_Acto_Condicion = causa.Tipo_Acto_Condicion;
            entity.Id_Factor_Causa_Incidente = causa.Id_Factor_Causa_Incidente;
            entity.Descripcion = causa.Descripcion;
            entity.Estado = EstadosIncidentes.Registrado.ToString();

            _context.Causa_Incidente_Investigacion.Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id_Causa_Incidente_Investigacion;
        }
    }
}
