using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.ConsumoAgua.AguaMar
{
    public class ConsumoAguaMarModel
    {
        public int Id { get; set; }
        [DisplayName("Resolución:")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Debe seleccionar una opción")]
        public int IdResolucion { get; set; }
        public string Resolucion { get; set; }
        [DisplayName("Fuente de Agua:")]
        public string FuenteAgua { get; set; }
        [DisplayName("Fecha:")]
        public string Fecha { get; set; }
        [DisplayName("Tiempo de Bombeo (Horas):")]
        public double TiempoBombeo { get; set; }
        [DisplayName("Lectura Inicial Flujometro en el día (m3):")]
        public double LecturaInicial { get; set; }
        [DisplayName("Lectura Final Flujometro en el día (m3):")]
        public double LecturaFinal { get; set; }
        [DisplayName("Volumen de Agua Bombeado (consumo) m3:")]
        public double VolumenAguaBombeado { get; set; }
        [DisplayName("Caudal Promedio Consumido (m3/s):")]
        public double CaudalPromedio { get; set; }
    }
}
