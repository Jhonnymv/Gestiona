using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ResiduosSolidos
{
    public class Ingreso_Salida_RS
    {
        public string Item { get; set; }
        public int Id_Ingreso_Salida_RS { get; set; }
        public ACR_CMRS ACR_CMRS { get; set; }
        public Responsable_ARC_CRMS Responsable_ACR_CMRS { get; set; }
        public Ingreso_Salida Ingreso_Salida { get; set; }
        public Motivo_Ingreso_Salida Motivo { get; set; }
        public DateTime Fecha_Ingreso_Salida { get; set; }
        public Residuo_Solido Residuo_Solido { get; set; }
        public double Cantidad { get; set; }
        public Empresa Empresa_RS { get; set; }
        public Gerencia Gerencia { get; set; }
        public string Comentario { get; set; }
        public int Id_Registrado_Por { get; set; }
        public DateTime Fecha_Registro { get; set; }
        public string Estado { get; set; }

        public Ingreso_Salida_RS()
        {
        }

        public Ingreso_Salida_RS(
            int id_Ingreso_Salida_RS, string item, ACR_CMRS aCR_CMRS, Responsable_ARC_CRMS responsable_ACR_CMRS, 
            Ingreso_Salida ingreso_Salida, Motivo_Ingreso_Salida motivo, 
            DateTime fecha_Ingreso_Salida, Residuo_Solido residuo_Solido, double cantidad, 
            Empresa id_Empresa_RS, Gerencia gerencia, string comentario,
            int id_Registrado_Por,DateTime fecha_Registro, string estado)
        {
            Item = item;
            Id_Ingreso_Salida_RS = id_Ingreso_Salida_RS;
            ACR_CMRS = aCR_CMRS;
            Responsable_ACR_CMRS = responsable_ACR_CMRS;
            Ingreso_Salida = ingreso_Salida;
            Motivo = motivo;
            Fecha_Ingreso_Salida = fecha_Ingreso_Salida;
            Residuo_Solido = residuo_Solido;
            Cantidad = cantidad;
            Empresa_RS = id_Empresa_RS;
            Gerencia = gerencia;
            Comentario = comentario;
            Id_Registrado_Por = id_Registrado_Por;
            Fecha_Registro = fecha_Registro;
            Estado = estado;
        }
    }
}
