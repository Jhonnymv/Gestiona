using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS
{
    public class ManifiestoRSOutput
    {
        public int Id_Manifiesto_RS { get; set; }
        public string Item { get; set; }
        public int Serie_Manifiesto { get; set; }
        public string Num_Manifiesto { get; set; }
        public DateTime Fecha_Emision_Manifiesto { get; set; }
        public string Observacion_Manifiesto { get; set; }
        public int Id_Tipo_Residuo_Solido { get; set; }
        public string Tipo_Residuo_Solido { get; set; }
        public int Id_Resido_Solido { get; set; }
        public string Residuo_Solido { get; set; }
        public double Cantidad_RS_Evacuado { get; set; }
        public int Id_EO_RS_Salida { get; set; }
        public string EO_RS_Salida { get; set; }
        public string Registro_EO_RS_Salida { get; set; }
        public DateTime Fecha_Vencimiento_Registro_Salida { get; set; }
        public string Num_Autorizacion_Municipal_Salida { get; set; }
        public string Num_Autorizacion_Hoja_Ruta { get; set; }
        public int Id_Conductor { get; set; }
        public string Num_Licencia_Condutor { get; set; }
        public int Id_Vehiculo { get; set; }
        public double Capacidad_Vehiculo { get; set; }
        public int Id_EO_RS_Llegada { get; set; }
        public string EO_RS_Llegada { get; set; }
        public string Registro_EO_RS_Llegada { get; set; }
        public DateTime Fecha_Vencimiento_Registro_Llegada { get; set; }
        public string Num_Autorizacion_Sanitaria_Llegada { get; set; }
        public string Num_Autorizacion_Municipal_Llegada { get; set; }
        public List<string> Certificado_Trat_Disp { get; set; }
        public double Cantidad_RS_Dispuesto { get; set; }
        public DateTime? Fecha_Devolucion { get; set; }
        public List<string> Informe_Transporte { get; set; }
        public List<string> Manifiesto { get; set; }
        public int Id_Asesor_MAP { get; set; }
        public string Asesor_MAP { get; set; }
        public DateTime Fecha_Registro { get; set; }
        public int Id_Registrado_Por { get; set; }
        public string Estado { get; set; }
        public bool Permiso { get; set; }
        public ManifiestoRSOutput(Domain.ResiduosSolidos.Manifiesto_RS manifiesto_RS)
        {
            Id_Manifiesto_RS = manifiesto_RS.Id_Manifiesto_RS;
            Item = manifiesto_RS.Item;
            Serie_Manifiesto = manifiesto_RS.Serie_Manifiesto;
            Num_Manifiesto = manifiesto_RS.Num_Manifiesto;
            Fecha_Emision_Manifiesto = manifiesto_RS.Fecha_Emision_Manifiesto;
            Observacion_Manifiesto = manifiesto_RS.Observacion_Manifiesto;
            Id_Tipo_Residuo_Solido = manifiesto_RS.Residuo_Solido.Tipo_Residuo.Id_Tipo_Residuo;
            Tipo_Residuo_Solido = manifiesto_RS.Residuo_Solido.Tipo_Residuo.Descripcion;
            Id_Resido_Solido = manifiesto_RS.Residuo_Solido.Id_Residuo_Solido;
            Residuo_Solido = manifiesto_RS.Residuo_Solido.Descripcion;
            Cantidad_RS_Evacuado = manifiesto_RS.Cantidad_RS_Evacuado;
            Id_EO_RS_Salida = manifiesto_RS.EO_RS_Salida.Id_EO_RS;
            EO_RS_Salida = manifiesto_RS.EO_RS_Salida.Razon_Social;
            Registro_EO_RS_Salida = manifiesto_RS.Registro_EO_RS_Salida;
            Fecha_Vencimiento_Registro_Salida = manifiesto_RS.Fecha_Vencimiento_Registro_Salida;
            Num_Autorizacion_Municipal_Salida = manifiesto_RS.Num_Autorizacion_Municipal_Salida;
            Num_Autorizacion_Hoja_Ruta = manifiesto_RS.Num_Autorizacion_Hoja_Ruta;
            Id_Conductor = manifiesto_RS.Conductor.Id_Conductor_RS;
            Num_Licencia_Condutor = manifiesto_RS.Num_Licencia_Condutor;
            Id_Vehiculo = manifiesto_RS.Vehiculo.Id_Vehiculo_RS;
            Capacidad_Vehiculo = manifiesto_RS.Capacidad_Vehiculo;
            EO_RS_Llegada = manifiesto_RS.EO_RS_Llegada.Razon_Social;
            Id_EO_RS_Llegada = manifiesto_RS.EO_RS_Llegada.Id_EO_RS;
            Registro_EO_RS_Llegada = manifiesto_RS.Registro_EO_RS_Llegada;
            Fecha_Vencimiento_Registro_Llegada = manifiesto_RS.Fecha_Vencimiento_Registro_Llegada;
            Num_Autorizacion_Sanitaria_Llegada = manifiesto_RS.Num_Autorizacion_Sanitaria_Llegada;
            Num_Autorizacion_Municipal_Llegada = manifiesto_RS.Num_Autorizacion_Municipal_Llegada;
            if (manifiesto_RS.Certificado_Trat_Disp != null)
            {
                var certificados = JArray.Parse(manifiesto_RS.Certificado_Trat_Disp);
                Certificado_Trat_Disp = new List<string>();
                foreach (var evidencia in certificados)
                {
                    Certificado_Trat_Disp.Add((string)evidencia);
                }
            }
            
            Cantidad_RS_Dispuesto = manifiesto_RS.Cantidad_RS_Dispuesto;
            Fecha_Devolucion = manifiesto_RS.Fecha_Devolucion;
            if (manifiesto_RS.Informe_Transporte != null)
            {
                var informes = JArray.Parse(manifiesto_RS.Informe_Transporte);
                Informe_Transporte = new List<string>();
                foreach (var evidencia in informes)
                {
                    Informe_Transporte.Add((string)evidencia);
                }
            }
            if (manifiesto_RS.Manifiesto != null)
            {
                var manifiestos = JArray.Parse(manifiesto_RS.Manifiesto);
                Manifiesto = new List<string>();
                foreach (var evidencia in manifiestos)
                {
                    Manifiesto.Add((string)evidencia);
                }
            }
            Id_Asesor_MAP = manifiesto_RS.Asesor_MAP !=null? manifiesto_RS.Asesor_MAP.IdUsuario:0;
            Asesor_MAP = manifiesto_RS.Asesor_MAP!=null?manifiesto_RS.Asesor_MAP.Nombres + " " + manifiesto_RS.Asesor_MAP.Apellidos:"";
            
            Fecha_Registro = manifiesto_RS.Fecha_Registro;
            Id_Registrado_Por = manifiesto_RS.Id_Registrado_Por;
            Estado = manifiesto_RS.Estado;
        }
    }    
}
