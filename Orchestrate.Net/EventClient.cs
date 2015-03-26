using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Orchestrate.Net
{
	public class EventClient
    {
		private readonly ICommunication _communication;

		public EventClient(ICommunication communication)
		{
			_communication = communication;
		}

        public EventResultList GetEvents(string collectionName, string key, string type, DateTime? start = null, DateTime? end = null)
        {
	        return AggregateExceptionUnpacker.Unwrap(()=> GetEventsAsync(collectionName, key, type, start, end).Result);
        }

		public Result PostEvent(string collectionName, string key, string type, DateTime? timeStamp, string msg)
		{
			return AggregateExceptionUnpacker.Unwrap(() => PostEventAsync(collectionName, key, type, timeStamp, msg).Result);
		}

		public async Task<Result> PostEventAsync(string collectionName, string key, string type, DateTime? timeStamp, string msg)
		{
			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key", "key cannot be null or empty");

			if (string.IsNullOrEmpty(type))
				throw new ArgumentNullException("type", "type cannot be null or empty");

			var url = collectionName + "/" + key + "/events/" + type;

			if (timeStamp != null)
				url += "?timestamp=" + ConvertToUnixTimestamp(timeStamp.Value);

			var message = new EventMessage { Msg = msg };
			var json = JsonConvert.SerializeObject(message);

			var baseResult = await _communication.CallWebRequestAsync(url, "POST", json);
			var ordinal = ExtractOrdinalFromLocation(baseResult);

			return new Result
			{
				Path = new OrchestratePath(collectionName, key, baseResult.ETag, ordinal),
				Score = 1,
				Value = baseResult.Payload
			};
		}

        public Result PutEvent(string collectionName, string key, string type, DateTime? timeStamp, string msg)
        {
			return AggregateExceptionUnpacker.Unwrap(() => PutEventAsync(collectionName, key, type, timeStamp, msg).Result);
        }

        public async Task<EventResultList> GetEventsAsync(string collectionName, string key, string type, DateTime? start = null, DateTime? end = null)
        {
            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "key cannot be null or empty");

            if (string.IsNullOrEmpty(type))
                throw new ArgumentNullException("type", "type cannot be null or empty");

            var url = collectionName + "/" + key + "/events/" + type;

            if (start != null)
                url += "?start=" + ConvertToUnixTimestamp(start.Value);

            if (end != null && start != null)
                url += "&end=" + ConvertToUnixTimestamp(end.Value);
            else if (end != null)
                url += "?end=" + ConvertToUnixTimestamp(end.Value);

            var result = await _communication.CallWebRequestAsync(url, "GET", null);

            return JsonConvert.DeserializeObject<EventResultList>(result.Payload);
        }

        public async Task<Result> PutEventAsync(string collectionName, string key, string type, DateTime? timeStamp, string msg)
        {
            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "key cannot be null or empty");

            if (string.IsNullOrEmpty(type))
                throw new ArgumentNullException("type", "type cannot be null or empty");

            var url = collectionName + "/" + key + "/events/" + type;

            if (timeStamp != null)
                url += "?timestamp=" + ConvertToUnixTimestamp(timeStamp.Value);

            var message = new EventMessage { Msg = msg };
            var json = JsonConvert.SerializeObject(message);

            var baseResult = await _communication.CallWebRequestAsync(url, "PUT", json);

            return new Result
            {
                Path = new OrchestratePath(collectionName,key,baseResult.ETag),
                Score = 1,
                Value = baseResult.Payload
            };
        }

        private static double ConvertToUnixTimestamp(DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalMilliseconds);
        }

		private static string ExtractOrdinalFromLocation(BaseResult baseResult)
		{
			// Always in the format /v0/<collection>/<key>/events/<type>/<timestamp>/<ordinal>
			var fragments = baseResult.Location.Split('/');
			return fragments[fragments.Length - 1];
		}
    }
}
