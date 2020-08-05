using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class Empresa
    {
        public int IdEmpresa { get; set; }
        public string RazonSocial { get; set; }
        public string Estado { get; set; }
        public Empresa()
        {
        }

        public Empresa(int id_Empresa_Contratista, string razon_Social)
        {
            IdEmpresa = id_Empresa_Contratista;
            RazonSocial = razon_Social;
        }
    }
}
