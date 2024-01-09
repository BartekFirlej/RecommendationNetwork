namespace RecommendationNetwork.Exceptions
{
    public class NotFoundProductException : DataInconsistencyException
    {
        public NotFoundProductException() : base(String.Format("Not found any product."))
        {
            this.HResult = 404;
        }
        public NotFoundProductException(int productId) : base(String.Format("Not found product with id {0}.", productId))
        {
            this.HResult = 404;
        }
    }
}