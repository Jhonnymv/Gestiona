using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.FOCAM.Tipo
{
    public class TiposOuput
    {
        public List<TipoOuput> Registros { get; set; }
        public int NRegistros { get; set; }

        public IEnumerable<object> OrderBy(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}
