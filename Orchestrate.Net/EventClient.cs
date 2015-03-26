using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Orchestrate.Net
{
	public static class AggregateExceptionUnpacker
	{
		public static TResult Unwrap<TResult>(Func<TResult> call)
		{
			TResult result = default(TResult);
			try
			{
				result = call();
			}
			catch (AggregateException ae)
			{
				ae.Handle(e => { throw e; });
			}
			return result;
		}
	}

	public class EventClient
    {
		private readonly ICommunication _communication;

		public EventClient(ICommunication communication)
		{
			_communication = communication;
		}

        public EventResultList GetEvents(string collectionName, string key, string type, DateTime? start = null, DateTime? end = null)
        {
	        return GetEventsAsync(collectionName, key, type, start, end).Result;
        }

		public Result PostEvent(string collectionName, string key, string type, DateTime? timeStamp, string msg)
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

			var baseResult = _communication.CallWebRequest(url, "PUT", json);

			return new Result
			{
				Path = new OrchestratePath(collectionName,key,baseResult.ETag),
				Score = 1,
				Value = baseResult.Payload
			};
		}

        public Result PutEvent(string collectionName, string key, string type, DateTime? timeStamp, string msg)
        {
	        return PutEventAsync(collectionName, key, type, timeStamp, msg).Result;
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

		private static string ExtractOrdinalFromLocation(BaseResult baseResult, string collection)
		{
			// Always in the format /v0/<collection>/<key>/events/<type>/<timestamp>/<ordinal>
			// <ref> is included in BaseResult
			string key = baseResult.Location.Replace("/v0/" + collection + "/", "");
			int index = key.IndexOf("/refs");
			return key.Remove(index);
		}
    }
}
