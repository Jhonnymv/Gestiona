using GestionAmbiental.Application.Repositories.RAC;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.RAC
{
    public class GetRACRepository : IGetRACReadOnlyRepository
    {
        private readonly Context _context;
        public GetRACRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Domain.RAC.RAC> GetRAC(int Id_RAC)
        {
            var registro = await _context.RACs.FirstOrDefaultAsync(x => x.Id_RAC.Equals(Id_RAC));

            var empresa = await _context.Empresas.FirstOrDefaultAsync(x => x.Id_Empresa.Equals(registro.Id_Empresa));
            var gerencia = await _context.Gerencias.FirstOrDefaultAsync(x => x.Id_Gerencia.Equals(registro.Id_Gerencia));
            var empresa_obs = await _context.Empresas.FirstOrDefaultAsync(x => x.Id_Empresa.Equals(registro.Id_Empresa_Obs));
            var gerencia_obs = await _context.Gerencias.FirstOrDefaultAsync(x => x.Id_Gerencia.Equals(registro.Id_Gerencia_Obs));
            var tipo_condicion = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(registro.Id_Tipo_Condicion));
            var responsable_cierre = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(registro.Id_Responsable_Cierre));
            var supervisor = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(registro.Id_Supervisor));

            var output = new Domain.RAC.RAC {
                Id_RAC = registro.Id_RAC,
                Id_Registrado_Por = registro.Id_Registrado_Por,
                Codigo = registro.Codigo,
                Nombres_Apellidos = registro.Nombres_Apellidos,
                Id_Gerencia = registro.Id_Gerencia,
                Gerencia = new Gerencia(gerencia.Id_Gerencia, gerencia.Descripcion),
                Id_Empresa = registro.Id_Empresa,
                Empresa = new Empresa(empresa.Id_Empresa, empresa.Razon_Social),
                Otra_Empresa = registro.Otra_Empresa,
                Id_Empresa_Obs = registro.Id_Empresa_Obs,
                Empresa_Ob = new Empresa(empresa_obs.Id_Empresa, empresa_obs.Razon_Social),
                Id_Gerencia_Obs = registro.Id_Gerencia_Obs,
                Gerencia_Obs = new Gerencia(gerencia_obs.Id_Gerencia, gerencia_obs.Descripcion),
                Ubicacion = registro.Ubicacion,
                Fecha = registro.Fecha,
                SSO_MA = registro.SSO_MA,
                Clasificacion = registro.Clasificacion,
                NivelRiesgo = registro.NivelRiesgo,
                Id_Tipo_Condicion = registro.Id_Tipo_Condicion,
                TipoCondicion = new TipoCondicion(tipo_condicion.Id_Tipo_Condicion, tipo_condicion.Descripcion, tipo_condicion.Grupo),
                Descripcion_Acto_Cond = registro.Descripcion_Acto_Cond,
                Medida_Control = registro.Medida_Control,
                PAC = registro.PAC,
                Id_Responsable_Cierre = registro.Id_Responsable_Cierre,
                Responsable_Cierre = new Usuario(responsable_cierre.Id_Usuario, responsable_cierre.Id_Rol, responsable_cierre.usuario_red, responsable_cierre.Nombres, responsable_cierre.Apellidos, responsable_cierre.Email, responsable_cierre.Id_Superior, responsable_cierre.Estado),
                Fecha_Cierre = registro.Fecha_Cierre,
                Id_Supervisor = registro.Id_Supervisor,
                Supervisor = new Usuario(supervisor.Id_Usuario, supervisor.Id_Rol, supervisor.usuario_red, supervisor.Nombres, supervisor.Apellidos, supervisor.Email, supervisor.Id_Superior, supervisor.Estado),
                Evidencia_Obs = registro.Evidencia_Obs,
                Evidencia_Cierre = registro.Evidencia_Cierre,
                Comentario_Cierre = registro.Comentario_Cierre,
                Comentario_Verificacion = registro.Comentario_Verificacion,
                Estado = registro.Estado
            };

            return output;
        }
        public async Task<List<Domain.RAC.RAC>> GetRACs(DateTime fecha, string codigo, int id_Usuario, int filtroGerenciaObs)
        {
            var fechaFiltro = (fecha == DateTime.MinValue) ? (DateTime)SqlDateTime.MinValue : fecha;
            var registros = await _context.RACs.Where(c =>
                !c.Estado.Equals(EstadosRAC.Eliminado.ToString())&&
                c.Fecha.Date.Equals(fechaFiltro != (DateTime)SqlDateTime.MinValue ? fechaFiltro.Date : c.Fecha.Date)
                && c.Codigo.Contains(codigo != "" ? codigo : c.Codigo)
                && c.Id_Registrado_Por.Equals(id_Usuario != 0 ? id_Usuario : c.Id_Registrado_Por)
                && c.Id_Gerencia_Obs.Equals(filtroGerenciaObs != 0 ? filtroGerenciaObs : c.Id_Gerencia_Obs))
                .OrderByDescending(x => x.Codigo)
                .ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var tipos_condicion = await _context.Tipos_Condicion.ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();
            var output = new List<Domain.RAC.RAC>();
            foreach (var item in registros)
            {
                var empresa = empresas.FirstOrDefault(x => x.Id_Empresa.Equals(item.Id_Empresa));
                var gerencia = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(item.Id_Gerencia));
                var empresa_obs = empresas.FirstOrDefault(x => x.Id_Empresa.Equals(item.Id_Empresa_Obs));
                var gerencia_obs = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(item.Id_Gerencia_Obs));
                var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Id_Tipo_Condicion));
                var responsable_cierre = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Responsable_Cierre));
                var supervisor = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Supervisor));
                output.Add(new Domain.RAC.RAC
                {
                    Id_RAC = item.Id_RAC,
                    Id_Registrado_Por = item.Id_Registrado_Por,
                    Codigo = item.Codigo,
                    Nombres_Apellidos = item.Nombres_Apellidos,
                    Id_Gerencia = item.Id_Gerencia,
                    Gerencia = new Gerencia(gerencia.Id_Gerencia, gerencia.Descripcion),
                    Id_Empresa = item.Id_Empresa,
                    Empresa = new Empresa(empresa.Id_Empresa, empresa.Razon_Social),
                    Otra_Empresa = item.Otra_Empresa,
                    Id_Empresa_Obs = item.Id_Empresa_Obs,
                    Empresa_Ob = new Empresa(empresa_obs.Id_Empresa, empresa_obs.Razon_Social),
                    Id_Gerencia_Obs = item.Id_Gerencia_Obs,
                    Gerencia_Obs = new Gerencia(gerencia_obs.Id_Gerencia, gerencia_obs.Descripcion),
                    Ubicacion = item.Ubicacion,
                    Fecha = item.Fecha,
                    SSO_MA = item.SSO_MA,
                    Clasificacion = item.Clasificacion,
                    NivelRiesgo = item.NivelRiesgo,
                    Id_Tipo_Condicion = item.Id_Tipo_Condicion,
                    TipoCondicion = new TipoCondicion(tipo_condicion.Id_Tipo_Condicion, tipo_condicion.Descripcion, tipo_condicion.Grupo),
                    Descripcion_Acto_Cond = item.Descripcion_Acto_Cond,
                    Medida_Control = item.Medida_Control,
                    PAC = item.PAC,
                    Id_Responsable_Cierre = item.Id_Responsable_Cierre,
                    Responsable_Cierre = new Usuario(responsable_cierre.Id_Usuario, responsable_cierre.Id_Rol, responsable_cierre.usuario_red, responsable_cierre.Nombres, responsable_cierre.Apellidos, responsable_cierre.Email, responsable_cierre.Id_Superior, responsable_cierre.Estado),
                    Fecha_Cierre = item.Fecha_Cierre,
                    Id_Supervisor = item.Id_Supervisor,
                    Supervisor = new Usuario(supervisor.Id_Usuario, supervisor.Id_Rol, supervisor.usuario_red, supervisor.Nombres, supervisor.Apellidos, supervisor.Email, supervisor.Id_Superior, supervisor.Estado),
                    Evidencia_Obs = item.Evidencia_Obs,
                    Evidencia_Cierre = item.Evidencia_Cierre,
                    Estado = item.Estado
                });
            }
            return output;
        }
        public async Task<List<Domain.RAC.RAC>> GetRACsImplementacion(DateTime fecha, string codigo, int id_Usuario, int id_Gerencia_Obs)
        {
            var fechaFiltro = (fecha == DateTime.MinValue) ? (DateTime)SqlDateTime.MinValue : fecha;
            var registros = await _context.RACs.Where(c =>
                !c.Estado.Equals(EstadosRAC.Eliminado.ToString()) &&
                c.Fecha.Date.Equals(fechaFiltro != (DateTime)SqlDateTime.MinValue ? fechaFiltro.Date : c.Fecha.Date)
                && c.Codigo.Contains(codigo != "" ? codigo : c.Codigo)
                && c.Id_Responsable_Cierre.Equals(id_Usuario != 0 ? id_Usuario : c.Id_Responsable_Cierre)
                && c.Id_Gerencia_Obs.Equals(id_Gerencia_Obs != 0 ? id_Gerencia_Obs : c.Id_Gerencia_Obs)
                && (
                c.Estado.Equals(EstadosRAC.Registrado.ToString()) ||
                c.Estado.Equals(EstadosRAC.Ampliado.ToString()) ||
                c.Estado.Equals(EstadosRAC.Rechazado.ToString())
                ))
                .OrderByDescending(x => x.Codigo)
                .ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var tipos_condicion = await _context.Tipos_Condicion.ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();
            var output = new List<Domain.RAC.RAC>();
            foreach (var item in registros)
            {
                var empresa = empresas.FirstOrDefault(x => x.Id_Empresa.Equals(item.Id_Empresa));
                var gerencia = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(item.Id_Gerencia));
                var empresa_obs = empresas.FirstOrDefault(x => x.Id_Empresa.Equals(item.Id_Empresa_Obs));
                var gerencia_obs = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(item.Id_Gerencia_Obs));
                var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Id_Tipo_Condicion));
                var responsable_cierre = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Responsable_Cierre));
                var supervisor = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Supervisor));
                output.Add(new Domain.RAC.RAC
                {
                    Id_RAC = item.Id_RAC,
                    Id_Registrado_Por = item.Id_Registrado_Por,
                    Codigo = item.Codigo,
                    Nombres_Apellidos = item.Nombres_Apellidos,
                    Id_Gerencia = item.Id_Gerencia,
                    Gerencia = new Gerencia(gerencia.Id_Gerencia, gerencia.Descripcion),
                    Id_Empresa = item.Id_Empresa,
                    Empresa = new Empresa(empresa.Id_Empresa, empresa.Razon_Social),
                    Otra_Empresa = item.Otra_Empresa,
                    Id_Empresa_Obs = item.Id_Empresa_Obs,
                    Empresa_Ob = new Empresa(empresa_obs.Id_Empresa, empresa_obs.Razon_Social),
                    Id_Gerencia_Obs = item.Id_Gerencia_Obs,
                    Gerencia_Obs = new Gerencia(gerencia_obs.Id_Gerencia, gerencia_obs.Descripcion),
                    Ubicacion = item.Ubicacion,
                    Fecha = item.Fecha,
                    SSO_MA = item.SSO_MA,
                    Clasificacion = item.Clasificacion,
                    NivelRiesgo = item.NivelRiesgo,
                    Id_Tipo_Condicion = item.Id_Tipo_Condicion,
                    TipoCondicion = new TipoCondicion(tipo_condicion.Id_Tipo_Condicion, tipo_condicion.Descripcion, tipo_condicion.Grupo),
                    Descripcion_Acto_Cond = item.Descripcion_Acto_Cond,
                    Medida_Control = item.Medida_Control,
                    PAC = item.PAC,
                    Id_Responsable_Cierre = item.Id_Responsable_Cierre,
                    Responsable_Cierre = new Usuario(responsable_cierre.Id_Usuario, responsable_cierre.Id_Rol, responsable_cierre.usuario_red, responsable_cierre.Nombres, responsable_cierre.Apellidos, responsable_cierre.Email, responsable_cierre.Id_Superior, responsable_cierre.Estado),
                    Fecha_Cierre = item.Fecha_Cierre,
                    Id_Supervisor = item.Id_Supervisor,
                    Supervisor = new Usuario(supervisor.Id_Usuario, supervisor.Id_Rol, supervisor.usuario_red, supervisor.Nombres, supervisor.Apellidos, supervisor.Email, supervisor.Id_Superior, supervisor.Estado),
                    Evidencia_Obs = item.Evidencia_Obs,
                    Evidencia_Cierre = item.Evidencia_Cierre,
                    Estado = item.Estado
                });
            }
            return output;
        }
        public async Task<List<Domain.RAC.RAC>> GetRACsVerificacion(DateTime fecha, string codigo, int id_Usuario, int id_Gerencia_Obs)
        {
            var fechaFiltro = (fecha == DateTime.MinValue) ? (DateTime)SqlDateTime.MinValue : fecha;
            var registros = await _context.RACs.Where(c =>
                !c.Estado.Equals(EstadosRAC.Eliminado.ToString()) &&
                c.Fecha.Date.Equals(fechaFiltro != (DateTime)SqlDateTime.MinValue ? fechaFiltro.Date : c.Fecha.Date)
                && c.Codigo.Contains(codigo != "" ? codigo : c.Codigo)
                && c.Id_Supervisor.Equals(id_Usuario != 0 ? id_Usuario : c.Id_Supervisor)
                && c.Id_Gerencia_Obs.Equals(id_Gerencia_Obs != 0 ? id_Gerencia_Obs:c.Id_Gerencia_Obs)
                && c.Estado.Equals(EstadosRAC.Implementado.ToString()))
                .OrderByDescending(x => x.Codigo)
                .ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var tipos_condicion = await _context.Tipos_Condicion.ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();
            var output = new List<Domain.RAC.RAC>();
            foreach (var item in registros)
            {
                var empresa = empresas.FirstOrDefault(x => x.Id_Empresa.Equals(item.Id_Empresa));
                var gerencia = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(item.Id_Gerencia));
                var empresa_obs = empresas.FirstOrDefault(x => x.Id_Empresa.Equals(item.Id_Empresa_Obs));
                var gerencia_obs = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(item.Id_Gerencia_Obs));
                var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Id_Tipo_Condicion));
                var responsable_cierre = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Responsable_Cierre));
                var supervisor = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Supervisor));
                output.Add(new Domain.RAC.RAC
                {
                    Id_RAC = item.Id_RAC,
                    Id_Registrado_Por = item.Id_Registrado_Por,
                    Codigo = item.Codigo,
                    Nombres_Apellidos = item.Nombres_Apellidos,
                    Id_Gerencia = item.Id_Gerencia,
                    Gerencia = new Gerencia(gerencia.Id_Gerencia, gerencia.Descripcion),
                    Id_Empresa = item.Id_Empresa,
                    Empresa = new Empresa(empresa.Id_Empresa, empresa.Razon_Social),
                    Otra_Empresa = item.Otra_Empresa,
                    Id_Empresa_Obs = item.Id_Empresa_Obs,
                    Empresa_Ob = new Empresa(empresa_obs.Id_Empresa, empresa_obs.Razon_Social),
                    Id_Gerencia_Obs = item.Id_Gerencia_Obs,
                    Gerencia_Obs = new Gerencia(gerencia_obs.Id_Gerencia, gerencia_obs.Descripcion),
                    Ubicacion = item.Ubicacion,
                    Fecha = item.Fecha,
                    SSO_MA = item.SSO_MA,
                    Clasificacion = item.Clasificacion,
                    NivelRiesgo = item.NivelRiesgo,
                    Id_Tipo_Condicion = item.Id_Tipo_Condicion,
                    TipoCondicion = new TipoCondicion(tipo_condicion.Id_Tipo_Condicion, tipo_condicion.Descripcion, tipo_condicion.Grupo),
                    Descripcion_Acto_Cond = item.Descripcion_Acto_Cond,
                    Medida_Control = item.Medida_Control,
                    PAC = item.PAC,
                    Id_Responsable_Cierre = item.Id_Responsable_Cierre,
                    Responsable_Cierre = new Usuario(responsable_cierre.Id_Usuario, responsable_cierre.Id_Rol, responsable_cierre.usuario_red, responsable_cierre.Nombres, responsable_cierre.Apellidos, responsable_cierre.Email, responsable_cierre.Id_Superior, responsable_cierre.Estado),
                    Fecha_Cierre = item.Fecha_Cierre,
                    Id_Supervisor = item.Id_Supervisor,
                    Supervisor = new Usuario(supervisor.Id_Usuario, supervisor.Id_Rol, supervisor.usuario_red, supervisor.Nombres, supervisor.Apellidos, supervisor.Email, supervisor.Id_Superior, supervisor.Estado),
                    Evidencia_Obs = item.Evidencia_Obs,
                    Evidencia_Cierre = item.Evidencia_Cierre,
                    Estado = item.Estado
                });
            }
            return output;
        }
    }
}
