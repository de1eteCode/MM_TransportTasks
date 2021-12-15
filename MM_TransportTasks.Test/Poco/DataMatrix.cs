using MM_TransportTasks.Models.Poco;
using System.Collections.Generic;

namespace MM_TransportTasks.Test.Poco
{
    internal class DataMatrix
    {
        public int Buyers { get; }
        public int Sellers { get; }
        public List<MatrixValue> BuyersNeed { get; }
        public List<MatrixValue> SellersOffer { get; }
        public List<MatrixValue> Variables { get; }

        public DataMatrix(int buyers, int sellers, List<MatrixValue> buyersNeed, List<MatrixValue> sellersOffer, List<MatrixValue> variables)
        {
            Buyers = buyers;
            Sellers = sellers;
            BuyersNeed = buyersNeed;
            SellersOffer = sellersOffer;
            Variables = variables;
        }
    }
}
