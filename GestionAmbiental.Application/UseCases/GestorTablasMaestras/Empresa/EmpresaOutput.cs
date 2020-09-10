using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Empresa
{
    public class EmpresaOutput
    {
        public int IdEmpresa { get; set; }
        public string RazonSocial { get; set; }
        public string Estado { get; set; }

        public EmpresaOutput()
        {
        }
        public EmpresaOutput(GestionAmbiental.Domain.ValueObjects.Empresa empresa)
        {
            IdEmpresa = empresa.IdEmpresa;
            RazonSocial = empresa.RazonSocial;
            Estado = empresa.Estado;
        }
    }
}
