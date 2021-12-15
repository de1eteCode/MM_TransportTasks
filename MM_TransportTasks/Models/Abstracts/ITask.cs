namespace MM_TransportTasks.Models.Abstracts
{
    public interface ITask
    {
        public (int[,], int) Calculate(IFuncZ funcZ);
    }
}
