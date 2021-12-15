using MM_TransportTasks.Models.Abstracts;
using MM_TransportTasks.Models.Poco;
using System.Collections.Generic;
using System.Linq;

namespace MM_TransportTasks.Models.Trasports
{
    public abstract class TransportTask : ITask
    {
        protected int _sellers;
        protected int _buyers;
        protected readonly List<MatrixValue> _buyersNeed;
        protected readonly List<MatrixValue> _sellersOffer;
        protected readonly List<MatrixValue> _variables;

        private MatrixValue? _fictitiousBuyer;
        private MatrixValue? _fictitiousSeller;

        protected TransportTask(
            int buyers,
            int sellers,
            List<MatrixValue> buyersNeed,
            List<MatrixValue> sellersOffer,
            List<MatrixValue> variables)
        {
            _sellers = sellers;
            _buyers = buyers;
            _buyersNeed = buyersNeed;
            _sellersOffer = sellersOffer;
            _variables = variables;
        }

        /// <summary>
        /// Решение задачи
        /// </summary>
        /// <param name="funcZ">Параметр для Z(x)</param>
        /// <returns>Матрица и целевая функция</returns>
        public (int[,], int) Calculate(IFuncZ funcZ)
        {
            if (OpenTask())
            {
                return OpenCalc(funcZ);
            }
            else
            {
                return CloseCalc(funcZ);
            }
        }

        /// <summary>
        /// Решение открытой задачи
        /// </summary>
        protected virtual (int[,], int) OpenCalc(IFuncZ funcZ)
        {
            // Введение фиктивного клиента/продавца
            // Вызов закрытого решения задачи

            int sumSellers = _sellersOffer.Sum(item => item.Value);
            int sumBuyers = _buyersNeed.Sum(item => item.Value);

            if (sumSellers > sumBuyers)
            {
                CreateFictitiousBuyer(sumSellers - sumBuyers);
                var buyer = GetFictitiousBuyer();
                _buyersNeed.Add(buyer);
                _buyers++;
                for (int i = 0; i < _sellers; i++)
                {
                    _variables.Add(new MatrixValue(0));
                }
            }
            else
            {
                CreateFictitiousSeller(sumBuyers - sumSellers);
                var seller = GetFictitiousSeller();
                _sellersOffer.Add(seller);
                _sellers++;
                for (int i = 0; i < _buyers; i++)
                {
                    _variables.Add(new MatrixValue(0));
                }
            }

            return CloseCalc(funcZ);
        }

        /// <summary>
        /// Решение закрытой задачи
        /// </summary>
        protected abstract (int[,], int) CloseCalc(IFuncZ funcZ);

        /// <summary>
        /// Определение задачи, открытая или закрытая
        /// </summary>
        /// <returns>True - открытая задача; False - закрытая</returns>
        protected bool OpenTask()
        {
            return 
                _buyersNeed.Sum(item => item.Value) != _sellersOffer.Sum(item => item.Value);
        }

        /// <summary>
        /// Создание фиктивного покупателя. Необходим при открытых задачах.
        /// </summary>
        /// <param name="need">Потребность покупателя</param>
        protected void CreateFictitiousBuyer(int need)
        {
            _fictitiousBuyer = new MatrixValue(need);
        }

        /// <summary>
        /// Получение фиктивного покупателя
        /// </summary>
        protected MatrixValue? GetFictitiousBuyer()
        {
            if (_fictitiousBuyer is not null)
            {
                return _fictitiousBuyer;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Создание фиктивного продавца. Необходим при открытых задачах.
        /// </summary>
        /// <param name="offer">Предложение продавца</param>
        protected void CreateFictitiousSeller(int offer)
        {
            _fictitiousSeller = new MatrixValue(offer);
        }

        /// <summary>
        /// Получение фиктивного продавца
        /// </summary>
        protected MatrixValue? GetFictitiousSeller()
        {
            if (_fictitiousSeller is null)
            {
                return null;
            }
            else
            {
                return _fictitiousSeller;
            }
        }

        /// <summary>
        /// Получение минимального числа
        /// </summary>
        protected int MinValue(int a, int b)
        {
            if (a < b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        /// <summary>
        /// Подсчет стоимости доставки для всех
        /// </summary>
        /// <param name="matrix">Матрица доставок</param>
        /// <returns>Сумма стоимости всех доставок</returns>
        protected int CalcZ(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);
            int z = 0;

            for (int i = 0, locN = 0, locM = 0; i < n * m; i++, locM++)
            {
                if (locM >= m)
                {
                    locM = 0;
                    locN++;
                }

                z += matrix[locN, locM] * _variables[i].Value;
            }

            return z;
        }
    }
}
