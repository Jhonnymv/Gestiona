using GestionAmbiental.Application.Repositories.Info_SGA;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA
{
    public class GetInfo_SGA : IGetInfo_SGA
    {
        private readonly IInfo_SGAReadOnlyRepository _info_SGAReadOnlyRepository;

        public GetInfo_SGA(IInfo_SGAReadOnlyRepository info_SGAReadOnlyRepository)
        {
            _info_SGAReadOnlyRepository = info_SGAReadOnlyRepository ?? throw new ArgumentNullException(nameof(info_SGAReadOnlyRepository));
        }

        public async Task<Info_SGAOutput> Execute(Tipo_Info_SGA tipo)
        {
            var info = await _info_SGAReadOnlyRepository.GetInfo_SGA(tipo.ToString());

            var output = new Info_SGAOutput(info);
            return output;
        }
        public async Task<LDAPConnectionOutput> ExecuteLDAPConnection(Tipo_Info_SGA tipo)
        {
            var info = await _info_SGAReadOnlyRepository.GetInfo_SGA(tipo.ToString());

            var output = new LDAPConnectionOutput(info);
            return output;
        }
    }
}
