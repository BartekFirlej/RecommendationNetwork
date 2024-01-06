namespace RecommendationNetwork.Exceptions
{
    public class DateTimeException : DataInconsistencyException
    {
        public DateTimeException(int purchaseId) : base(String.Format("Date of purchase {0} must be greater than minimum date.", purchaseId))
        {
            this.HResult = 400;
        }
    }
}
