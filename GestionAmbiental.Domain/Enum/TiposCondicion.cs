using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Enum
{
    public enum TiposCondicion
    {
        CSA = 1,    //Condiciones subestándares ambientales
        ASA = 2,    //Actos Subestándares Ambientales
        ASS = 3,    //Actos Subestándares Seguridad
        CSS = 4,     //Condiciones Subestándares Seguridad
        BPA = 5,     //Buenas Prácticas Ambientales
        BPS = 6     //Buenas Prácticas Seguridad
    }
}
