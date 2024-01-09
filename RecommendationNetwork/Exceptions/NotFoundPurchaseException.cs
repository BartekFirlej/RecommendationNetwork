namespace RecommendationNetwork.Exceptions
{
    public class NotFoundPurchaseException : DataInconsistencyException
    {
        public NotFoundPurchaseException() : base(String.Format("Not found any purchases."))
        {
            this.HResult = 404;
        }
        public NotFoundPurchaseException(int orderId) : base(String.Format("Not found purchase with id {0}.", orderId))
        {
            this.HResult = 404;
        }
    }
}