using GestionAmbiental.Application.Repositories.Info_SGA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGAs
{
    public class GetInfo_SGAs : IGetInfo_SGAs
    {
        private IInfo_SGAReadOnlyRepository _info_SGAReadOnlyRepository;

        public GetInfo_SGAs(IInfo_SGAReadOnlyRepository info_SGAReadOnlyRepository)
        {
            _info_SGAReadOnlyRepository = info_SGAReadOnlyRepository ?? throw new ArgumentNullException(nameof(info_SGAReadOnlyRepository));
        }

        public async Task<Info_SGAsOutput> Execute(string filtro, int pagina, int rpp)
        {
            var info = await _info_SGAReadOnlyRepository.GetInfo_SGAs(filtro);
            var infoOutput = new List<Info_SGAOutput>();
            foreach (var item in info)
            {
                var temp = new Info_SGAOutput();
                temp.IdInfo = item.Id_Info;
                temp.Tipo = item.Tipo;
                infoOutput.Add(temp);
            }
            var output = new Info_SGAsOutput
            {
                nInfo_SGAs = infoOutput.Count,
                Info_SGAs = infoOutput.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };
            return output;
        }
    }
}
