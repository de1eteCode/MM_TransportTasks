using MM_TransportTasks.Models.Abstracts;
using MM_TransportTasks.Models.Poco;
using System;
using System.Collections.Generic;

namespace MM_TransportTasks.Models.Trasports
{
    internal class TaskMethodSelector
    {
        private TaskMethodSelector(string name, Type method)
        {
            Name = name;
            Method = method;
        }

        /// <summary>
        /// Создан для избежания конфликтных ситуаций при работе <see cref="Calculation(int, int, List{MatrixValue}, List{MatrixValue}, List{MatrixValue}, IFuncZ)">
        /// </summary>
        /// <typeparam name="T">Метод решения</typeparam>
        /// <param name="name">Имя метода решения</param>
        /// <returns>Экземпляр данного класса</returns>
        public static TaskMethodSelector Ctor<T>(string name)
            where T : ITask
        {
            return new TaskMethodSelector(name, typeof(T));
        }

        public string Name { get; }
        public Type Method { get; }

        /// <summary>
        /// Расчет
        /// </summary>
        public (int[,], int) Calculation(
            int buyers,
            int sellers,
            List<MatrixValue> buyersNeed,
            List<MatrixValue> sellersOffer,
            List<MatrixValue> variables,
            IFuncZ funcZ)
        {
            var obj = (ITask?)Activator.CreateInstance(Method, buyers, sellers, buyersNeed, sellersOffer, variables);
            return obj.Calculate(funcZ);
        }
    }
}
