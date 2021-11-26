using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Inspecciones
{
    public class CabeceraInspeccionRepository : ICabeceraInspeccionReadOnlyRepository
    {
        private readonly Context _context;

        public CabeceraInspeccionRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<InspeccionCabecera>> GetInspeccionCabecera(DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] empresaFiltro, int[] registradoPorFiltro, List<string> estadoFiltro, int[] responsableFiltro, int[] asesorFiltro, string codigo)
        {
            var cabeceras = await _context.Inspecciones_Cabecera.ToListAsync();
            cabeceras = cabeceras.Where(c =>
                c.Codigo.Contains(codigo ?? c.Codigo) &&
                c.Fecha_Inspeccion >= fechaInicio &&
                c.Fecha_Inspeccion <= fechaFin &&
                (c.Estado.Equals(EstadosInspeccion.Registrado.ToString()) || c.Estado.Equals(EstadosInspeccion.Pendiente.ToString())))
                .OrderByDescending(x=>x.Codigo)
                .ToList();

            if (gerenciaFiltro.Length > 0)                
                cabeceras = cabeceras.Where(x => x.Id_Gerencia_Inspeccionada.HasValue && gerenciaFiltro.Contains(x.Id_Gerencia_Inspeccionada.Value)).ToList();
            
            if (empresaFiltro.Length > 0)            
                cabeceras = cabeceras.Where(x => empresaFiltro.Contains(x.Id_Empresa)).ToList();
            
            if (registradoPorFiltro.Length > 0)
                cabeceras = cabeceras.Where(x => registradoPorFiltro.Contains(x.Id_Registrado_Por)).ToList();

            var observaciones = await _context.Inspecciones_Detalle.ToListAsync();
            var cabecerasSinObs = cabeceras.Where(x => !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) && !observaciones.GroupBy(y => y.Id_Inspeccion_Cabecera).Select(z => z.Key).Contains(x.Id_Inspeccion_Cabecera)).OrderByDescending(x => x.Fecha_Inspeccion).ToList();
            observaciones = observaciones.Where(x => !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString())).ToList();
            if (responsableFiltro.Length > 0 || asesorFiltro.Length > 0 || estadoFiltro.Count > 0)
            {
                cabecerasSinObs = new List<Entities.Inspecciones.Inspeccion_Cabecera>();
                if (responsableFiltro.Length > 0)
                    observaciones = observaciones.Where(x => responsableFiltro.Contains(x.Id_Responsable_Marcobre)).ToList();
                if (asesorFiltro.Length > 0)
                    observaciones = observaciones.Where(x => asesorFiltro.Contains(x.Id_Asesor)).ToList();
                if (estadoFiltro.Count > 0)
                    observaciones = observaciones.Where(x => estadoFiltro.Contains(x.Estado)).ToList();
            }
            cabeceras = cabeceras.Where(x => observaciones.GroupBy(y => y.Id_Inspeccion_Cabecera).Select(z => z.Key).Contains(x.Id_Inspeccion_Cabecera)).ToList(); 
            cabeceras.InsertRange(0, cabecerasSinObs);
            //else
            //{
            //    var cabecerasSinObs = cabeceras.Where(x => !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) && !observaciones.GroupBy(y => y.Id_Inspeccion_Cabecera).Select(z => z.Key).Contains(x.Id_Inspeccion_Cabecera)).OrderByDescending(x => x.Fecha_Inspeccion).ToList(); 
            //    cabeceras.InsertRange(0, cabecerasSinObs);
            //}

            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var detalles_cabecera = await _context.Inspecciones_Detalle
                .Where(e => cabeceras.Select(c => c.Id_Inspeccion_Cabecera).Contains(e.Id_Inspeccion_Cabecera)&&
                e.Estado!=EstadosInspeccion.Eliminado.ToString())
                .ToListAsync();

            var TablaResumen = new List<InspeccionCabecera>();
            foreach (var item in cabeceras)
            {
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
            var cabeccera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera)&&x.Estado!=EstadosInspeccion.Eliminado.ToString()).ToListAsync();
            var output = new InspeccionCabecera
            {
                IdCabecera = cabeccera.Id_Inspeccion_Cabecera,
                IdRegistradoPor = cabeccera.Id_Registrado_Por,
                Codigo = cabeccera.Codigo,
                IdGerencia = cabeccera.Id_Gerencia,
                IdGerenciaInspeccionada = cabeccera.Id_Gerencia_Inspeccionada,
                IdEmpresa = cabeccera.Id_Empresa,
                TipoInspeccion = cabeccera.Tipo_Inspeccion,
                AreaInspeccionada = cabeccera.Area_Inspeccionada,
                FechaInspeccion = cabeccera.Fecha_Inspeccion,
                NObservaciones = observaciones.Count,
                Estado = cabeccera.Estado,
                IdResponsableArea = cabeccera.Id_Responsable_Area,
                Objetivo = cabeccera.Objetivo,
                ResponsableInspeccion = cabeccera.Responsable_Inspeccion,
                Participantes = cabeccera.Participantes
            };

            return output;
        }

        public async Task<List<InspeccionCabecera>> GetInspeccionCabeceraImplementacion(DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] empresaFiltro, int[] registradoPorFiltro, int[] responsableFiltro, int[] asesorFiltro, string codigo)
        {
            var obsPendImp = await _context.Inspecciones_Detalle
                 .Where(x =>
                 x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) ||
                 x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) ||
                 x.Estado.Equals(EstadosInspeccion.Rechazado.ToString()))
                 .ToListAsync();
            var cabeceras = await _context.Inspecciones_Cabecera.ToListAsync();
            cabeceras = cabeceras.Where(c =>
                c.Codigo.Contains(codigo ?? c.Codigo) &&
                c.Fecha_Inspeccion >= fechaInicio &&
                c.Fecha_Inspeccion <= fechaFin &&
                c.Estado.Equals(EstadosInspeccion.Registrado.ToString()))
                .OrderByDescending(x => x.Codigo)
                .ToList();

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
                TipoInspeccion = cabecera.Tipo_Inspeccion
            };
            if (!IsValidJson(cabecera.Responsable_Inspeccion))
                output.ResponsableInspeccion = cabecera.Responsable_Inspeccion;
            else
            {
                var lista_ids = JsonConvert.DeserializeObject<List<int>>(cabecera.Responsable_Inspeccion);
                var lista_usuario = usuarios.Where(x => lista_ids.Contains(x.Id_Usuario)).Select(x => x.Nombres.ToUpper() + " " + x.Apellidos.ToUpper()).ToList();
                output.ResponsableInspeccion = ConcatUsingComa(lista_usuario);
            }
            return output;
        }

        public async Task<List<InspeccionCabecera>> GetInspeccionCabeceraVerificacion(DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] empresaFiltro, int[] registradoPorFiltro, int[] responsableFiltro, int[] asesorFiltro, string codigo)
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
                TipoInspeccion = cabecera.Tipo_Inspeccion
            };
            if (!IsValidJson(cabecera.Responsable_Inspeccion))
                output.ResponsableInspeccion = cabecera.Responsable_Inspeccion;
            else
            {
                var lista_ids = JsonConvert.DeserializeObject<List<int>>(cabecera.Responsable_Inspeccion);
                var lista_usuario = usuarios.Where(x => lista_ids.Contains(x.Id_Usuario)).Select(x => x.Nombres.ToUpper() + " " + x.Apellidos.ToUpper()).ToList();
                output.ResponsableInspeccion = ConcatUsingComa(lista_usuario);
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
                TipoInspeccion = cabecera.Tipo_Inspeccion
            };
            if (!IsValidJson(cabecera.Responsable_Inspeccion))
                output.ResponsableInspeccion = cabecera.Responsable_Inspeccion;
            else
            {
                var lista_ids = JsonConvert.DeserializeObject<List<int>>(cabecera.Responsable_Inspeccion);
                var lista_usuario = usuarios.Where(x => lista_ids.Contains(x.Id_Usuario)).Select(x => x.Nombres.ToUpper() + " " + x.Apellidos.ToUpper()).ToList();
                output.ResponsableInspeccion = ConcatUsingComa(lista_usuario);
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
    }
}
