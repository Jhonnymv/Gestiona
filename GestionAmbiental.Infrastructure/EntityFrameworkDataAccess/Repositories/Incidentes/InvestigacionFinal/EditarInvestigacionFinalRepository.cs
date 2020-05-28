using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Incidentes.InvestigacionFinal
{
    public class EditarInvestigacionFinalRepository : IEditarInvestigacionFinalReadOnlyRepository
    {
        private readonly Context _context;
        public EditarInvestigacionFinalRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> EditarProductoDerramado(Producto_Derramado_Incidente producto)
        {
            var entity = await _context.Producto_Derramado_Incidente.FirstOrDefaultAsync(x => x.Id_Producto_Derramado_Incidente.Equals(producto.Id_Producto_Derramado_Incidente));

            entity.Id_Investigacion_Final = producto.Id_Investigacion_Final!=0? producto.Id_Investigacion_Final : entity.Id_Investigacion_Final;
            entity.Id_Producto_Incidente = producto.Producto_Incidente !=null? producto.Producto_Incidente.Id_Producto_Incidente: entity.Id_Producto_Incidente;
            entity.Id_Tipo_Producto = producto.Tipo_Producto != null? producto.Tipo_Producto.Id_Tipo_Producto_Incidente: entity.Id_Tipo_Producto;
            entity.Estado_Producto = producto.Estado_Producto ?? entity.Estado_Producto;
            entity.Cantidad_Derramada = producto.Cantidad_Derramada ?? entity.Cantidad_Derramada;
            entity.Id_Unidad = producto.Unidad != null ? producto.Unidad.Id_Unidad_Medida : entity.Id_Unidad;
            entity.Equipo_Responsable = producto.Equipo_Responsable ?? entity.Equipo_Responsable;
            entity.Id_Estado_Equipo_Responsable = producto.Estado_Equipo_Responsable != null ? producto.Estado_Equipo_Responsable.Id_Estado_Equipo_Incidente : entity.Id_Estado_Equipo_Responsable;
            entity.Cantidad_Recuperado = producto.Cantidad_Recuperado ?? entity.Cantidad_Recuperado;
            entity.Cantidad_No_Recuperado = producto.Cantidad_No_Recuperado ?? entity.Cantidad_No_Recuperado;
            entity.Estado = producto.Estado;

            _context.Producto_Derramado_Incidente.Update(entity);
            
            await _context.SaveChangesAsync();
            return entity.Id_Producto_Derramado_Incidente;
        }

        public async Task<int> EditarAccionCorrectivaInvestigacion(Accion_Incidente_Investigacion accion)
        {
            var entity = await _context.Accion_Incidente_Investigacion.FirstOrDefaultAsync(x => x.Id_Accion_Incidente_Investigacion.Equals(accion.Id_Accion_Incidente_Investigacion));
            entity.Tipo_Accion = accion.Tipo_Accion;
            entity.Accion_Inmediata = accion.Accion_Inmediata;
            entity.Id_Usuario = accion.Id_Usuario;
            entity.Responsable = accion.Responsable;
            entity.Fecha = accion.Fecha != null ? accion.Fecha : entity.Fecha;
            entity.Estatus = accion.Estatus != null ? accion.Estatus : entity.Estatus;
            entity.Fecha_Verificacion = accion.Fecha_Verificacion != null ? accion.Fecha_Verificacion : entity.Fecha_Verificacion;
            entity.Comentarios_Verificacion = accion.Comentarios_Verificacion != null ? accion.Comentarios_Verificacion : entity.Comentarios_Verificacion;
            entity.Estado = accion.Estado;

            _context.Accion_Incidente_Investigacion.Update(entity);

            await _context.SaveChangesAsync();

            return entity.Id_Accion_Incidente_Investigacion;
        }

        public async Task<int> EditarCausaIncidenteInvestigacion(Causa_Incidente_Investigacion causa)
        {
            var entity = await _context.Causa_Incidente_Investigacion.FirstOrDefaultAsync(x => x.Id_Causa_Incidente_Investigacion.Equals(causa.Id_Causa_Incidente_Investigacion));
            entity.Id_Acto_Condicion = causa.Id_Acto_Condicion;
            entity.Tipo_Acto_Condicion = causa.Tipo_Acto_Condicion != null ? causa.Tipo_Acto_Condicion : entity.Tipo_Acto_Condicion;
            entity.Id_Factor_Causa_Incidente = causa.Id_Factor_Causa_Incidente;
            entity.Descripcion = causa.Descripcion != null ? causa.Descripcion : entity.Descripcion;
            entity.Estado = causa.Estado;

            _context.Causa_Incidente_Investigacion.Update(entity);

            await _context.SaveChangesAsync();

            return entity.Id_Causa_Incidente_Investigacion;
        }

        public async Task<int> EditarInvolucradoInvestigacion(Involucrado_Incidente_Investigacion involucrado)
        {
            var entity = await _context.Involucrado_Incidente_Investigacion.FirstOrDefaultAsync(x => x.Id_Involucrado_Incidente_Investigacion.Equals(involucrado.Id_Involucrado_Incidente_Investigacion));
            entity.Id_Usuario = involucrado.Id_Usuario != null ? involucrado.Id_Usuario : entity.Id_Usuario;
            entity.Nombres = involucrado.Nombres != null ? involucrado.Nombres : entity.Nombres;
            entity.Apellidos = involucrado.Apellidos != null ? involucrado.Apellidos : entity.Apellidos;
            entity.Email = involucrado.Email != null ? involucrado.Email : entity.Email;
            entity.Celular = involucrado.Celular != null ? involucrado.Celular : entity.Celular;
            entity.Doc_Identidad = involucrado.Doc_Identidad != null ? involucrado.Doc_Identidad : entity.Doc_Identidad;
            entity.Edad = involucrado.Edad != null ? involucrado.Edad : entity.Edad;
            entity.Puesto_Trabajo = involucrado.Puesto_Trabajo != null ? involucrado.Puesto_Trabajo : entity.Puesto_Trabajo;
            entity.Empresa = involucrado.Empresa != null ? involucrado.Empresa : entity.Empresa;
            entity.Turno = involucrado.Turno != null ? involucrado.Turno : entity.Turno;
            entity.Tipo_Involucrado = involucrado.Tipo_Involucrado != null ? involucrado.Tipo_Involucrado : entity.Tipo_Involucrado;
            entity.Estado = involucrado.Estado;

            _context.Involucrado_Incidente_Investigacion.Update(entity);
            await _context.SaveChangesAsync();

            return entity.Id_Involucrado_Incidente_Investigacion;
        }

        public async Task<int> EditarInvestigacionFinal(Investigacion_Final_Incidente investigacion, Impacto_Ambiental_Incidente impacto)
        {
            var investigacion_entity = await _context.Investigacion_Final_Incidente.FirstOrDefaultAsync(x => x.Id_Investigacion_Final.Equals(investigacion.Id_Investigacion_Final));
            var impacto_entity = await _context.Impacto_Ambiental_Incidente.FirstOrDefaultAsync(x => x.Id_Investigacion_Final.Equals(investigacion.Id_Investigacion_Final));

            investigacion_entity.Num_Trabajadores = investigacion.Num_Trabajadores;
            investigacion_entity.Razon_Social_Contratista = investigacion.Razon_Social_Contratista;
            investigacion_entity.RUC_Contratista = investigacion.RUC_Contratista;
            investigacion_entity.Actividad_Contratista = investigacion.Actividad_Contratista;
            investigacion_entity.Domicilio_Contratista = investigacion.Domicilio_Contratista;
            investigacion_entity.Num_Trabajadores_Contratista = investigacion.Num_Trabajadores_Contratista;
            investigacion_entity.Razon_Social_Subcontratista = investigacion.Razon_Social_Subcontratista;
            investigacion_entity.RUC_Subcontratista = investigacion.RUC_Subcontratista;
            investigacion_entity.Actividad_Subcontratista = investigacion.Actividad_Subcontratista;
            investigacion_entity.Domicilio_Subcontratista = investigacion.Domicilio_Subcontratista;
            investigacion_entity.Num_Trabajadores_Subcontratista = investigacion.Num_Trabajadores_Subcontratista;
            investigacion_entity.Id_Tipo_Incidente = investigacion.Tipo_Incidente!=null? investigacion.Tipo_Incidente.Id_Tipo_Incidente:investigacion_entity.Id_Tipo_Incidente;
            investigacion_entity.Id_Nivel_Incidente = investigacion.Nivel_Incidente != null ? investigacion.Nivel_Incidente.Id_Nivel_Incidente : investigacion_entity.Id_Nivel_Incidente;
            investigacion_entity.Coordenada_Este = investigacion.Coordenada_Este;
            investigacion_entity.Coordenada_Norte = investigacion.Coordenada_Norte;
            investigacion_entity.Departamento = investigacion.Departamento;
            investigacion_entity.Localidad = investigacion.Localidad;
            investigacion_entity.Distrito = investigacion.Distrito;
            investigacion_entity.Provincia = investigacion.Provincia;
            investigacion_entity.Fecha_Incidente = investigacion.Fecha_Incidente;
            investigacion_entity.Fecha_Inicio_Investigacion = investigacion.Fecha_Inicio_Investigacion;
            investigacion_entity.Turno = investigacion.Turno;
            investigacion_entity.Descripcion_Incidente = investigacion.Descripcion_Incidente;
            investigacion_entity.Id_Gerencia_Responsable = investigacion.Gerencia_Responsable!=null? investigacion.Gerencia_Responsable.IdGerencia: investigacion_entity.Id_Gerencia_Responsable;
            investigacion_entity.Id_Supervisor_MA = investigacion.Supervisor_MA!=null? investigacion.Supervisor_MA.IdUsuario: investigacion_entity.Id_Supervisor_MA;
            investigacion_entity.Evidencia_Papel = investigacion.Evidencia_Papel;
            investigacion_entity.Evidencia_Persona = investigacion.Evidencia_Persona;
            investigacion_entity.Evidencia_Posicion = investigacion.Evidencia_Posicion;
            investigacion_entity.Evidencia_Parte = investigacion.Evidencia_Parte;
            investigacion_entity.Estado = investigacion.Estado;

            impacto_entity.Id_Tipo_Suelo = impacto.Tipo_Suelo?.Id_Tipo_Suelo;
            impacto_entity.Area_Suelo = impacto.Area_Suelo;
            impacto_entity.Id_Unidad_Area_Suelo = impacto.Unidad_Area_Suelo?.Id_Unidad_Medida;
            impacto_entity.Cantidad_Suelo = impacto.Cantidad_Suelo;
            impacto_entity.Id_Unidad_Cantidad_Suelo = impacto.Unidad_Cantidad_Suelo?.Id_Unidad_Medida;
            impacto_entity.Id_Lugar_Disposicion_Suelo = impacto.Lugar_Disposicion_Suelo?.Id_Lugar_Disposicion;

            impacto_entity.Id_Especie_Flora = impacto.Especie_Flora?.Id_Especie_Flora;
            impacto_entity.Unidad_Flora = impacto.Unidad_Flora;
            impacto_entity.Id_Unidad_Flora = impacto.UnidadM_Flora?.Id_Unidad_Medida;
            impacto_entity.Cantidad_Residuo_Flora = impacto.Cantidad_Residuo_Flora;
            impacto_entity.Id_Unidad_Residuo_Flora = impacto.Unidad_Residuo_Flora?.Id_Unidad_Medida;
            impacto_entity.Id_Lugar_Disposicion_Flora = impacto.Lugar_Disposicion_Flora?.Id_Lugar_Disposicion;

            impacto_entity.Id_Especie_Fauna = impacto.Especie_Fauna?.Id_Especie_Fauna;
            impacto_entity.Unidad_Fauna = impacto.Unidad_Fauna;
            impacto_entity.Id_Unidad_Fauna = impacto.UnidadM_Fauna?.Id_Unidad_Medida;
            impacto_entity.Cantidad_Residuo_Fauna = impacto.Cantidad_Residuo_Fauna;
            impacto_entity.Id_Unidad_Residuo_Fauna = impacto.Unidad_Residuo_Fauna?.Id_Unidad_Medida;
            impacto_entity.Id_Lugar_Disposicion_Fauna = impacto.Lugar_Disposicion_Fauna?.Id_Lugar_Disposicion;

            impacto_entity.Id_Tipo_Agua = impacto.Tipo_Agua?.Id_Tipo_Agua;
            impacto_entity.Volumen_Agua = impacto.Volumen_Agua;
            impacto_entity.Id_Unidad_Agua = impacto.Unidad_Agua?.Id_Unidad_Medida;
            impacto_entity.Cantidad_Residuo_Agua = impacto.Cantidad_Residuo_Agua;
            impacto_entity.Id_Unidad_Residuo_Agua = impacto.Unidad_Residuo_Agua?.Id_Unidad_Medida;
            impacto_entity.Id_Lugar_Disposicion_Agua = impacto.Lugar_Disposicion_Agua?.Id_Lugar_Disposicion;

            impacto_entity.Restos_Arq = impacto.Restos_Arq;
            impacto_entity.Nombre_Resto_Arq = impacto.Nombre_Resto_Arq;
            impacto_entity.Cord_Este = impacto.Cord_Este;
            impacto_entity.Cord_Norte = impacto.Cord_Norte;
            
            impacto_entity.Danio_Salud = impacto.Danio_Salud;
            impacto_entity.Personas_Afectadas = impacto.Personas_Afectadas;
            impacto_entity.Cantidad_RS = impacto.Cantidad_RS;
            impacto_entity.Id_Unidad_Cantidad_RS = impacto.Unidad_Cantidad_RS?.Id_Unidad_Medida;
            impacto_entity.Volumen_Rs = impacto.Volumen_Rs;
            impacto_entity.Id_Unidad_Volumen_RS = impacto.Unidad_Volumen_RS?.Id_Unidad_Medida;
            impacto_entity.EO_RS = impacto.EO_RS;

            _context.Investigacion_Final_Incidente.Update(investigacion_entity);
            _context.Impacto_Ambiental_Incidente.Update(impacto_entity);

            await _context.SaveChangesAsync();

            return investigacion_entity.Id_Investigacion_Final;
        }

        public async Task<int> ImplementarAccionCorrectivaInvestigacion(Accion_Incidente_Investigacion accion)
        {
            var entity = await _context.Accion_Incidente_Investigacion.FirstOrDefaultAsync(x => x.Id_Accion_Incidente_Investigacion.Equals(accion.Id_Accion_Incidente_Investigacion));
            entity.Evidencia = accion.Evidencia;
            entity.Comentarios_Implementacion = accion.Comentarios_Implementacion;
            entity.Fecha_Implementacion = accion.Fecha_Implementacion;
            entity.Estado = accion.Estado;

            _context.Accion_Incidente_Investigacion.Update(entity);

            await _context.SaveChangesAsync();

            return entity.Id_Accion_Incidente_Investigacion;
        }

        public async Task<int> VerificarAccionCorrectivaInvestigacion(Accion_Incidente_Investigacion accion)
        {
            var entity = await _context.Accion_Incidente_Investigacion.FirstOrDefaultAsync(x => x.Id_Accion_Incidente_Investigacion.Equals(accion.Id_Accion_Incidente_Investigacion));
            entity.Comentarios_Verificacion = accion.Comentarios_Verificacion;
            entity.Estatus = accion.Estatus;
            entity.Fecha_Verificacion = accion.Fecha_Verificacion;
            entity.Estado = accion.Estado;

            _context.Accion_Incidente_Investigacion.Update(entity);

            await _context.SaveChangesAsync();

            return entity.Id_Accion_Incidente_Investigacion;
        }
        public async Task<int> AmpliarFechaAccion(Accion_Incidente_Investigacion accion)
        {
            var entity = await _context.Accion_Incidente_Investigacion.FirstOrDefaultAsync(x => x.Id_Accion_Incidente_Investigacion.Equals(accion.Id_Accion_Incidente_Investigacion));
            entity.Fecha = accion.Fecha;
            entity.Estado = accion.Estado;

            _context.Accion_Incidente_Investigacion.Update(entity);

            await _context.SaveChangesAsync();

            return entity.Id_Accion_Incidente_Investigacion;
        }
    }
}