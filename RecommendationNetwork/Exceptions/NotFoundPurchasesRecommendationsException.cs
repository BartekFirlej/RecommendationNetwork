namespace RecommendationNetwork.Exceptions
{
    public class NotFoundPurchasesRecommendationsException : DataInconsistencyException
    {
        public NotFoundPurchasesRecommendationsException() : base(String.Format("Not found any purchase recommendations."))
        {
            this.HResult = 404;
        }
        public NotFoundPurchasesRecommendationsException(int customerId) : base(String.Format("Not found purchase recommendations for customer with id {0}.", customerId))
        {
            this.HResult = 404;
        }
    }
}
