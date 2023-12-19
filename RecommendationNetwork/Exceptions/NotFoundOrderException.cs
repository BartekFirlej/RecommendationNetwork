namespace RecommendationNetwork.Exceptions
{
    public class NotFoundOrderException : DataInconsistencyException
    {
        public NotFoundOrderException() : base(String.Format("Not found any order."))
        {
            this.HResult = 404;
        }
        public NotFoundOrderException(int orderId) : base(String.Format("Not found order with id {0}.", orderId))
        {
            this.HResult = 404;
        }
    }
}
