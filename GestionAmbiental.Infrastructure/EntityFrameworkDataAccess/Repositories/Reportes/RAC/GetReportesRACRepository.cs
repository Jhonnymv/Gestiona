using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.RAC;
using GestionAmbiental.Domain.Reportes;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Reportes.RAC
{
    public class GetReportesRACRepository : IGetReportesRACReadOnlyRepository
    {
        private Context _context;
        public GetReportesRACRepository(Context context)
        {
            _context = context;
        }
        public async Task<List<Domain.RAC.RAC>> GetRACs(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            var reportes = await _context.RACs
                .Where(x=>
                x.Id_Gerencia.Equals(IdGerencia>0?IdGerencia:x.Id_Gerencia)&&
                x.Id_Empresa.Equals(IdEmpresa>0?IdEmpresa:x.Id_Empresa)&&
                x.Id_Responsable_Cierre.Equals(IdUsuario>0?IdUsuario:x.Id_Responsable_Cierre) &&
                x.Fecha >= FechaInicio &&
                x.Fecha <= FechaFin)
                .ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();
            var tipoActoCondicion = await _context.Tipos_Condicion.ToListAsync();

            var output = new List<Domain.RAC.RAC>();

            foreach (var item in reportes)
            {
                var empresaDomain = empresas.Find(x => x.Id_Empresa.Equals(item.Id_Empresa));
                var empresaObDomain = empresas.Find(x => x.Id_Empresa.Equals(item.Id_Empresa_Obs));
                var gerenciaDomain = gerencias.Find(x => x.Id_Gerencia.Equals(item.Id_Gerencia));
                var gerenciaObDomain = gerencias.Find(x => x.Id_Gerencia.Equals(item.Id_Gerencia_Obs));
                var registradoPorDomain = usuarios.Find(x => x.Id_Usuario.Equals(item.Id_Registrado_Por));
                var responsableDomain = usuarios.Find(x => x.Id_Usuario.Equals(item.Id_Responsable_Cierre));
                var supervisorDomain = usuarios.Find(x => x.Id_Usuario.Equals(item.Id_Supervisor));
                var tipoActoCondicionDomain = tipoActoCondicion.Find(x => x.Id_Tipo_Condicion.Equals(item.Id_Tipo_Condicion));

                output.Add(new Domain.RAC.RAC
                {
                    Clasificacion = item.Clasificacion,
                    Codigo = item.Codigo,
                    Comentario_Cierre = item.Comentario_Cierre,
                    Comentario_Verificacion = item.Comentario_Verificacion,
                    Descripcion_Acto_Cond = item.Descripcion_Acto_Cond,
                    Empresa = new Empresa
                    {
                        IdEmpresa = empresaDomain.Id_Empresa,
                        RazonSocial = empresaDomain.Razon_Social,
                        Estado = empresaDomain.Estado,
                    },
                    Empresa_Ob = new Empresa
                    {
                        IdEmpresa = empresaObDomain.Id_Empresa,
                        RazonSocial = empresaObDomain.Razon_Social,
                        Estado = empresaObDomain.Estado,
                    },
                    Fecha_Cierre = item.Fecha_Cierre,
                    Fecha = item.Fecha,
                    Estado = item.Estado,
                    Evidencia_Obs = item.Evidencia_Obs,
                    Evidencia_Cierre = item.Evidencia_Cierre,
                    Gerencia = new Gerencia
                    {
                        IdGerencia = gerenciaDomain.Id_Gerencia,
                        Descripcion = gerenciaDomain.Descripcion,
                        Estado = gerenciaDomain.Estado
                    },
                    Gerencia_Obs = new Gerencia
                    {
                        IdGerencia = gerenciaObDomain.Id_Gerencia,
                        Descripcion = gerenciaObDomain.Descripcion,
                        Estado = gerenciaObDomain.Estado
                    },
                    Id_RAC = item.Id_RAC,
                    Id_Registrado_Por = item.Id_Registrado_Por,
                    NivelRiesgo = item.NivelRiesgo,
                    Medida_Control = item.Medida_Control,
                    TipoCondicion = new TipoCondicion
                    {
                        IdTipoCondicion = tipoActoCondicionDomain.Id_Tipo_Condicion,
                        Descripcion = tipoActoCondicionDomain.Descripcion,
                        Grupo = tipoActoCondicionDomain.Grupo
                    },
                    Nombres_Apellidos = item.Nombres_Apellidos,
                    Otra_Empresa = item.Otra_Empresa,
                    PAC = item.PAC,
                    Responsable_Cierre = new Usuario
                    {
                        Nombres = responsableDomain.Nombres,
                        Apellidos = responsableDomain.Apellidos,
                        DNI = responsableDomain.DNI,
                        Puesto = responsableDomain.Puesto,
                        Email = responsableDomain.Email,
                        Celular = responsableDomain.Celular,
                        Estado = responsableDomain.Estado,
                        UsuarioRed = responsableDomain.usuario_red
                    },
                    Supervisor = new Usuario
                    {
                        Nombres = supervisorDomain.Nombres,
                        Apellidos = supervisorDomain.Apellidos,
                        DNI = supervisorDomain.DNI,
                        Puesto = supervisorDomain.Puesto,
                        Email = supervisorDomain.Email,
                        Celular = supervisorDomain.Celular,
                        Estado = supervisorDomain.Estado,
                        UsuarioRed = supervisorDomain.usuario_red
                    },
                    SSO_MA = item.SSO_MA,
                    Ubicacion = item.Ubicacion                    
                });
            }
            return output;
        } public async Task<List<Dimension>> GetObservacionesXEstado(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            var reportes = await _context.RACs
                .Where(x=>
                x.Id_Gerencia.Equals(IdGerencia>0?IdGerencia:x.Id_Gerencia)&&
                x.Id_Empresa.Equals(IdEmpresa>0?IdEmpresa:x.Id_Empresa)&&
                x.Id_Responsable_Cierre.Equals(IdUsuario>0?IdUsuario:x.Id_Responsable_Cierre) &&
                x.Fecha >= FechaInicio &&
                x.Fecha <= FechaFin)
                .ToListAsync();

            var output = new List<Dimension>();

            foreach (var item in Enum.GetNames(typeof(EstadosRAC)))
            {
                output.Add(new Dimension(item, reportes.Where(x => x.Estado.Equals(item)).Count()));
            }
            var totalRAC = await _context.RACs.CountAsync();
            output.Add(new Dimension("total", totalRAC));
            return output;
        }
        public async Task<List<Domain.RAC.RAC>> GetDetalleReportesXEstado(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin, EstadosRAC estado)
        {
            var output = new List<Domain.RAC.RAC>();
            var reportes = await _context.RACs
                .Where(x =>
                x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
                x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
                x.Id_Responsable_Cierre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Cierre) &&
                x.Fecha >= FechaInicio &&
                x.Fecha <= FechaFin &&
                x.Estado.Equals(estado.ToString()))
                .ToListAsync();

            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var tipos_condicion = await _context.Tipos_Condicion.ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();

            foreach (var item in reportes)
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
                    Gerencia = new Gerencia{ IdGerencia = gerencia.Id_Gerencia, Descripcion = gerencia.Descripcion, Estado = gerencia.Estado },
                    Id_Empresa = item.Id_Empresa,
                    Empresa = new Empresa(empresa.Id_Empresa, empresa.Razon_Social),
                    Otra_Empresa = item.Otra_Empresa,
                    Id_Empresa_Obs = item.Id_Empresa_Obs,
                    Empresa_Ob = new Empresa(empresa_obs.Id_Empresa, empresa_obs.Razon_Social),
                    Id_Gerencia_Obs = item.Id_Gerencia_Obs,
                    Gerencia_Obs = new Gerencia{ IdGerencia = gerencia_obs.Id_Gerencia, Descripcion = gerencia_obs.Descripcion, Estado = gerencia_obs.Estado },
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
        public async Task<List<Dimension>> GetNReportesEntreFechas(int IdGerencia, int IdEmpresa, int IdUsuario,DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new List<Dimension>();

            var reportes = await _context.RACs
                .Where(x =>
                x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
                x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
                x.Id_Responsable_Cierre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Cierre)&&
                x.Fecha >= FechaInicio&&
                x.Fecha <= FechaFin)
                .ToListAsync();

            var nReportesxFecha = reportes.Select(x => x.Fecha).GroupBy(y => y.Date).ToList();

            int nDias = 0;
            //Calcular n días entre fechas de distinto año (considerando años biciestos)
            if (FechaFin.Year>FechaInicio.Year)
                nDias = FechaFin.DayOfYear + (FechaInicio.Year%4==0?366:365) - FechaInicio.DayOfYear;
            else
                nDias = FechaFin.DayOfYear - FechaInicio.DayOfYear;
            var lstDiasSinValor = new List<Dimension>();
            var lstDiasConValor = new List<Dimension>();

            for (int i = nDias; i >= 0; i--)
            {
                lstDiasSinValor.Add(new Dimension(FechaFin.AddDays(-i).ToString("dd MMMM").Substring(0, 6), 0, i));
            }

            foreach (var item in nReportesxFecha)
            {
                lstDiasConValor.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), item.Count(), FechaFin.DayOfYear-item.Key.DayOfYear));
            }
            var distinct = lstDiasSinValor.Where(x => !lstDiasConValor.Select(y =>y.Nombre).ToList().Contains(x.Nombre)).ToList();
            //var distinct = lstDiasSinValor.Where(x => !lstDiasConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
            output = distinct.Union(lstDiasConValor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();

            //var union = distinct.Union(output).OrderBy(x=>x.Nombre).ToList();
            return output;
        }
        public async Task<List<Dimension>> GetReportesNivelRiesgoMA(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            var data = await _context.RACs.ToListAsync();
            var reportes= data.Where(x =>
                x.SSO_MA.Equals("MA") &&
                x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
                x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
                x.Id_Responsable_Cierre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Cierre) &&
                x.Fecha >= FechaInicio &&
                x.Fecha <= FechaFin)
                .GroupBy(y=>y.NivelRiesgo)
                .ToList();

            var output = new List<Dimension>();
            
            output.Add(new Dimension("Tolerable", 0, 0));
            output.Add(new Dimension("Alerta", 0, 1));
            output.Add(new Dimension("No Aceptable", 0, 2));
            foreach (var item in reportes)
            {
                switch (item.Key)
                {
                    case "Tolerable": output.FirstOrDefault(x => x.Nombre.Equals("Tolerable")).Valor = item.Count(); break;
                    case "Alerta": output.FirstOrDefault(x => x.Nombre.Equals("Alerta")).Valor = item.Count(); break;
                    case "No Aceptable": output.FirstOrDefault(x => x.Nombre.Equals("No Aceptable")).Valor = item.Count(); break;
                }
            }
            return output;
        }
        public async Task<List<Dimension>> GetReportesNivelRiesgoSSO(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            var data = await _context.RACs.ToListAsync();
            var reportes = data.Where(x =>
                 x.SSO_MA.Equals("SSO") &&
                x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
                x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
                x.Id_Responsable_Cierre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Cierre) &&
                x.Fecha >= FechaInicio &&
                x.Fecha <= FechaFin)
                .GroupBy(y => y.NivelRiesgo)
                .ToList();

            var output = new List<Dimension>();

            output.Add(new Dimension("Bajo", 0, 0));
            output.Add(new Dimension("Medio", 0, 1));
            output.Add(new Dimension("Alto", 0, 2));
            foreach (var item in reportes)
            {
                switch (item.Key)
                {
                    case "Alto": output.FirstOrDefault(x => x.Nombre.Equals("Alto")).Valor = item.Count(); break;
                    case "Medio": output.FirstOrDefault(x => x.Nombre.Equals("Medio")).Valor = item.Count(); break;
                    case "Bajo": output.FirstOrDefault(x => x.Nombre.Equals("Bajo")).Valor = item.Count(); break;
                }
            }
            return output;
        }
        public async Task<List<Dimension>> GetReportesClasificacionMA(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            var data = await _context.RACs.ToListAsync();
            var reportes = data.Where(x =>
                 x.SSO_MA.Equals("MA") &&
                x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
                x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
                x.Id_Responsable_Cierre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Cierre) &&
                x.Fecha >= FechaInicio &&
                x.Fecha <= FechaFin)
                .GroupBy(x=>x.Clasificacion)
                .ToList();

            var output = new List<Dimension>();

            output.Add(new Dimension("Condición Subestándar", 0, 1));
            output.Add(new Dimension("Acto Subestándar", 0, 2));
            
            foreach (var item in reportes)
            {
                switch (item.Key)
                {
                    case "Condición Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Condición Subestándar")).Valor = item.Count(); break;
                    case "Acto Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Acto Subestándar")).Valor = item.Count(); break;
                }
            }
            return output;
        }
        public async Task<List<Dimension>> GetReportesClasificacionSSO(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            var data = await _context.RACs.ToListAsync();
            var reportes = data.Where(x =>
                 x.SSO_MA.Equals("SSO") &&
                x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
                x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
                x.Id_Responsable_Cierre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Cierre) &&
                x.Fecha >= FechaInicio &&
                x.Fecha <= FechaFin)
                .GroupBy(x => x.Clasificacion)
                .ToList();

            var output = new List<Dimension>();

            output.Add(new Dimension("Condición Subestándar", 0, 1));
            output.Add(new Dimension("Acto Subestándar", 0, 2));

            foreach (var item in reportes)
            {
                switch (item.Key)
                {
                    case "Condición Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Condición Subestándar")).Valor = item.Count(); break;
                    case "Acto Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Acto Subestándar")).Valor = item.Count(); break;
                }
            }
            return output;
        }
        public async Task<List<List<Dimension>>> GetReportesTipoCondicionxNivelRiesgoMA(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            var data = await _context.RACs.ToListAsync();
            var reportes = data.Where(x =>
                 x.SSO_MA.Equals("MA") &&
                x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
                x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
                x.Id_Responsable_Cierre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Cierre) &&
                x.Fecha >= FechaInicio &&
                x.Fecha <= FechaFin)
                .GroupBy(y => y.NivelRiesgo)
                .ToList();

            var tipos_condicion = await _context.Tipos_Condicion.ToListAsync();

            var output = new List<List<Dimension>>();
            var list_no_aceptable = new List<Dimension>();
            var list_alerta = new List<Dimension>();
            var list_tolerable = new List<Dimension>();

            foreach (var item in reportes)
            {
                switch (item.Key)
                {
                    case "Tolerable":
                        var tipos_tolerable = item.GroupBy(x => x.Id_Tipo_Condicion).ToList();
                        foreach (var subitem in tipos_tolerable)
                        {
                            var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(subitem.Key));
                            list_tolerable.Add(new Dimension { Nombre = tipo_condicion.Descripcion,Valor= subitem.Count()});
                        }
                        break;
                    case "Alerta":
                        var tipos_alerta = item.GroupBy(x => x.Id_Tipo_Condicion).ToList();
                        foreach (var subitem in tipos_alerta)
                        {
                            var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(subitem.Key));
                            list_alerta.Add(new Dimension { Nombre = tipo_condicion.Descripcion, Valor = subitem.Count() });
                        }
                        break;
                    case "No Aceptable":
                        var tipos_no_aceptable = item.GroupBy(x => x.Id_Tipo_Condicion).ToList();
                        foreach (var subitem in tipos_no_aceptable)
                        {
                            var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(subitem.Key));
                            list_no_aceptable.Add(new Dimension { Nombre = tipo_condicion.Descripcion, Valor = subitem.Count() });
                        }
                        break;
                }
            }
            
            output.Add(list_tolerable);
            output.Add(list_alerta);
            output.Add(list_no_aceptable);

            return output;
        }
        public async Task<List<List<Dimension>>> GetReportesTipoCondicionxNivelRiesgoSSO(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            var data = await _context.RACs.ToListAsync();
            var reportes = data.Where(x =>
                 x.SSO_MA.Equals("SSO") &&
                x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
                x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
                x.Id_Responsable_Cierre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Cierre) &&
                x.Fecha >= FechaInicio &&
                x.Fecha <= FechaFin)
                .GroupBy(y => y.NivelRiesgo)
                .ToList();

            var tipos_condicion = await _context.Tipos_Condicion.ToListAsync();

            var output = new List<List<Dimension>>();
            var list_alto = new List<Dimension>();
            var list_medio = new List<Dimension>();
            var list_bajo = new List<Dimension>();

            foreach (var item in reportes)
            {
                switch (item.Key)
                {
                    case "Alto":
                        var tipos_alto = item.GroupBy(x => x.Id_Tipo_Condicion).ToList();
                        foreach (var subitem in tipos_alto)
                        {
                            var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(subitem.Key));
                            list_alto.Add(new Dimension { Nombre = tipo_condicion.Descripcion, Valor = subitem.Count() });
                        }
                        break;
                    case "Medio":
                        var tipos_medio = item.GroupBy(x => x.Id_Tipo_Condicion).ToList();
                        foreach (var subitem in tipos_medio)
                        {
                            var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(subitem.Key));
                            list_medio.Add(new Dimension { Nombre = tipo_condicion.Descripcion, Valor = subitem.Count() });
                        }
                        break;
                    case "Bajo":
                        var tipos_bajo = item.GroupBy(x => x.Id_Tipo_Condicion).ToList();
                        foreach (var subitem in tipos_bajo)
                        {
                            var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(subitem.Key));
                            list_bajo.Add(new Dimension { Nombre = tipo_condicion.Descripcion, Valor = subitem.Count() });
                        }
                        break;
                }
            }
            
            output.Add(list_bajo);
            output.Add(list_medio);
            output.Add(list_alto);

            return output;
        }
    }
}
