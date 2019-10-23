using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class Empresa_Contratista
    {
        public int Id_Empresa_Contratista { get; set; }
        public string Razon_Social { get; set; }
        public int EPCM { get; set; }

        public Empresa_Contratista(int id_Empresa_Contratista, string razon_Social, int ePCM)
        {
            Id_Empresa_Contratista = id_Empresa_Contratista;
            Razon_Social = razon_Social;
            EPCM = ePCM;
        }
    }
}
