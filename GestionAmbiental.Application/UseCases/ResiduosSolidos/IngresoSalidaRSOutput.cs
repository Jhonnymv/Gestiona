using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos
{
    public class IngresoSalidaRSOutput
    {
        public int Item { get; set; }
        public int Id_Ingreso_Salida_RS { get; set; }
        public string ACR_CMRS { get; set; }
        public string Responsable_ACR_CMRS { get; set; }
        public string Ingreso_Salida { get; set; }
        public string Motivo { get; set; }
        public DateTime Fecha_Ingreso_Salida { get; set; }
        public string Tipo_Residuo { get; set; }
        public string Residuo_Solido { get; set; }
        public double Cantidad { get; set; }
        public string Empresa { get; set; }
        public string Responsable_Marcobre { get; set; }
        public string Comentario { get; set; }
        public string Estado { get; set; }
        public IngresoSalidaRSOutput(Domain.ResiduosSolidos.Ingreso_Salida_RS item)
        {
            Item = item.Item;
            ACR_CMRS = item.ACR_CMRS.Descripcion;
            Cantidad = item.Cantidad;
            Comentario = item.Comentario;
            Empresa = item.Empresa_RS.RazonSocial;
            Estado = item.Estado;
            Fecha_Ingreso_Salida = item.Fecha_Ingreso_Salida;
            Id_Ingreso_Salida_RS = item.Id_Ingreso_Salida_RS;
            Ingreso_Salida = item.Ingreso_Salida.Descripcion;
            Motivo = item.Motivo.Descripcion;
            Residuo_Solido = item.Residuo_Solido.Descripcion;
            Responsable_ACR_CMRS = item.Responsable_ACR_CMRS;
            Responsable_Marcobre = item.Responsable_Marcobre.Nombres + " " + item.Responsable_Marcobre.Apellidos;
            Tipo_Residuo = item.Residuo_Solido.Tipo_Residuo.Descripcion;
        }
    }
}
