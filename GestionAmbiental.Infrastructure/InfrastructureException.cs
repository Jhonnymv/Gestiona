using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Infrastructure
{
    public class InfrastructureException : Exception
    {
        internal InfrastructureException(string businessMessage)
            : base(businessMessage)
        {

        }
    }
}
