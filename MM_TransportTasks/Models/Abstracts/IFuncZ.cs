using MM_TransportTasks.Models.Poco;
using System.Collections.Generic;

namespace MM_TransportTasks.Models.Abstracts
{
    public interface IFuncZ
    {
        string Name { get; }

        public MatrixValue GetValue(List<MatrixValue> variables);
    }
}
