using GestionAmbiental.Application.Repositories.Notificaciones;
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
    public class GetInspeccionCabeceraInfoRepository : IGetInspeccionCabeceraInfoReadOnlyRepository
    {
        private Context _context;

        public GetInspeccionCabeceraInfoRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<InspeccionCabecera> GetInspeccionCabecera(int idCabecera)
        {
            var cabecera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var gerEjectMar = gerencias.FirstOrDefault(g => g.Id_Gerencia.Equals(cabecera.Id_Gerencia)).Descripcion;
            var gerInspec = gerencias.FirstOrDefault(g => cabecera.Id_Gerencia_Inspeccionada.HasValue && g.Id_Gerencia.Equals(cabecera.Id_Gerencia_Inspeccionada))?.Descripcion;
            var empContMar = empresas.FirstOrDefault(e => e.Id_Empresa.Equals(cabecera.Id_Empresa)).Razon_Social;
            var usuarios = await _context.Usuarios.ToListAsync();
            var responsableLugar = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(cabecera.Id_Responsable_Area));
            var responsableLugarDomain = new Usuario(responsableLugar.Id_Usuario, responsableLugar.Id_Rol, responsableLugar.Nombres, responsableLugar.Apellidos, responsableLugar.Email);

            var output = new InspeccionCabecera();
            output.Codigo = cabecera.Codigo;
            output.IdCabecera = cabecera.Id_Inspeccion_Cabecera;
            output.Empresa = empContMar;
            output.Gerencia = gerEjectMar;
            output.GerenciaInspeccionada = gerInspec;
            output.TipoInspeccion = cabecera.Tipo_Inspeccion;
            output.AreaInspeccionada = cabecera.Area_Inspeccionada;
            output.ResponsableArea = responsableLugarDomain;
            output.Participantes = cabecera.Participantes;
            output.FechaInspeccion = cabecera.Fecha_Inspeccion;
            output.Objetivo = cabecera.Objetivo;
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
