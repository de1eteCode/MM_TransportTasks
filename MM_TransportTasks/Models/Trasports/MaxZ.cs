using MM_TransportTasks.Models.Abstracts;
using MM_TransportTasks.Models.Poco;
using System.Collections.Generic;
using System.Linq;

namespace MM_TransportTasks.Models.Trasports
{
    public class MaxZ : IFuncZ
    {
        public string Name => "max";

        public MatrixValue GetValue(List<MatrixValue> variables)
        {
            return variables.Where(item => item.IsUsed is false)
                    .OrderByDescending(item => item.Value)
                    .First();
        }
    }
}
