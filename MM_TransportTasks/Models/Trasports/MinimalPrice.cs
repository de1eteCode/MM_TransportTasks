using MM_TransportTasks.Models.Abstracts;
using MM_TransportTasks.Models.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM_TransportTasks.Models.Trasports
{
    public class MinimalPrice : TransportTask
    {
        public MinimalPrice(
            int buyers,
            int sellers,
            List<MatrixValue> buyersNeed,
            List<MatrixValue> sellersOffer,
            List<MatrixValue> variables)
            : base(buyers, sellers, buyersNeed, sellersOffer, variables)
        {
        }

        protected override (int[,], int) CloseCalc(IFuncZ funcZ)
        {
            // Раскидка значений от продавцов к покупателям, записав в матрицу [n,m]
            int[,] matrixDelivery = new int[_sellers, _buyers];

            // Идем по стоимостям в таблице от минимального к максимальному
            // В ходе операций мы записываем в таблицу matrixDelivery значения
            for (int i = 0; i < _variables.Count; i++)
            {
                // Нахождение минимальной стоимости
                var coef = funcZ.GetValue(_variables);

                var indexInLineArray = _variables.IndexOf(coef);
                var n = indexInLineArray / _buyers;
                var m = indexInLineArray % _buyers;

                // расчитать сколько продавец может предложить и сколько нужно потребителю

                int sold = 0;

                if (_buyersNeed[m].Value >= _sellersOffer[n].Value)
                {
                    sold = _sellersOffer[n].Value;
                    _buyersNeed[m].Value -= _sellersOffer[n].Value;
                    _sellersOffer[n].Value = 0;
                    _sellersOffer[n].IsUsed = true;
                }
                else 
                {
                    sold = _buyersNeed[m].Value;
                    _sellersOffer[n].Value -= _buyersNeed[m].Value;
                    _buyersNeed[m].Value = 0;
                    _buyersNeed[m].IsUsed = true;
                }

                coef.IsUsed = true;

                // записать в таблицу доставки
                matrixDelivery[n, m] = sold;
            }

            return (matrixDelivery, CalcZ(matrixDelivery));
        }
    }
}
