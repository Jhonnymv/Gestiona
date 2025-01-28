using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Inspecciones
{
    public class DetalleInspeccionRepository : IDetalleInspeccionReadOnlyRepository
    {
        private readonly Context _context;

        public DetalleInspeccionRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<InspeccionDetalle>> GetInspeccionDetalles(int idCabecera)
        {
            var detalles = new List<InspeccionDetalle>();
            
            var inspeccionesDetalle = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera) && x.Estado!=EstadosInspeccion.Eliminado.ToString()).ToListAsync();
            
            var usuarios = await _context.Usuarios.ToListAsync();
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();
            foreach (var item in inspeccionesDetalle)
            {
                var responsableMarc = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Responsable_Marcobre));
                var responsableMarc2 = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Responsable_Marcobre2));

                var responsableMA = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Asesor));
                var tipoCondicion = tiposCondicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Id_Tipo_Condicion));
                var detalle = new InspeccionDetalle
                {
                    IdInspeccionCabecera = item.Id_Inspeccion_Cabecera,
                    IdInspeccionDetalle = item.Id_Inspeccion_Detalle,
                    TipoObservacion = item.Tipo_Observacion,
                    DescripcionCondicion = item.Descripcion_Condicion,
                    EvidenciaCondicion = item.Evidencia_Condicion,
                    NivelRiesgo = item.Nivel_Riesgo,
                    DescripcionAccion = item.Descripcion_Accion,
                    ResponsableMarcobre = new Domain.ValueObjects.Usuario(responsableMarc.Id_Usuario, responsableMarc.Id_Rol, responsableMarc.Nombres, responsableMarc.Apellidos, responsableMarc.Email),
                    ResponsableContratista = item.Responsable_Contratista,
                    CorreoContratista = item.Correo_Contratista,
                    FechaProgramada = item.Fecha_Programada,
                    EvidenciaImplementacion = item.Evidencia_Implementacion,
                    ComentarioImplementacion = item.Comentario_Implementacion,
                    FechaCierre = item.Fecha_Cierre,
                    Asesor = new Domain.ValueObjects.Usuario(responsableMA.Id_Usuario, responsableMA.Id_Rol, responsableMA.Nombres, responsableMA.Apellidos, responsableMA.Email),
                    FechaVerificacion = item.Fecha_Verificacion,
                    ComentarioVerificacion = item.Comentario_Verificacion,
                    Estado = item.Estado,
                    DescripcionActoCondicion = item.Descripcion_Acto_Condicion
                };
                if (responsableMarc2 != null)
                {
                    detalle.ResponsableMarcobre2 = new Domain.ValueObjects.Usuario(responsableMarc2.Id_Usuario, responsableMarc2.Id_Rol, responsableMarc2.Nombres, responsableMarc2.Apellidos, responsableMarc2.Email);
                }
                if (tipoCondicion != null)
                    detalle.TipoCondicion = new TipoCondicion(tipoCondicion.Id_Tipo_Condicion, tipoCondicion.Descripcion, tipoCondicion.Grupo);

                detalles.Add(detalle);
            }
            
            return detalles;
        }
        public async Task<InspeccionDetalle> GetInspeccionDetalle(int idDetalle)
        {
            var inspeccionDetalle = await _context.Inspecciones_Detalle.FirstOrDefaultAsync(x => x.Id_Inspeccion_Detalle.Equals(idDetalle));
            var tipoCondicion = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(inspeccionDetalle.Id_Tipo_Condicion));
            var responsableMarcobre = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Responsable_Marcobre));
            var responsableMarcobre_Domain = new Usuario(responsableMarcobre.Id_Usuario,responsableMarcobre.Id_Rol,responsableMarcobre.Nombres,responsableMarcobre.Apellidos,responsableMarcobre.Email);
            var responsableMarcobre2 = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Responsable_Marcobre2));
            var responsableMA = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Asesor));
            var responsableMA_Domain = new Usuario(responsableMA.Id_Usuario, responsableMA.Id_Rol, responsableMA.Nombres, responsableMA.Apellidos, responsableMA.Email);

            var detalle = new InspeccionDetalle {
                IdInspeccionCabecera = inspeccionDetalle.Id_Inspeccion_Cabecera,
                IdInspeccionDetalle = inspeccionDetalle.Id_Inspeccion_Detalle,
                TipoObservacion = inspeccionDetalle.Tipo_Observacion,
                DescripcionActoCondicion = inspeccionDetalle.Descripcion_Acto_Condicion,
                DescripcionCondicion = inspeccionDetalle.Descripcion_Condicion,
                EvidenciaCondicion = inspeccionDetalle.Evidencia_Condicion,
                NivelRiesgo = inspeccionDetalle.Nivel_Riesgo,
                DescripcionAccion = inspeccionDetalle.Descripcion_Accion,
                ResponsableMarcobre = responsableMarcobre_Domain,
                ResponsableContratista = inspeccionDetalle.Responsable_Contratista,
                CorreoContratista = inspeccionDetalle.Correo_Contratista,
                FechaProgramada = inspeccionDetalle.Fecha_Programada,
                EvidenciaImplementacion = inspeccionDetalle.Evidencia_Implementacion,
                ComentarioImplementacion = inspeccionDetalle.Comentario_Implementacion,
                FechaCierre = inspeccionDetalle.Fecha_Cierre,
                Asesor = responsableMA_Domain,
                FechaVerificacion = inspeccionDetalle.Fecha_Verificacion,
                ComentarioVerificacion = inspeccionDetalle.Comentario_Verificacion,
                Estado = inspeccionDetalle.Estado
                };
            if (tipoCondicion != null)
            {
                var tipoCondicion_Domain = new TipoCondicion(tipoCondicion.Id_Tipo_Condicion, tipoCondicion.Descripcion, tipoCondicion.Grupo);
                detalle.TipoCondicion = tipoCondicion_Domain;
            }
            if (responsableMarcobre2 != null)
            {
                var responsableMarcobre2_Domain = new Usuario(responsableMarcobre2.Id_Usuario, responsableMarcobre2.Id_Rol, responsableMarcobre2.Nombres, responsableMarcobre2.Apellidos, responsableMarcobre2.Email);
                detalle.ResponsableMarcobre2 = responsableMarcobre2_Domain;
            }
            return detalle;
        }
        public async Task<List<InspeccionCabecera>> GetInspeccionesYObservaciones(int empresa, int gerencia, int respLevObs, int asesor, DateTime FechaInicio, DateTime FechaFin, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion)
        {
            var output = new List<InspeccionCabecera>();
            var usuarios = await _context.Usuarios.ToListAsync();
            var cabeceras = await _context.Inspecciones_Cabecera.ToListAsync();
            cabeceras = cabeceras.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString()) &&
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) 
            ).ToList();

            cabeceras = cabeceras.Where(x =>
                x.Fecha_Inspeccion >= FechaInicio &&
                x.Fecha_Inspeccion <= FechaFin
            ).ToList();

            cabeceras = cabeceras.Where(x =>
                empresa != 0 ? x.Id_Empresa.Equals(empresa) : x.Id_Empresa.Equals(x.Id_Empresa) &&
                gerencia != 0 ? x.Id_Gerencia.Equals(gerencia) : x.Id_Gerencia.Equals(x.Id_Gerencia)
            ).ToList();

            if (idGerenciaInspeccionada != 0)
                cabeceras = cabeceras.Where(x => x.Id_Gerencia_Inspeccionada.Equals(idGerenciaInspeccionada)).ToList();

            if (usuarioInvolucrado != 0)
                cabeceras = cabeceras.Where(x =>
                (x.Responsable_Inspeccion != null && x.Responsable_Inspeccion.Contains('\"' + usuarioInvolucrado.ToString() + '\"')) ||
                (x.Participantes_Inspección != null && x.Participantes_Inspección.Contains('\"' + usuarioInvolucrado.ToString() + '\"'))).ToList();

            var observaciones = await _context.Inspecciones_Detalle.ToListAsync();
            if (tipoObservacion != null)
            {
                observaciones = observaciones.Where(x => x.Tipo_Observacion.Equals(tipoObservacion)).ToList();
            }
            observaciones = observaciones.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                cabeceras.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera)).ToList();

            observaciones = observaciones.Where(x =>
                (respLevObs != 0 ? x.Id_Responsable_Marcobre.Equals(respLevObs) : x.Id_Responsable_Marcobre.Equals(x.Id_Responsable_Marcobre) ||
                respLevObs != 0 ? x.Id_Responsable_Marcobre2.Equals(respLevObs) : x.Id_Responsable_Marcobre2.Equals(x.Id_Responsable_Marcobre2)) && 
                asesor != 0 ? x.Id_Asesor.Equals(asesor) : x.Id_Asesor.Equals(x.Id_Asesor)
            ).ToList();


            if (estado != "")
            {
                
                /*"Pendiente de Implementación", "Pendiente"
                  "Pendiente de Verificación", "Verificacion"
                  "Vencido", "Vencido"
                  "Aprobado", "Aprobado"*/
                switch (estado)
                {
                    case "Pendiente": observaciones = observaciones.Where(x => 
                    x.Estado.Equals(EstadosInspeccion.Registrado.ToString()) || 
                    x.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) || 
                    x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                    case "Verificacion": observaciones = observaciones.Where(x => 
                    x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).ToList();
                        break;
                    case "Vencido": observaciones = observaciones.Where(x => 
                    x.Estado.Equals(EstadosInspeccion.Vencido.ToString())).ToList();
                        break;
                    case "Aprobado": observaciones = observaciones.Where(x => 
                    x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).ToList();
                        break;
                    case "Rechazado": observaciones = observaciones.Where(x =>
                    x.Descripcion_Condicion.Equals("Buenas Prácticas") &&
                    x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).ToList();
                        break;
                }
            }
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();

            var empresas = await _context.Empresas.ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var miembrosComite = await _context.MiembrosComiteInspeccion.ToListAsync();
            foreach (var cabecera in cabeceras)
            {
                var responsableArea = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(cabecera.Id_Responsable_Area));
                var cabDomain = new InspeccionCabecera
                {
                    IdCabecera = cabecera.Id_Inspeccion_Cabecera,
                    AreaInspeccionada = cabecera.Area_Inspeccionada,
                    Codigo = cabecera.Codigo,
                    Empresa = empresas.FirstOrDefault(x => x.Id_Empresa.Equals(cabecera.Id_Empresa)).Razon_Social,
                    FechaInspeccion = cabecera.Fecha_Inspeccion,
                    Gerencia = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(cabecera.Id_Gerencia)).Descripcion,
                    Estado = cabecera.Estado,
                    IdEmpresa = empresas.FirstOrDefault(x => x.Id_Empresa.Equals(cabecera.Id_Empresa)).Id_Empresa,
                    IdGerencia = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(cabecera.Id_Gerencia)).Id_Gerencia,
                    IdGerenciaInspeccionada = gerencias.FirstOrDefault(x => cabecera.Id_Gerencia_Inspeccionada.HasValue && x.Id_Gerencia.Equals(cabecera.Id_Gerencia_Inspeccionada.Value))?.Id_Gerencia,
                    GerenciaInspeccionada = gerencias.FirstOrDefault(x => cabecera.Id_Gerencia_Inspeccionada.HasValue && x.Id_Gerencia.Equals(cabecera.Id_Gerencia_Inspeccionada.Value))?.Descripcion,
                    Objetivo = cabecera.Objetivo,
                    Participantes = cabecera.Participantes,
                    ResponsableArea = new Usuario
                    {
                        IdUsuario = responsableArea.Id_Usuario,
                        Nombres = responsableArea.Nombres,
                        Apellidos = responsableArea.Apellidos
                    },
                    TipoInspeccion = cabecera.Tipo_Inspeccion,
                    Codigo_Formato = cabecera.Codigo_Formato,
                    Area_Formato = cabecera.Area_Formato,
                    Id_Miembro_Comite = cabecera.Id_Miembro_Comite,
                    Miembro_Comite = miembrosComite.Select(x => new { Id = x.Id_Miembro_Comite, Miembro = x.Nombres + " " + x.Apellidos }).FirstOrDefault(x => cabecera.Id_Miembro_Comite.Equals(x.Id))?.Miembro,
                    Participantes_Inspección_Detalle = new List<Usuario>()
                };

                if (!IsValidJson(cabecera.Responsable_Inspeccion))
                    cabDomain.ResponsableInspeccion = cabecera.Responsable_Inspeccion;
                else
                {
                    var lista_ids = JsonConvert.DeserializeObject<List<int>>(cabecera.Responsable_Inspeccion);
                    var lista_usuario = usuarios.Where(x => lista_ids.Contains(x.Id_Usuario)).Select(x => x.Nombres.ToUpper() + " " + x.Apellidos.ToUpper()).ToList();
                    cabDomain.ResponsableInspeccion = ConcatUsingComa(lista_usuario);
                }
                if (IsValidJson(cabecera.Participantes_Inspección)) 
                { 
                    var lista_ids = JsonConvert.DeserializeObject<List<int>>(cabecera.Participantes_Inspección);
                    var lista_usuario = usuarios.Where(x => lista_ids.Contains(x.Id_Usuario)).ToList();
                    foreach( var x in lista_usuario)
                    {
                        cabDomain.Participantes_Inspección_Detalle.Add(new Usuario
                        {
                            IdUsuario = x.Id_Usuario,
                            Nombres = x.Nombres,
                            Apellidos = x.Apellidos
                        });
                    }
                    cabDomain.Participantes_Inspección_Texto = ConcatUsingComa(lista_usuario.Select(x => x.Nombres.ToUpper() + " " + x.Apellidos.ToUpper()).ToList());
                }

                var observacionesCab = observaciones.Where(x => x.Id_Inspeccion_Cabecera.Equals(cabecera.Id_Inspeccion_Cabecera)).ToList();
                var obsDomain = new List<InspeccionDetalle>();
                foreach (var item in observacionesCab)
                {
                    var tipoCondicion = tiposCondicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Id_Tipo_Condicion));
                    var responsableMarcobre = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Responsable_Marcobre));
                    var responsableMarcobre_Domain = new Usuario(responsableMarcobre.Id_Usuario, responsableMarcobre.Id_Rol, responsableMarcobre.Nombres, responsableMarcobre.Apellidos, responsableMarcobre.Email);
                    var responsableMarcobre2 = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Responsable_Marcobre2));
                    var responsableMA = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Asesor));
                    var responsableMA_Domain = new Usuario(responsableMA.Id_Usuario, responsableMA.Id_Rol, responsableMA.Nombres, responsableMA.Apellidos, responsableMA.Email);

                    var obs = new InspeccionDetalle
                    {
                        IdInspeccionCabecera = item.Id_Inspeccion_Cabecera,
                        IdInspeccionDetalle = item.Id_Inspeccion_Detalle,
                        TipoObservacion = item.Tipo_Observacion,
                        DescripcionActoCondicion = item.Descripcion_Acto_Condicion,
                        DescripcionCondicion = item.Descripcion_Condicion,
                        EvidenciaCondicion = item.Evidencia_Condicion,
                        NivelRiesgo = item.Nivel_Riesgo,
                        DescripcionAccion = item.Descripcion_Accion,
                        ResponsableMarcobre = responsableMarcobre_Domain,                        
                        ResponsableContratista = item.Responsable_Contratista,
                        CorreoContratista = item.Correo_Contratista,
                        FechaProgramada = item.Fecha_Programada,
                        EvidenciaImplementacion = item.Evidencia_Implementacion,
                        ComentarioImplementacion = item.Comentario_Implementacion,
                        FechaCierre = item.Fecha_Cierre,
                        Asesor = responsableMA_Domain,
                        FechaVerificacion = item.Fecha_Verificacion,
                        ComentarioVerificacion = item.Comentario_Verificacion,
                        Estado = item.Estado
                    };

                    if (tipoCondicion != null)
                    {
                        var tipoCondicion_Domain = new TipoCondicion(tipoCondicion.Id_Tipo_Condicion, tipoCondicion.Descripcion, tipoCondicion.Grupo);
                        obs.TipoCondicion = tipoCondicion_Domain;
                    }
                    if (responsableMarcobre2 != null)
                    {
                        var responsableMarcobre2_Domain = new Usuario(responsableMarcobre2.Id_Usuario, responsableMarcobre2.Id_Rol, responsableMarcobre2.Nombres, responsableMarcobre2.Apellidos, responsableMarcobre2.Email);
                        obs.ResponsableMarcobre2 = responsableMarcobre2_Domain;
                    }
                    obsDomain.Add(obs);
                }
                cabDomain.Observaciones = obsDomain;
                output.Add(cabDomain);
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
                nuevo_texto += lista[i] + ", ";
            }

            nuevo_texto = nuevo_texto.Remove(nuevo_texto.Length - 2);
            return nuevo_texto;
        }
    }
}
