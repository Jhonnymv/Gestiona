using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ResiduosSolidos
{
    [Table("Tb_Manifiesto_RS")]
    public class Manifiesto_RS
    {
        [Key]
        public int Id_Manifiesto_RS { get; set; }
        [Column(TypeName = "varchar(6)")]
        public string Item { get; set; }
        [Column(TypeName = "int")]
        public int Serie_Manifiesto { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Num_Manifiesto { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha_Emision_Manifiesto { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Observacion_Manifiesto { get; set; }
        [Column(TypeName = "int")]
        public int Id_Residuo_Solido { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public double Cantidad_RS_Evacuado { get; set; }
        [Column(TypeName = "int")]
        public int Id_EO_RS_Salida { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Registro_EO_RS_Salida { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha_Vencimiento_Registro_Salida { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Num_Autorizacion_Municipal_Salida { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Num_Autorizacion_Hoja_Ruta { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Nombre_Conductor { get; set; }
        [Column(TypeName = "varchar(8)")]
        public string Num_Licencia_Condutor { get; set; }
        [Column(TypeName = "varchar(7)")]
        public string Placa_Vehiculo { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public double? Capacidad_Vehiculo { get; set; }
        [Column(TypeName = "int")]
        public int Id_EO_RS_Llegada { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Registro_EO_RS_Llegada { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha_Vencimiento_Registro_Llegada { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Num_Autorizacion_Sanitaria_Llegada { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Num_Autorizacion_Municipal_Llegada { get; set; }
        [Column(TypeName = "text")]
        public string Certificado_Trat_Disp { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public double Cantidad_RS_Dispuesto { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Fecha_Devolucion { get; set; }
        [Column(TypeName = "text")]
        public string Informe_Transporte { get; set; }
        [Column(TypeName = "text")]
        public string Manifiesto { get; set; }
        [Column(TypeName = "int")]
        public int? Id_Asesor_MAP { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha_Registro { get; set; }
        [Column(TypeName = "int")]
        public int Id_Registrado_Por { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Estado { get; set; }
    }
}
