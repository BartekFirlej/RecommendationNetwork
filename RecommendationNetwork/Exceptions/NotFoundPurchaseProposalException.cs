namespace RecommendationNetwork.Exceptions
{
    public class NotFoundPurchaseProposalException : DataInconsistencyException
    {
        public NotFoundPurchaseProposalException() : base(String.Format("Not found any purchase proposals."))
        {
            this.HResult = 404;
        }
        public NotFoundPurchaseProposalException(int customerId, int productId) : base(String.Format("Not found purchase propsal for customer with id {0} and product id {1}.", customerId, productId))
        {
            this.HResult = 404;
        }
    }
}
