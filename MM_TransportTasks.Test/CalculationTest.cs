using Microsoft.VisualStudio.TestTools.UnitTesting;
using MM_TransportTasks.Models.Abstracts;
using MM_TransportTasks.Models.Poco;
using MM_TransportTasks.Models.Trasports;
using MM_TransportTasks.Test.Poco;
using System.Collections.Generic;

namespace MM_TransportTasks.Test
{
    [TestClass]
    public class CalculationTest
    {
        private readonly DataMatrix _matrix1;
        private readonly DataMatrix _matrix2;

        public CalculationTest()
        {
            _matrix1 = new DataMatrix(
                buyers: 4,
                sellers: 3,
                buyersNeed: new List<MatrixValue>() 
                { 
                    new MatrixValue(5), new MatrixValue(10), new MatrixValue(10), new MatrixValue(5)
                },
                sellersOffer: new List<MatrixValue>() 
                { 
                    new MatrixValue(10), new MatrixValue(13), new MatrixValue(7)
                },
                variables: new List<MatrixValue>() 
                { 
                    new MatrixValue(4), new MatrixValue(6), new MatrixValue(5), new MatrixValue(13), 
                    new MatrixValue(3), new MatrixValue(7), new MatrixValue(6), new MatrixValue(10), 
                    new MatrixValue(5), new MatrixValue(2), new MatrixValue(2), new MatrixValue(6), 
                });

            _matrix2 = new DataMatrix(
                buyers: 5,
                sellers: 4,
                buyersNeed: new List<MatrixValue>()
                {
                    new MatrixValue(30), new MatrixValue(90), new MatrixValue(60), new MatrixValue(90), new MatrixValue(30)
                },
                sellersOffer: new List<MatrixValue>()
                {
                    new MatrixValue(30), new MatrixValue(60), new MatrixValue(90), new MatrixValue(60)
                },
                variables: new List<MatrixValue>()
                {
                    new MatrixValue(1), new MatrixValue(3), new MatrixValue(4), new MatrixValue(3), new MatrixValue(1),
                    new MatrixValue(9), new MatrixValue(5), new MatrixValue(2), new MatrixValue(4), new MatrixValue(8),
                    new MatrixValue(4), new MatrixValue(4), new MatrixValue(7), new MatrixValue(4), new MatrixValue(3),
                    new MatrixValue(5), new MatrixValue(7), new MatrixValue(2), new MatrixValue(6), new MatrixValue(6),
                });

        }

        [TestMethod]
        public void Matrix1_NorthWest()
        {
            ITask task = new NorthWest(
                buyers: _matrix1.Buyers,
                sellers: _matrix1.Sellers,
                buyersNeed: _matrix1.BuyersNeed,
                sellersOffer: _matrix1.SellersOffer,
                variables: _matrix1.Variables);

            var (matrix, z) = task.Calculate(new MinZ());

            Assert.AreEqual(167, z);
        }

        [TestMethod]
        public void Matrix2_NorthWest()
        {
            ITask task = new NorthWest(
                buyers: _matrix2.Buyers,
                sellers: _matrix2.Sellers,
                buyersNeed: _matrix2.BuyersNeed,
                sellersOffer: _matrix2.SellersOffer,
                variables: _matrix2.Variables);

            var (matrix, z) = task.Calculate(new MinZ());

            Assert.AreEqual(1230, z);
        }

        [TestMethod]
        public void Matrix1_MinimalPrice_Min()
        {
            ITask task = new MinimalPrice(
                buyers: _matrix1.Buyers,
                sellers: _matrix1.Sellers,
                buyersNeed: _matrix1.BuyersNeed,
                sellersOffer: _matrix1.SellersOffer,
                variables: _matrix1.Variables);

            var (matrix, z) = task.Calculate(new MinZ());

            Assert.AreEqual(150, z);
        }

        [TestMethod]
        public void Matrix1_MinimalPrice_Max()
        {
            ITask task = new MinimalPrice(
                buyers: _matrix1.Buyers,
                sellers: _matrix1.Sellers,
                buyersNeed: _matrix1.BuyersNeed,
                sellersOffer: _matrix1.SellersOffer,
                variables: _matrix1.Variables);

            var (matrix, z) = task.Calculate(new MaxZ());

            Assert.AreEqual(207, z);
        }

        [TestMethod]
        public void Matrix2_MinimalPrice_Min()
        {
            ITask task = new MinimalPrice(
                buyers: _matrix2.Buyers,
                sellers: _matrix2.Sellers,
                buyersNeed: _matrix2.BuyersNeed,
                sellersOffer: _matrix2.SellersOffer,
                variables: _matrix2.Variables);

            var (matrix, z) = task.Calculate(new MinZ());

            Assert.AreEqual(840, z);
        }

        [TestMethod]
        public void Matrix2_MinimalPrice_Max()
        {
            ITask task = new MinimalPrice(
                buyers: _matrix2.Buyers,
                sellers: _matrix2.Sellers,
                buyersNeed: _matrix2.BuyersNeed,
                sellersOffer: _matrix2.SellersOffer,
                variables: _matrix2.Variables);

            var (matrix, z) = task.Calculate(new MaxZ());

            Assert.AreEqual(1560, z);
        }
    }
}
