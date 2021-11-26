using GestionAmbiental.Application.Repositories.Notificaciones;
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

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Notificaciones
{
    public class GetInspeccionInfoRepository : IGetInspeccionInfoReadOnlyRepository
    {
        private Context _context;

        public GetInspeccionInfoRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<InspeccionCabecera> GetInfoCompletaInspeccion(int idCabecera)
        {
            var cabecera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();

            var gerEjectMar = gerencias.FirstOrDefault(g => g.Id_Gerencia.Equals(cabecera.Id_Gerencia)).Descripcion;
            var gerInspec = gerencias.FirstOrDefault(g => cabecera.Id_Gerencia_Inspeccionada.HasValue && g.Id_Gerencia.Equals(cabecera.Id_Gerencia_Inspeccionada))?.Descripcion;
            var empContMar = empresas.FirstOrDefault(e => e.Id_Empresa.Equals(cabecera.Id_Empresa)).Razon_Social;
           
            var responsableLugar = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(cabecera.Id_Responsable_Area));
            var responsableLugarDomain = new Usuario(responsableLugar.Id_Usuario, responsableLugar.Id_Rol, responsableLugar.Nombres, responsableLugar.Apellidos, responsableLugar.Email);
            
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera)&&x.Estado!=EstadosInspeccion.Eliminado.ToString()).ToListAsync();
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();

            var output = new InspeccionCabecera();
            output.IdCabecera = cabecera.Id_Inspeccion_Cabecera;
            output.Codigo = cabecera.Codigo;
            output.Objetivo = cabecera.Objetivo;
            output.Gerencia = gerEjectMar;
            output.GerenciaInspeccionada = gerInspec;
            output.Empresa = empContMar;
            output.AreaInspeccionada = cabecera.Area_Inspeccionada;
            output.TipoInspeccion = cabecera.Tipo_Inspeccion;
            output.ResponsableArea = responsableLugarDomain;
            if (!IsValidJson(cabecera.Responsable_Inspeccion))
                output.ResponsableInspeccion = cabecera.Responsable_Inspeccion;
            else
            {
                var lista_ids = JsonConvert.DeserializeObject<List<int>>(cabecera.Responsable_Inspeccion);
                var lista_usuario = usuarios.Where(x => lista_ids.Contains(x.Id_Usuario)).Select(x => x.Nombres.ToUpper() + " " + x.Apellidos.ToUpper()).ToList();
                output.ResponsableInspeccion = ConcatUsingComa(lista_usuario);
            }
            output.Participantes = cabecera.Participantes;
            output.FechaInspeccion = cabecera.Fecha_Inspeccion;
            output.Estado = cabecera.Estado;

            output.Observaciones = new List<InspeccionDetalle>();

            foreach (var observacion in observaciones)
            {
                var tipoCondicion = tiposCondicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(observacion.Id_Tipo_Condicion));
                var responsableMarcobre = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(observacion.Id_Responsable_Marcobre));
                var responsableMarcobre2 = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(observacion.Id_Responsable_Marcobre2));
                var responsableMarcobre_Domain = new Usuario(responsableMarcobre.Id_Usuario, responsableMarcobre.Id_Rol, responsableMarcobre.Nombres, responsableMarcobre.Apellidos, responsableMarcobre.Email);
                var responsableMA = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(observacion.Id_Asesor));
                var responsableMA_Domain = new Usuario(responsableMA.Id_Usuario, responsableMA.Id_Rol, responsableMA.Nombres, responsableMA.Apellidos, responsableMA.Email);

                var detalle = new InspeccionDetalle();
                detalle.IdInspeccionCabecera = observacion.Id_Inspeccion_Cabecera;
                detalle.IdInspeccionDetalle = observacion.Id_Inspeccion_Detalle;
                detalle.TipoObservacion = observacion.Tipo_Observacion;
                detalle.DescripcionCondicion = observacion.Descripcion_Condicion;
                detalle.DescripcionActoCondicion = observacion.Descripcion_Acto_Condicion;
                detalle.EvidenciaCondicion = observacion.Evidencia_Condicion;
                detalle.NivelRiesgo = observacion.Nivel_Riesgo;
                detalle.DescripcionAccion = observacion.Descripcion_Accion;
                detalle.ResponsableMarcobre = responsableMarcobre_Domain;
                detalle.Asesor = responsableMA_Domain;
                detalle.ResponsableContratista = observacion.Responsable_Contratista;
                detalle.FechaProgramada = observacion.Fecha_Programada;
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

                output.Observaciones.Add(detalle);
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
