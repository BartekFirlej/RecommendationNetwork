namespace RecommendationNetwork.Exceptions
{
    public class NotFoundProductTypeException : DataInconsistencyException
    {
        public NotFoundProductTypeException() : base(String.Format("Not found any product type."))
        {
            this.HResult = 404;
        }
        public NotFoundProductTypeException(int productTypeId) : base(String.Format("Not found product type with id {0}.", productTypeId))
        {
            this.HResult = 404;
        }
    }
}
