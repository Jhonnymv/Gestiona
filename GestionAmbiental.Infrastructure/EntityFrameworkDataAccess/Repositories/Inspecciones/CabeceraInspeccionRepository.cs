using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Inspecciones
{
    public class CabeceraInspeccionRepository : ICabeceraInspeccionReadOnlyRepository
    {
        private readonly Context _context;

        public CabeceraInspeccionRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<InspeccionCabecera>> GetInspeccionCabecera(DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] empresaFiltro, int[] registradoPorFiltro, List<string> estadoFiltro, int[] responsableFiltro, int[] asesorFiltro, string codigo, string[] tipo_inspeccion_filtro, string[] tipo_observacion_filtro)
        {
            var cabeceras = await _context.Inspecciones_Cabecera.ToListAsync();
            cabeceras = cabeceras.Where(c =>
                c.Codigo.Contains(codigo ?? c.Codigo) &&
                c.Fecha_Inspeccion >= fechaInicio &&
                c.Fecha_Inspeccion <= fechaFin &&
                (c.Estado.Equals(EstadosInspeccion.Registrado.ToString()) || c.Estado.Equals(EstadosInspeccion.Pendiente.ToString())))
                .OrderByDescending(x=>x.Codigo)
                .ToList();

            var usuarios = await _context.Usuarios.ToListAsync();

            if (gerenciaFiltro.Length > 0)                
                cabeceras = cabeceras.Where(x => x.Id_Gerencia_Inspeccionada.HasValue && gerenciaFiltro.Contains(x.Id_Gerencia_Inspeccionada.Value)).ToList();
            
            if (empresaFiltro.Length > 0)            
                cabeceras = cabeceras.Where(x => empresaFiltro.Contains(x.Id_Empresa)).ToList();
            
            if (registradoPorFiltro.Length > 0)
                cabeceras = cabeceras.Where(x => registradoPorFiltro.Contains(x.Id_Registrado_Por)).ToList(); 
            
            if (tipo_inspeccion_filtro.Length > 0)
                cabeceras = cabeceras.Where(x => x.Tipo_Inspeccion!=null && tipo_inspeccion_filtro.Contains(x.Tipo_Inspeccion)).ToList();

            var observaciones = await _context.Inspecciones_Detalle.Where(x => !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString())).ToListAsync();
            if (tipo_observacion_filtro.Length > 0)
                observaciones = observaciones.Where(x => tipo_observacion_filtro.Contains(x.Tipo_Observacion)).ToList();

            var idCabecerasConObs = observaciones.GroupBy(y => y.Id_Inspeccion_Cabecera).Select(z => z.Key).ToList();
            var cabecerasSinObs = cabeceras.Where(x => !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) && !idCabecerasConObs.Contains(x.Id_Inspeccion_Cabecera)).OrderByDescending(x => x.Fecha_Inspeccion).ToList();
            var cabecerasConObs = cabeceras.Where(x => idCabecerasConObs.Contains(x.Id_Inspeccion_Cabecera)).ToList();

            if (responsableFiltro.Length > 0 || asesorFiltro.Length > 0 || estadoFiltro.Count > 0)
            {
                cabecerasSinObs = new List<Entities.Inspecciones.Inspeccion_Cabecera>();
                if (responsableFiltro.Length > 0)
                    observaciones = observaciones.Where(x => responsableFiltro.Contains(x.Id_Responsable_Marcobre)).ToList();
                if (asesorFiltro.Length > 0)
                    observaciones = observaciones.Where(x => asesorFiltro.Contains(x.Id_Asesor)).ToList();
                if (estadoFiltro.Count > 0)
                {
                    observaciones = observaciones.Where(x => estadoFiltro.Contains(x.Estado)).ToList();
                    var BPRechazadas = new List<int>();
                    foreach(var observacion in observaciones)
                    {
                        if (observacion.Descripcion_Condicion == "Buenas Prácticas" && observacion.Estado.Equals(EstadosInspeccion.Rechazado.ToString()))
                            BPRechazadas.Add(observacion.Id_Inspeccion_Detalle);                            
                    }
                    observaciones = observaciones.Where(x => !BPRechazadas.Contains(x.Id_Inspeccion_Detalle)).ToList();
                }

            }
            
            //cabeceras = cabeceras.Where(x => observaciones.GroupBy(y => y.Id_Inspeccion_Cabecera).Select(z => z.Key).Contains(x.Id_Inspeccion_Cabecera)).ToList(); 
            cabeceras = cabecerasConObs;
            cabeceras.InsertRange(0, cabecerasSinObs);
            
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var detalles_cabecera = await _context.Inspecciones_Detalle
                .Where(e => cabeceras.Select(c => c.Id_Inspeccion_Cabecera).Contains(e.Id_Inspeccion_Cabecera)&&
                e.Estado!=EstadosInspeccion.Eliminado.ToString())
                .ToListAsync();

            var TablaResumen = new List<InspeccionCabecera>();
            foreach (var item in cabeceras)
            {
                var registradoPor = usuarios.FirstOrDefault(g => g.Id_Usuario.Equals(item.Id_Registrado_Por));
                var gerEjectMar = gerencias.FirstOrDefault(g => g.Id_Gerencia.Equals(item.Id_Gerencia));
                var gerInspec = gerencias.FirstOrDefault(g => item.Id_Gerencia_Inspeccionada.HasValue && g.Id_Gerencia.Equals(item.Id_Gerencia_Inspeccionada));
                var empContMar = empresas.FirstOrDefault(e => e.Id_Empresa.Equals(item.Id_Empresa));
                var nObservaciones = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera)&&d.Estado!=EstadosInspeccion.Eliminado.ToString()).Count();
                var nObsImplementadas = detalles_cabecera.Where(
                    d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera) && 
                    d.Estado.Equals(EstadosInspeccion.Implementado.ToString())).Count();
                var nObsAprobadas = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera) && d.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).Count();
                var nObsRechazadas = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera) && d.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).Count();

                var cabecera = new InspeccionCabecera
                {
                    IdCabecera = item.Id_Inspeccion_Cabecera,
                    RegistradoPor = registradoPor.Nombres.ToUpper()+" "+registradoPor.Apellidos.ToUpper(),
                    Codigo = item.Codigo,
                    Gerencia = gerEjectMar.Descripcion,
                    GerenciaInspeccionada = gerInspec?.Descripcion,
                    Empresa = empContMar.Razon_Social,
                    AreaInspeccionada = item.Area_Inspeccionada,
                    FechaInspeccion = item.Fecha_Inspeccion,
                    NObservaciones = nObservaciones,
                    Estado = item.Estado,
                    NObsImplementadas = nObsImplementadas,
                    NObsAprobadas = nObsAprobadas,
                    NObsRechazadas = nObsRechazadas,
                    TipoInspeccion = item.Tipo_Inspeccion
                };
                TablaResumen.Add(cabecera);
            }

            return TablaResumen;
        }

        public async Task<InspeccionCabecera> GetInspeccionCabecera(int idCabecera)
        {
            var cabecera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera)&&x.Estado!=EstadosInspeccion.Eliminado.ToString()).ToListAsync();
            var output = new InspeccionCabecera
            {
                IdCabecera = cabecera.Id_Inspeccion_Cabecera,
                IdRegistradoPor = cabecera.Id_Registrado_Por,
                Codigo = cabecera.Codigo,
                Codigo_Formato = cabecera.Codigo_Formato,
                Area_Formato = cabecera.Area_Formato,
                Id_Miembro_Comite = cabecera.Id_Miembro_Comite,
                Participantes_Inspección = cabecera.Participantes_Inspección,
                IdGerencia = cabecera.Id_Gerencia,
                IdGerenciaInspeccionada = cabecera.Id_Gerencia_Inspeccionada,
                IdEmpresa = cabecera.Id_Empresa,
                TipoInspeccion = cabecera.Tipo_Inspeccion,
                AreaInspeccionada = cabecera.Area_Inspeccionada,
                FechaInspeccion = cabecera.Fecha_Inspeccion,
                NObservaciones = observaciones.Count,
                Estado = cabecera.Estado,
                IdResponsableArea = cabecera.Id_Responsable_Area,
                Objetivo = cabecera.Objetivo,
                ResponsableInspeccion = cabecera.Responsable_Inspeccion,
                Participantes = cabecera.Participantes
            };

            return output;
        }

        public async Task<List<InspeccionCabecera>> GetInspeccionCabeceraImplementacion(DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] empresaFiltro, int[] registradoPorFiltro, int[] responsableFiltro, int[] asesorFiltro, string codigo, string[] tipo_inspeccion_filtro, string[] tipo_observacion_filtro)
        {
            var obsPendImp = await _context.Inspecciones_Detalle
                 .Where(x =>
                 x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                 x.Estado.Equals(EstadosInspeccion.Vencido.ToString()) ||
                 x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                 x.Estado.Equals(EstadosInspeccion.Rechazado.ToString()))
                 .ToListAsync();

            var BPRechazados = new List<Entities.Inspecciones.Inspeccion_Detalle>();
            foreach(var item in obsPendImp)
            {
                if (item.Descripcion_Condicion.Equals("Buenas Prácticas") && item.Estado.Equals(EstadosInspeccion.Rechazado.ToString()))
                    BPRechazados.Add(item);
            }
            obsPendImp = obsPendImp.Where(x => !BPRechazados.Contains(x)).ToList();

            if (tipo_observacion_filtro.Length > 0)
                obsPendImp = obsPendImp.Where(x => tipo_observacion_filtro.Contains(x.Tipo_Observacion)).ToList();

            var cabeceras = await _context.Inspecciones_Cabecera.ToListAsync();
            cabeceras = cabeceras.Where(c =>
                c.Codigo.Contains(codigo ?? c.Codigo) &&
                c.Fecha_Inspeccion >= fechaInicio &&
                c.Fecha_Inspeccion <= fechaFin &&
                c.Estado.Equals(EstadosInspeccion.Registrado.ToString()))
                .OrderByDescending(x => x.Codigo)
                .ToList();

            if (tipo_inspeccion_filtro.Length > 0)
                cabeceras = cabeceras.Where(x => x.Tipo_Inspeccion != null && tipo_inspeccion_filtro.Contains(x.Tipo_Inspeccion)).ToList();

            if (gerenciaFiltro.Length > 0)
                cabeceras = cabeceras.Where(x => x.Id_Gerencia_Inspeccionada.HasValue && gerenciaFiltro.Contains(x.Id_Gerencia_Inspeccionada.Value)).ToList();

            if (empresaFiltro.Length > 0)
                cabeceras = cabeceras.Where(x => empresaFiltro.Contains(x.Id_Empresa)).ToList();

            if (registradoPorFiltro.Length > 0)
                cabeceras = cabeceras.Where(x => registradoPorFiltro.Contains(x.Id_Registrado_Por)).ToList();

            if (responsableFiltro.Length > 0 || asesorFiltro.Length > 0 )
            {
                if (responsableFiltro.Length > 0)
                    obsPendImp = obsPendImp.Where(x => responsableFiltro.Contains(x.Id_Responsable_Marcobre)).ToList();
                if (asesorFiltro.Length > 0)
                    obsPendImp = obsPendImp.Where(x => asesorFiltro.Contains(x.Id_Asesor)).ToList();
            }
            cabeceras = cabeceras.Where(x => obsPendImp.GroupBy(y => y.Id_Inspeccion_Cabecera).Select(z => z.Key).Contains(x.Id_Inspeccion_Cabecera)).ToList();

            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var detalles_cabecera = await _context.Inspecciones_Detalle.Where(e => cabeceras.Select(c => c.Id_Inspeccion_Cabecera).Contains(e.Id_Inspeccion_Cabecera) && e.Estado != EstadosInspeccion.Eliminado.ToString()).ToListAsync();

            var TablaResumen = new List<InspeccionCabecera>();
            foreach (var item in cabeceras)
            {
                var gerEjectMar = gerencias.FirstOrDefault(g => g.Id_Gerencia.Equals(item.Id_Gerencia));
                var gerInspec = gerencias.FirstOrDefault(g => item.Id_Gerencia_Inspeccionada.HasValue && g.Id_Gerencia.Equals(item.Id_Gerencia_Inspeccionada.Value));
                var empContMar = empresas.FirstOrDefault(e => e.Id_Empresa.Equals(item.Id_Empresa));
                var nObservaciones = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera)).Count();

                TablaResumen.Add(new InspeccionCabecera(item.Id_Inspeccion_Cabecera, item.Codigo, gerEjectMar.Descripcion,
                    empContMar.Razon_Social, item.Area_Inspeccionada, item.Fecha_Inspeccion, nObservaciones, item.Estado, gerInspec?.Descripcion));
            }
            return TablaResumen;
        }
        public async Task<InspeccionCabecera> GetInspeccionCabeceraImplementacion(int idCabecera)
        {
            var cabecera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera) && x.Estado != EstadosInspeccion.Eliminado.ToString()).ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();
            var miembros_comite = await _context.MiembrosComiteInspeccion.ToListAsync();
            var usuarios_domain = new List<Domain.ValueObjects.Usuario>();
            foreach(var item in usuarios)
            {
                usuarios_domain.Add(new Domain.ValueObjects.Usuario(item.Id_Usuario,item.Id_Rol,item.Nombres,item.Apellidos,item.Email));
            }
            var output = new InspeccionCabecera
            {
                IdCabecera = cabecera.Id_Inspeccion_Cabecera,
                Codigo = cabecera.Codigo,
                Objetivo = cabecera.Objetivo,
                Gerencia = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(cabecera.Id_Gerencia)).Descripcion,
                GerenciaInspeccionada = gerencias.FirstOrDefault(x => cabecera.Id_Gerencia_Inspeccionada.HasValue && x.Id_Gerencia.Equals(cabecera.Id_Gerencia_Inspeccionada))?.Descripcion,
                Empresa = empresas.FirstOrDefault(x => x.Id_Empresa.Equals(cabecera.Id_Empresa)).Razon_Social,
                AreaInspeccionada = cabecera.Area_Inspeccionada,
                ResponsableArea = usuarios_domain.FirstOrDefault(x => x.IdUsuario.Equals(cabecera.Id_Responsable_Area)),
                FechaInspeccion = cabecera.Fecha_Inspeccion,
                NObservaciones = observaciones.Count,
                Participantes = cabecera.Participantes,
                Estado = cabecera.Estado,
                TipoInspeccion = cabecera.Tipo_Inspeccion,
                Codigo_Formato = cabecera.Codigo_Formato,
                Area_Formato = cabecera.Area_Formato,
                Id_Miembro_Comite = cabecera.Id_Miembro_Comite,
                Participantes_Inspección_Detalle = new List<Usuario>()
            };
            if(cabecera.Id_Miembro_Comite != null)
            {
                output.Miembro_Comite = miembros_comite.Select(x => new { x.Id_Miembro_Comite, NombresyApellidos = x.Nombres + " " + x.Apellidos }).FirstOrDefault(x => x.Id_Miembro_Comite.Equals(cabecera.Id_Miembro_Comite)).NombresyApellidos;
            }
            if (!IsValidJson(cabecera.Responsable_Inspeccion))
                output.ResponsableInspeccion = cabecera.Responsable_Inspeccion;
            else
            {
                var lista_ids = JsonConvert.DeserializeObject<List<int>>(cabecera.Responsable_Inspeccion);
                var lista_usuario = usuarios.Where(x => lista_ids.Contains(x.Id_Usuario)).Select(x => x.Nombres.ToUpper() + " " + x.Apellidos.ToUpper()).ToList();
                output.ResponsableInspeccion = ConcatUsingComa(lista_usuario);
            }
            if (IsValidJson(cabecera.Participantes_Inspección))
            {
                var lista_ids = JsonConvert.DeserializeObject<List<int>>(cabecera.Participantes_Inspección);
                var lista_usuario = usuarios.Where(x => lista_ids.Contains(x.Id_Usuario)).ToList();
                foreach (var x in lista_usuario)
                {
                    output.Participantes_Inspección_Detalle.Add(new Usuario
                    {
                        IdUsuario = x.Id_Usuario,
                        Nombres = x.Nombres,
                        Apellidos = x.Apellidos
                    });
                    output.Participantes_Inspección_Texto += x.Nombres + " " + x.Apellidos + ", ";
                }
                output.Participantes_Inspección_Texto = output.Participantes_Inspección_Texto.Substring(0, output.Participantes_Inspección_Texto.Length - 2);
            }
            return output;
        }
        public async Task<List<InspeccionCabecera>> GetInspeccionCabeceraVerificacion(DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] empresaFiltro, int[] registradoPorFiltro, int[] responsableFiltro, int[] asesorFiltro, string codigo, string[] tipo_inspeccion_filtro, string[] tipo_observacion_filtro)
        {
            var obsPendVer = await _context.Inspecciones_Detalle
               .Where(x => x.Estado.Equals(EstadosInspeccion.Implementado.ToString()))
               .ToListAsync();

            var cabeceras = await _context.Inspecciones_Cabecera.ToListAsync();
            cabeceras = cabeceras.Where(c =>
                c.Codigo.Contains(codigo ?? c.Codigo) &&
                c.Fecha_Inspeccion >= fechaInicio &&
                c.Fecha_Inspeccion <= fechaFin &&
                c.Estado.Equals(EstadosInspeccion.Registrado.ToString()))
                .OrderByDescending(x => x.Codigo)
                .ToList();
            if (tipo_inspeccion_filtro.Length > 0)
                cabeceras = cabeceras.Where(x => x.Tipo_Inspeccion != null && tipo_inspeccion_filtro.Contains(x.Tipo_Inspeccion)).ToList();

            if (gerenciaFiltro.Length > 0)
                cabeceras = cabeceras.Where(x => x.Id_Gerencia_Inspeccionada.HasValue && gerenciaFiltro.Contains(x.Id_Gerencia_Inspeccionada.Value)).ToList();

            if (empresaFiltro.Length > 0)
                cabeceras = cabeceras.Where(x => empresaFiltro.Contains(x.Id_Empresa)).ToList();

            if (registradoPorFiltro.Length > 0)
                cabeceras = cabeceras.Where(x => registradoPorFiltro.Contains(x.Id_Registrado_Por)).ToList();

            if (responsableFiltro.Length > 0 || asesorFiltro.Length > 0)
            {
                if (responsableFiltro.Length > 0)
                    obsPendVer = obsPendVer.Where(x => responsableFiltro.Contains(x.Id_Responsable_Marcobre)).ToList();
                if (asesorFiltro.Length > 0)
                    obsPendVer = obsPendVer.Where(x => asesorFiltro.Contains(x.Id_Asesor)).ToList();
            }

            if (tipo_observacion_filtro.Length > 0)
                obsPendVer = obsPendVer.Where(x => tipo_observacion_filtro.Contains(x.Tipo_Observacion)).ToList();


            cabeceras = cabeceras.Where(x => obsPendVer.GroupBy(y => y.Id_Inspeccion_Cabecera).Select(z => z.Key).Contains(x.Id_Inspeccion_Cabecera)).ToList();

            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var detalles_cabecera = await _context.Inspecciones_Detalle.Where(e => cabeceras.Select(c => c.Id_Inspeccion_Cabecera).Contains(e.Id_Inspeccion_Cabecera) && e.Estado != EstadosInspeccion.Eliminado.ToString()).ToListAsync();

            var TablaResumen = new List<InspeccionCabecera>();
            foreach (var item in cabeceras)
            {
                var gerEjectMar = gerencias.FirstOrDefault(g => g.Id_Gerencia.Equals(item.Id_Gerencia));
                var gerInspec = gerencias.FirstOrDefault(g => item.Id_Gerencia_Inspeccionada.HasValue && g.Id_Gerencia.Equals(item.Id_Gerencia_Inspeccionada.Value));
                var empContMar = empresas.FirstOrDefault(e => e.Id_Empresa.Equals(item.Id_Empresa));
                var nObservaciones = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera)).Count();

                TablaResumen.Add(new InspeccionCabecera(item.Id_Inspeccion_Cabecera, item.Codigo, gerEjectMar.Descripcion,
                    empContMar.Razon_Social, item.Area_Inspeccionada, item.Fecha_Inspeccion, nObservaciones, item.Estado, gerInspec?.Descripcion));
            }

            return TablaResumen;
        }
        public async Task<InspeccionCabecera> GetInspeccionCabeceraVerificacion(int idCabecera)
        {
            var cabecera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera) && x.Estado != EstadosInspeccion.Eliminado.ToString()).ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();
            var responsable_area = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(cabecera.Id_Responsable_Area));
            var miembros_comite = await _context.MiembrosComiteInspeccion.ToListAsync();

            var responsable_area_domain = new Domain.ValueObjects.Usuario(responsable_area.Id_Usuario, responsable_area.Id_Rol, responsable_area.Nombres, responsable_area.Apellidos, responsable_area.Email);

            var output = new InspeccionCabecera
            {
                IdCabecera = cabecera.Id_Inspeccion_Cabecera,
                Codigo = cabecera.Codigo,
                Objetivo = cabecera.Objetivo,
                Gerencia = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(cabecera.Id_Gerencia)).Descripcion,
                GerenciaInspeccionada = gerencias.FirstOrDefault(x => cabecera.Id_Gerencia_Inspeccionada.HasValue && x.Id_Gerencia.Equals(cabecera.Id_Gerencia_Inspeccionada))?.Descripcion,
                Empresa = empresas.FirstOrDefault(x => x.Id_Empresa.Equals(cabecera.Id_Empresa)).Razon_Social,
                AreaInspeccionada = cabecera.Area_Inspeccionada,
                ResponsableArea = responsable_area_domain,
                FechaInspeccion = cabecera.Fecha_Inspeccion,
                NObservaciones = observaciones.Count,
                Participantes = cabecera.Participantes,
                Estado = cabecera.Estado,
                TipoInspeccion = cabecera.Tipo_Inspeccion,
                Codigo_Formato = cabecera.Codigo_Formato,
                Area_Formato = cabecera.Area_Formato,
                Id_Miembro_Comite = cabecera.Id_Miembro_Comite,
                Participantes_Inspección_Detalle = new List<Usuario>()
            }; if (cabecera.Id_Miembro_Comite != null)
            {
                output.Miembro_Comite = miembros_comite.Select(x => new { x.Id_Miembro_Comite, NombresyApellidos = x.Nombres + " " + x.Apellidos }).FirstOrDefault(x => x.Id_Miembro_Comite.Equals(cabecera.Id_Miembro_Comite)).NombresyApellidos;
            }
            if (!IsValidJson(cabecera.Responsable_Inspeccion))
                output.ResponsableInspeccion = cabecera.Responsable_Inspeccion;
            else
            {
                var lista_ids = JsonConvert.DeserializeObject<List<int>>(cabecera.Responsable_Inspeccion);
                var lista_usuario = usuarios.Where(x => lista_ids.Contains(x.Id_Usuario)).Select(x => x.Nombres.ToUpper() + " " + x.Apellidos.ToUpper()).ToList();
                output.ResponsableInspeccion = ConcatUsingComa(lista_usuario);
            }
            if (IsValidJson(cabecera.Participantes_Inspección))
            {
                var lista_ids = JsonConvert.DeserializeObject<List<int>>(cabecera.Participantes_Inspección);
                var lista_usuario = usuarios.Where(x => lista_ids.Contains(x.Id_Usuario)).ToList();
                foreach (var x in lista_usuario)
                {
                    output.Participantes_Inspección_Detalle.Add(new Usuario
                    {
                        IdUsuario = x.Id_Usuario,
                        Nombres = x.Nombres,
                        Apellidos = x.Apellidos
                    });
                    output.Participantes_Inspección_Texto += x.Nombres + " " + x.Apellidos + ", ";
                }
                output.Participantes_Inspección_Texto = output.Participantes_Inspección_Texto.Substring(0, output.Participantes_Inspección_Texto.Length - 2);
            }
            return output;
        }
        public async Task<InspeccionCabecera> GetInspeccionCabeceraDescarga(int idCabecera)
        {
            //Cabecera
            var cabecera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera) && x.Estado != EstadosInspeccion.Eliminado.ToString()).ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();

            var usuarios = await _context.Usuarios.ToListAsync();
            var usuarios_domain = new List<Domain.ValueObjects.Usuario>();
            var miembrosComite = await _context.MiembrosComiteInspeccion.ToListAsync();
            foreach (var item in usuarios)
            {
                usuarios_domain.Add(new Domain.ValueObjects.Usuario(item.Id_Usuario, item.Id_Rol, item.Nombres, item.Apellidos, item.Email));
            }
            //Detalle
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();
            var tiposCondicion_domain = new List<Domain.ValueObjects.TipoCondicion>();
            foreach (var item in tiposCondicion)
            {
                tiposCondicion_domain.Add(new Domain.ValueObjects.TipoCondicion(item.Id_Tipo_Condicion, item.Descripcion,item.Grupo));
            }
            var output = new InspeccionCabecera
            {
                IdCabecera = cabecera.Id_Inspeccion_Cabecera,
                Codigo = cabecera.Codigo,
                Objetivo = cabecera.Objetivo,
                Gerencia = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(cabecera.Id_Gerencia)).Descripcion,
                GerenciaInspeccionada = gerencias.FirstOrDefault(x => cabecera.Id_Gerencia_Inspeccionada.HasValue && x.Id_Gerencia.Equals(cabecera.Id_Gerencia_Inspeccionada.Value))?.Descripcion,
                Empresa = empresas.FirstOrDefault(x => x.Id_Empresa.Equals(cabecera.Id_Empresa)).Razon_Social,
                AreaInspeccionada = cabecera.Area_Inspeccionada,
                ResponsableArea = usuarios_domain.FirstOrDefault(x => x.IdUsuario.Equals(cabecera.Id_Responsable_Area)),
                FechaInspeccion = cabecera.Fecha_Inspeccion,
                Participantes = cabecera.Participantes,
                Estado = cabecera.Estado,
                TipoInspeccion = cabecera.Tipo_Inspeccion,
                Codigo_Formato = cabecera.Codigo_Formato,
                Area_Formato = cabecera.Area_Formato,
                Id_Miembro_Comite = cabecera.Id_Miembro_Comite,
                Miembro_Comite = miembrosComite.Select(x => new { Id = x.Id_Miembro_Comite, Miembro = x.Nombres + " " + x.Apellidos }).FirstOrDefault(x => cabecera.Id_Miembro_Comite.Equals(x.Id))?.Miembro,
                Participantes_Inspección = cabecera.Participantes_Inspección,
                Participantes_Inspección_Detalle = new List<Usuario>()
            };
            if (!IsValidJson(cabecera.Responsable_Inspeccion))
                output.ResponsableInspeccion = cabecera.Responsable_Inspeccion;
            else
            {
                var lista_ids = JsonConvert.DeserializeObject<List<int>>(cabecera.Responsable_Inspeccion);
                var lista_usuario = usuarios.Where(x => lista_ids.Contains(x.Id_Usuario)).Select(x => x.Nombres.ToUpper() + " " + x.Apellidos.ToUpper()).ToList();
                output.ResponsableInspeccion = ConcatUsingComa(lista_usuario);
            }
            if (IsValidJson(cabecera.Participantes_Inspección))
            {
                var lista_ids = JsonConvert.DeserializeObject<List<int>>(cabecera.Participantes_Inspección);
                var lista_usuario = usuarios.Where(x => lista_ids.Contains(x.Id_Usuario)).ToList();
                foreach (var x in lista_usuario)
                {
                    output.Participantes_Inspección_Detalle.Add(new Usuario
                    {
                        IdUsuario = x.Id_Usuario,
                        Nombres = x.Nombres,
                        Apellidos = x.Apellidos
                    });
                }
            }

            output.Observaciones = new List<InspeccionDetalle>();
            foreach(var observacion in observaciones)
            {
                var obs = new InspeccionDetalle
                {
                    IdInspeccionCabecera = idCabecera,
                    IdInspeccionDetalle = observacion.Id_Inspeccion_Detalle,
                    TipoObservacion = observacion.Tipo_Observacion,
                    DescripcionCondicion = observacion.Descripcion_Condicion,
                    DescripcionActoCondicion = observacion.Descripcion_Acto_Condicion,
                    TipoCondicion = tiposCondicion_domain.FirstOrDefault(x => x.IdTipoCondicion.Equals(observacion.Id_Tipo_Condicion)),
                    NivelRiesgo = observacion.Nivel_Riesgo,
                    DescripcionAccion = observacion.Descripcion_Accion,
                    ResponsableMarcobre = usuarios_domain.FirstOrDefault(x => x.IdUsuario.Equals(observacion.Id_Responsable_Marcobre)),
                    ResponsableMarcobre2 = usuarios_domain.FirstOrDefault(x => x.IdUsuario.Equals(observacion.Id_Responsable_Marcobre2)),
                    ResponsableContratista = observacion.Responsable_Contratista,
                    CorreoContratista = observacion.Correo_Contratista,
                    FechaProgramada = observacion.Fecha_Programada,
                    ComentarioImplementacion = observacion.Comentario_Implementacion,
                    FechaCierre = observacion.Fecha_Cierre,
                    ComentarioVerificacion = observacion.Comentario_Verificacion,
                    FechaVerificacion = observacion.Fecha_Verificacion,
                    Estado = observacion.Estado,
                    EvidenciaImplementacion = observacion.Evidencia_Implementacion,
                    EvidenciaCondicion = observacion.Evidencia_Condicion
                };
                output.Observaciones.Add(obs);
            }
            return output;
        }
        public async Task EliminarVencidos(DateTime fechaInicio, DateTime fechaFin)
        {
            //Eliminar obs vencidas
            var observacionesVencidas = await _context.Inspecciones_Detalle.ToListAsync();
            observacionesVencidas = observacionesVencidas.Where(x => x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();

            observacionesVencidas = observacionesVencidas.Where(x=>x.Fecha_Programada.Value >= fechaInicio && x.Fecha_Programada.Value<=fechaFin).Select(x => { x.Estado = EstadosInspeccion.Eliminado.ToString();return x; }).ToList();

            _context.Inspecciones_Detalle.UpdateRange(observacionesVencidas);
            await _context.SaveChangesAsync();
            //Listar observaciones eliminadas
            var observacionesEliminadas = await _context.Inspecciones_Detalle.ToListAsync();
            observacionesEliminadas = observacionesEliminadas.Where(x => x.Estado.Equals(EstadosInspeccion.Eliminado.ToString())).ToList();
            //Listar observaciones no eliminadas
            var observacionesNoEliminadas = await _context.Inspecciones_Detalle.Where(x => !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString())).ToListAsync();
            //Buscar las inspecciones con observaciones eliminadas
            var inspeccionesConObsEliminadas = await _context.Inspecciones_Cabecera.ToListAsync();
            inspeccionesConObsEliminadas = inspeccionesConObsEliminadas.Where(x => observacionesEliminadas.Select(x => x.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera)).ToList();
            //Eliminar las inspecciones con 0 observaciones no eliminadas
            var inspeccionesEliminables = new List<Entities.Inspecciones.Inspeccion_Cabecera>();
            foreach(var item in inspeccionesConObsEliminadas)
            {
                if (observacionesEliminadas.Select(x => x.Id_Inspeccion_Cabecera).Contains(item.Id_Inspeccion_Cabecera) && !observacionesNoEliminadas.Select(x => x.Id_Inspeccion_Cabecera).Contains(item.Id_Inspeccion_Cabecera))
                {
                    inspeccionesEliminables.Add(item);
                }
            }

            inspeccionesEliminables = inspeccionesEliminables.Select(x => { x.Estado = EstadosInspeccion.Eliminado.ToString(); x.Codigo = "TBD"; return x; }).ToList();
            var noTBD = inspeccionesEliminables.Where(x => !x.Codigo.Equals("TBD")).ToList();

            _context.Inspecciones_Cabecera.UpdateRange(inspeccionesEliminables);
            await _context.SaveChangesAsync();

            //Reasignar codigos
            var inspeccionesSinVencidos = await _context.Inspecciones_Cabecera.Where(x=>!x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) && !x.Codigo.Equals("TBD")).ToListAsync();

            var codigos = new List<InspeccionCodigos>();
            foreach(var inspeccion in inspeccionesSinVencidos)
            {
                codigos.Add(new InspeccionCodigos
                {
                    IdInspeccion = inspeccion.Id_Inspeccion_Cabecera,
                    CodigoActual = inspeccion.Codigo
                });
            }

            codigos = codigos.OrderBy(x => x.CodigoActual).ToList();

            for(int i = 0; i < codigos.Count; i++)
            {
                codigos[i].CodigoNuevo = "IP-" + (i + 1).ToString().PadLeft(5, '0'); ;
            }

            foreach(var inspeccion in inspeccionesSinVencidos)
            {
                var codigoNuevo = codigos.FirstOrDefault(x => x.IdInspeccion.Equals(inspeccion.Id_Inspeccion_Cabecera));
                if (codigoNuevo != null)
                    inspeccion.Codigo = codigoNuevo.CodigoNuevo;
            }
            _context.Inspecciones_Cabecera.UpdateRange(inspeccionesSinVencidos);
            await _context.SaveChangesAsync();
        }
        private static bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private static string ConcatUsingComa(List<string> lista)
        {
            string nuevo_texto = "";

            for (int i = 0; i < lista.Count; i++)
            {
                nuevo_texto += lista[i]+", ";
            }

            nuevo_texto = nuevo_texto.Remove(nuevo_texto.Length - 2);
            return nuevo_texto;
        }
        private class InspeccionCodigos
        {
            public int IdInspeccion { get; set; }
            public string CodigoActual { get; set; }
            public string CodigoNuevo { get; set; }

        }
    }
}
