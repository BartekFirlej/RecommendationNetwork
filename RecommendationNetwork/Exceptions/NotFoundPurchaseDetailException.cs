namespace RecommendationNetwork.Exceptions
{
    public class NotFoundPurchaseDetailException : DataInconsistencyException
    {
        public NotFoundPurchaseDetailException() : base(String.Format("Not found any purchase details."))
        {
            this.HResult = 404;
        }
        public NotFoundPurchaseDetailException(int purchaseDetailId) : base(String.Format("Not found purchase detail with id {0}.", purchaseDetailId))
        {
            this.HResult = 404;
        }
    }
}
