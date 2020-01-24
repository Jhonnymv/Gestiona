using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental
{
    public class UserInfo
    {
        public string NombreUsuario { get; set; }
        public InspeccionRol Rol { get; set; }
    }
}
