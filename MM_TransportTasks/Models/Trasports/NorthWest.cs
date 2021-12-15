using MM_TransportTasks.Models.Abstracts;
using MM_TransportTasks.Models.Poco;
using System;
using System.Collections.Generic;

namespace MM_TransportTasks.Models.Trasports
{
    public class NorthWest : TransportTask
    {
        public NorthWest(
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
            // FuncZ можно не использовать, т.к. алгоритм непокалебим

            int i = 0;
            int j = 0;

            int n = _sellers;
            int m = _buyers;

            int[,] matrixDelivery = new int[n, m];

            while (i < n && j < m)
            {
                try
                {
                    if (_sellersOffer[i].Value == 0)
                    {
                        i++;
                    }

                    if (_buyersNeed[j].Value == 0)
                    {
                        j++;
                    }

                    if (_sellersOffer[i].Value == 0 && _buyersNeed[j].Value == 0)
                    {
                        i++;
                        j++;
                    }

                    matrixDelivery[i, j] = MinValue(_sellersOffer[i].Value, _buyersNeed[j].Value);
                    _sellersOffer[i].Value -= matrixDelivery[i, j];
                    _buyersNeed[j].Value -= matrixDelivery[i, j];
                }
                catch (ArgumentOutOfRangeException)
                {
                    break;
                }
            }

            return (matrixDelivery, CalcZ(matrixDelivery));
        }
    }
}
