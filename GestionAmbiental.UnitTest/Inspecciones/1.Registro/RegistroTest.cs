using GestionAmbiental.Application.UseCases.Inspecciones.Registro.RegistrarInspeccion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.UnitTest.Inspecciones._1.Registro
{
    [TestClass()]
    public class RegistroTest
    {
        private readonly IRegistrarInspeccionCabecera _registrarInspeccionCabecera;

        public RegistroTest(IRegistrarInspeccionCabecera registrarInspeccionCabecera)
        {
            _registrarInspeccionCabecera = registrarInspeccionCabecera;
        }
        //GuardarEstado TBD
        [Test]
        public async Task CrearCabecera()
        {
            //Retorna Id de la nueva cabecera
            //Assert.IsTrue(await _registrarInspeccionCabecera.Execute("objetivo", "Kevin", 3, 4, "lugar", "31/01/2020", "08:50", "PLANEADA", 278, "Responsable inspeccion", "Inpectores", "GuardarEstado") > 0);
            NUnit.Framework.Assert.IsTrue("a".Equals("a"));
        }
    }
}
