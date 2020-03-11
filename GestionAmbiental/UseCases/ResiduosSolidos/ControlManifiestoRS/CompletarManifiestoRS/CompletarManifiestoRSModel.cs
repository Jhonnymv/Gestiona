using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.ResiduosSolidos.ControlManifiestoRS.CompletarManifiestoRS
{
    public class CompletarManifiestoRSModel
    {
        public int Id_Manifiesto_RS { get; set; }
        public string opcion { get; set; }
        public string Id_Encriptado { get; set; }
        [DisplayName("Serie:")]
        public int Serie_Manifiesto { get; set; }
        [DisplayName("N° Manifiesto:")]
        public string Num_Manifiesto { get; set; }
        [DisplayName("Fecha Emisión:")]
        public DateTime Fecha_Emision_Manifiesto { get; set; }
        [DisplayName("Observación Manifiesto:")]
        public string Observacion_Manifiesto { get; set; }
        [DisplayName("Nombre del Residuo (Nombre que va en el manifiesto):")]
        public int Id_Residuo_Solido { get; set; }
        [DisplayName("Cantidad Residuo Evacuado y Transportado TM:")]
        public double Cantidad_RS_Evacuado { get; set; }
        [DisplayName("Nombre de EO-RS:")]
        public int Id_EO_RS_Salida { get; set; }
        [DisplayName("Registro de la EO-RS:")]
        public string Registro_EO_RS_Salida { get; set; }
        [DisplayName("Fecha de Vencimiento Registro:")]
        public DateTime Fecha_Vencimiento_Registro_Salida { get; set; }
        [DisplayName("N° Autorización Municipal:")]
        public string Num_Autorizacion_Municipal_Salida { get; set; }
        [DisplayName("N° Autorización de Hoja Ruta:")]
        public string Num_Autorizacion_Hoja_Ruta { get; set; }
        [DisplayName("Nombre del Conductor:")]
        public string Nombre_Conductor { get; set; }
        [DisplayName("N° Licencia Especial (A-4):")]
        public string Num_Licencia_Condutor { get; set; }
        [DisplayName("Placa del Vehículo:")]
        public string Placa_Vehiculo { get; set; }
        [DisplayName("Capacidad de Vehículo TM:")]
        public double Capacidad_Vehiculo { get; set; }
        [DisplayName("Nombre de la EO-RS:")]
        public int Id_EO_RS_Llegada { get; set; }
        [DisplayName("Registro de la EO-RS:")]
        public string Registro_EO_RS_Llegada { get; set; }
        [DisplayName("Fecha de Vencimiento Registro EO-RS:")]
        public DateTime Fecha_Vencimiento_Registro_Llegada { get; set; }
        [DisplayName("N° Autorización Sanitaria:")]
        public string Num_Autorizacion_Sanitaria_Llegada { get; set; }
        [DisplayName("N° Autorización Municipal:")]
        public string Num_Autorizacion_Municipal_Llegada { get; set; }
        [DisplayName("Certificado de Tratamiento y/o Disposición Final")]
        public List<IFormFile> Certificado { get; set; }
        [DisplayName("Cantidad del Residuo Dispuesto (TM):")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe completar este campo")]
        public double Cantidad_RS_Dispuesto { get; set; }
        [DisplayName("Fecha de Devolución (debe ser <15 días después de su emisión:")]
        public string Fecha_Devolucion { get; set; }
        [DisplayName("Informe de Transporte (Incluye guía de remisión remitente)")]
        public List<IFormFile> Informe_Transporte { get; set; }
        [DisplayName("Manifiesto")]
        public List<IFormFile> Manifiesto_Doc { get; set; }
        [DisplayName("Asesor MAP Responsable de la Recepción de Documentos:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opción")]
        public int Id_Asesor { get; set; }
        public string Estado { get; set; }
        public bool Permiso { get; set; }
    }
}
