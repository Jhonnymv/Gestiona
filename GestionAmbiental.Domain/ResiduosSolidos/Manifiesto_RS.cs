using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ResiduosSolidos
{
    public class Manifiesto_RS
    {
        public int Id_Manifiesto_RS { get; set; }
        public int Item { get; set; }
        public int Serie_Manifiesto { get; set; }
        public string Num_Manifiesto { get; set; }
        public DateTime Fecha_Emision_Manifiesto { get; set; }
        public string Observacion_Manifiesto { get; set; }
        public Residuo_Solido Residuo_Solido { get; set; }
        public double Cantidad_RS_Evacuado { get; set; }
        public EO_RS EO_RS_Salida { get; set; }
        public string Registro_EO_RS_Salida { get; set; }
        public DateTime Fecha_Vencimiento_Registro_Salida { get; set; }
        public string Num_Autorizacion_Municipal_Salida { get; set; }
        public string Num_Autorizacion_Hoja_Ruta { get; set; }
        public string Nombre_Conductor { get; set; }
        public string Num_Licencia_Condutor { get; set; }
        public string Placa_Vehiculo { get; set; }
        public double Capacidad_Vehiculo { get; set; }
        public EO_RS EO_RS_Llegada { get; set; }
        public string Registro_EO_RS_Llegada { get; set; }
        public DateTime Fecha_Vencimiento_Registro_Llegada { get; set; }
        public string Num_Autorizacion_Sanitaria_Llegada { get; set; }
        public string Num_Autorizacion_Municipal_Llegada { get; set; }
        public string Certificado_Trat_Disp { get; set; }
        public double Cantidad_RS_Dispuesto { get; set; }
        public DateTime Fecha_Devolucion { get; set; }
        public string Informe_Transporte { get; set; }
        public string Manifiesto { get; set; }
        public Usuario Asesor_MAP { get; set; }
        public DateTime Fecha_Registro { get; set; }
        public int Id_Registrado_Por { get; set; }
        public string Estado { get; set; }

        public Manifiesto_RS(
            int id_Manifiesto_RS, int item, int serie_Manifiesto, string num_Manifiesto, 
            DateTime fecha_Emision_Manifiesto, string observacion_Manifiesto, 
            Residuo_Solido residuo_Solido, double cantidad_RS_Evacuado, 
            EO_RS eO_RS_Salida, string registro_EO_RS_Salida, 
            DateTime fecha_Vencimiento_Registro_Salida, string num_Autorizacion_Municipal_Salida, 
            string num_Autorizacion_Hoja_Ruta, string nombre_Conductor, string num_Licencia_Condutor, 
            string placa_Vehiculo, double capacidad_Vehiculo, EO_RS eO_RS_Llegada, 
            string registro_EO_RS_Llegada, DateTime fecha_Vencimiento_Registro_Llegada, 
            string num_Autorizacion_Sanitaria_Llegada, string num_Autorizacion_Municipal_Llegada,
            string certificado_Trat_Disp, double cantidad_RS_Dispuesto, DateTime fecha_Devolucion,
            string informe_Transporte, string manifiesto, Usuario asesor_MAP, string estado,int id_registrado_por,DateTime fecha_registro)
        {
            Id_Manifiesto_RS = id_Manifiesto_RS;
            Item = item;
            Serie_Manifiesto = serie_Manifiesto;
            Num_Manifiesto = num_Manifiesto;
            Fecha_Emision_Manifiesto = fecha_Emision_Manifiesto;
            Observacion_Manifiesto = observacion_Manifiesto;
            Residuo_Solido = residuo_Solido;
            Cantidad_RS_Evacuado = cantidad_RS_Evacuado;
            EO_RS_Salida = eO_RS_Salida;
            Registro_EO_RS_Salida = registro_EO_RS_Salida;
            Fecha_Vencimiento_Registro_Salida = fecha_Vencimiento_Registro_Salida;
            Num_Autorizacion_Municipal_Salida = num_Autorizacion_Municipal_Salida;
            Num_Autorizacion_Hoja_Ruta = num_Autorizacion_Hoja_Ruta;
            Nombre_Conductor = nombre_Conductor;
            Num_Licencia_Condutor = num_Licencia_Condutor;
            Placa_Vehiculo = placa_Vehiculo;
            Capacidad_Vehiculo = capacidad_Vehiculo;
            EO_RS_Llegada = eO_RS_Llegada;
            Registro_EO_RS_Llegada = registro_EO_RS_Llegada;
            Fecha_Vencimiento_Registro_Llegada = fecha_Vencimiento_Registro_Llegada;
            Num_Autorizacion_Sanitaria_Llegada = num_Autorizacion_Sanitaria_Llegada;
            Num_Autorizacion_Municipal_Llegada = num_Autorizacion_Municipal_Llegada;
            Certificado_Trat_Disp = certificado_Trat_Disp;
            Cantidad_RS_Dispuesto = cantidad_RS_Dispuesto;
            Fecha_Devolucion = fecha_Devolucion;
            Informe_Transporte = informe_Transporte;
            Manifiesto = manifiesto;
            Asesor_MAP = asesor_MAP;
            Estado = estado;
            Id_Registrado_Por = id_registrado_por;
            Fecha_Registro = fecha_registro;
        }

        public Manifiesto_RS()
        {
        }
    }
}
