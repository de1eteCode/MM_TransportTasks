namespace MM_TransportTasks.Models.Poco
{
    public class MatrixValue
    {
        public MatrixValue() { }

        public MatrixValue(int value)
        {
            Value = value;
        }

        public int Value { get; set; }
        public bool IsUsed { get; set; }
    }
}
