using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociadaAvistamiento
{
    public class faunaAsociadaAvistamientoOuput
    {
        public int Id { get; set; }
        public int Id_Avistamiento { get; set; }
        public int Id_FaunaAsociada { get; set; }
        public string? Name{ get; set; }
        public string? Des { get; set; }
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }
    }
}
