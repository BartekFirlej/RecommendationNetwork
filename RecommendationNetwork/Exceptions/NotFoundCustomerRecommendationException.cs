namespace RecommendationNetwork.Exceptions
{
    public class NotFoundCustomerRecommendationException : DataInconsistencyException
    {
        public NotFoundCustomerRecommendationException() : base(String.Format("Not found any customer recommendations."))
        {
            this.HResult = 404;
        }
        public NotFoundCustomerRecommendationException(int customerId) : base(String.Format("Not found customer recommendations for customer with id {0}.", customerId))
        {
            this.HResult = 404;
        }
    }
}