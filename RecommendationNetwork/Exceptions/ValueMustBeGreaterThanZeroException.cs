namespace RecommendationNetwork.Exceptions
{
    public class ValueMustBeGreaterThanZeroException : DataInconsistencyException
    {
        public ValueMustBeGreaterThanZeroException(int productId) : base(String.Format("Value of product {0} must be greater than 0", productId))
        {
            this.HResult = 400;
        }
        public ValueMustBeGreaterThanZeroException(string name, int productId) : base(String.Format("{0} of product {1} must be greater than 0", name, productId))
        {
            this.HResult = 400;
        }
    }
}