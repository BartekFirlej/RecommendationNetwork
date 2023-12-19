namespace RecommendationNetwork.Exceptions
{
    public class NotFoundVoivodeshipException : DataInconsistencyException
    {
        public NotFoundVoivodeshipException() : base(String.Format("Not found any voivodeship."))
        {
            this.HResult = 404;
        }
        public NotFoundVoivodeshipException(int voivodeshipId) : base(String.Format("Not found voivodeship with id {0}.", voivodeshipId))
        {
            this.HResult = 404;
        }
    }
}
