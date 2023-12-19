using Newtonsoft.Json;

namespace RecommendationNetwork.Exceptions
{
    public class DataInconsistencyException : Exception
    {
        [JsonProperty("Message")]
        public string Message;
        public DataInconsistencyException() : base("Data consistency problem.")
        {
            this.HResult = 404;
        }
        public DataInconsistencyException(string message) : base(message) {
            this.Message = message;
        }
        public Dictionary<string, object> ToJson()
        {
            return new Dictionary<string, object>()
            {
                {"Type", this.HResult},
                {"Message", this.Message}
            };
        }
    }
}
