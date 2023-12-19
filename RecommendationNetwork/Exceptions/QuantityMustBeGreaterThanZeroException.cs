namespace RecommendationNetwork.Exceptions
{
    public class QuantityMustBeGreaterThanZeroException : DataInconsistencyException
    {
        public QuantityMustBeGreaterThanZeroException(int productId) : base(String.Format("Value of product {0} must be greater than 0", productId))
        {
            this.HResult = 400;
        }
    }
}
