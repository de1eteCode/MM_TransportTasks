using MM_TransportTasks.Models.Abstracts;
using MM_TransportTasks.Models.Poco;
using System.Collections.Generic;
using System.Linq;

namespace MM_TransportTasks.Models.Trasports
{
    public class MinZ : IFuncZ
    {
        public string Name => "min";

        public MatrixValue GetValue(List<MatrixValue> variables)
        {
            var item = variables.Where(item => item.IsUsed is false && item.Value > 0)
                    .OrderBy(item => item.Value)
                    .FirstOrDefault();

            if (item is null)
            {
                item = variables.Where(item => item.IsUsed is false)
                    .OrderBy(item => item.Value)
                    .First();
            }

            return item;
        }
    }
}
