using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS
{
    public class IngresoSalidaRSOutput
    {
        public string Item { get; set; }
        public int Id_Ingreso_Salida_RS { get; set; }
        public string ACR_CMRS { get; set; }
        public int Id_ACR_CMRS { get; set; }
        public string Responsable_ACR_CMRS { get; set; }
        public string Ingreso_Salida { get; set; }
        public int Id_Ingreso_Salida { get; set; }
        public string Motivo { get; set; }
        public int Id_Motivo { get; set; }
        public DateTime Fecha_Ingreso_Salida { get; set; }
        public string Tipo_Residuo { get; set; }
        public int Id_Tipo_Residuo { get; set; }
        public string Residuo_Solido { get; set; }
        public int Id_Residuo_Solido { get; set; }
        public double Cantidad { get; set; }
        public string Empresa { get; set; }
        public int Id_Empresa { get; set; }
        public string Responsable_Marcobre { get; set; }
        public int Id_Responsable_Marcobre { get; set; }
        public string Comentario { get; set; }
        public string Estado { get; set; }
        public int Id_Registrado_Por { get; set; }
        public DateTime Fecha_Registro { get; set; }
        public bool Permiso { get; set; }
        public IngresoSalidaRSOutput(Domain.ResiduosSolidos.Ingreso_Salida_RS item)
        {
            Item = item.Item;
            ACR_CMRS = item.ACR_CMRS.Descripcion;
            Id_ACR_CMRS = item.ACR_CMRS.Id_ACR_CMRS;
            Cantidad = item.Cantidad;
            Comentario = item.Comentario;
            Empresa = item.Empresa_RS.RazonSocial;
            Id_Empresa = item.Empresa_RS.IdEmpresa;
            Estado = item.Estado;
            Fecha_Ingreso_Salida = item.Fecha_Ingreso_Salida;
            Id_Ingreso_Salida_RS = item.Id_Ingreso_Salida_RS;
            Ingreso_Salida = item.Ingreso_Salida.Descripcion;
            Id_Ingreso_Salida = item.Ingreso_Salida.Id_Ingreso_Salida;
            Motivo = item.Motivo.Descripcion;
            Id_Motivo = item.Motivo.Id_Motivo;
            Residuo_Solido = item.Residuo_Solido.Descripcion;
            Id_Residuo_Solido = item.Residuo_Solido.Id_Residuo_Solido;
            Responsable_ACR_CMRS = item.Responsable_ACR_CMRS;
            Responsable_Marcobre = item.Responsable_Marcobre.Nombres + " " + item.Responsable_Marcobre.Apellidos;
            Id_Responsable_Marcobre = item.Responsable_Marcobre.IdUsuario;
            Tipo_Residuo = item.Residuo_Solido.Tipo_Residuo.Descripcion;
            Id_Tipo_Residuo = item.Residuo_Solido.Tipo_Residuo.Id_Tipo_Residuo;
            Id_Registrado_Por = item.Id_Registrado_Por;
            Fecha_Registro = item.Fecha_Registro;
        }
    }
}
