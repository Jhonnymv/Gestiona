using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Util.Interfaces
{
    public interface IGestorEncriptado
    {
        Task EncryptFileAsync(string inputFile, string outputFile);
        Task DecryptFileAsync(string inputFile, string outputFile);
    }
}