using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua
{
    public class ConsumoAguaOutput
    {

        public int IdConsumoAgua { get; set; }
        public int IdResolucionConsumoAgua { get; set; }
        public string ResolucionConsumoAgua { get; set; }
        public string FuenteAgua { get; set; }
        public DateTime Fecha { get; set; }
        public double TiempoBombeo { get; set; }
        public double LecturaInicial { get; set; }
        public double LecturaFinal { get; set; }
        public double VolumenBombeado { get; set; }
        public double CaudalPromedio { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Estado { get; set; }
        public ConsumoAguaOutput()
        {

        }
        public ConsumoAguaOutput(Domain.ConsumoAgua.ConsumoAgua consumoAgua)
        {
            IdConsumoAgua = consumoAgua.IdConsumoAgua;
            IdResolucionConsumoAgua = consumoAgua.ResolucionConsumoAgua.IdResolucionConsumoAgua;
            ResolucionConsumoAgua = consumoAgua.ResolucionConsumoAgua.Resolucion;
            FuenteAgua = consumoAgua.ResolucionConsumoAgua.FuenteAgua.Descripcion;
            Fecha = consumoAgua.Fecha;
            TiempoBombeo = consumoAgua.TiempoBombeo;
            LecturaInicial = consumoAgua.LecturaInicial;
            LecturaFinal = consumoAgua.LecturaFinal;
            VolumenBombeado = consumoAgua.VolumenBombeado;
            CaudalPromedio = consumoAgua.CaudalPromedio;
            IdUsuario = consumoAgua.IdUsuario;
            FechaRegistro = consumoAgua.FechaRegistro;
            Estado = consumoAgua.Estado;
        }
    }
}
