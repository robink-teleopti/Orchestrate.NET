using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Orchestrate.Net
{
	public class GraphClient
	{
		private readonly ICommunication _communication;

		public GraphClient(ICommunication communication)
		{
			_communication = communication;
		}

		public ListResult GetGraph(string collectionName, string key, string[] kinds)
		{
			return AggregateExceptionUnpacker.Unwrap(() => GetGraphAsync(collectionName, key, kinds).Result);
		}

		public Result PutGraph(string collectionName, string key, string kind, string toCollectionName, string toKey)
		{
			return AggregateExceptionUnpacker.Unwrap(() => PutGraphAsync(collectionName, key, kind, toCollectionName, toKey).Result);
		}

		public Result DeleteGraph(string collectionName, string key, string kind, string toCollectionName, string toKey)
		{
			return AggregateExceptionUnpacker.Unwrap(() => DeleteGraphAsync(collectionName, key, kind,toCollectionName,toKey).Result);
		}

		public async Task<ListResult> GetGraphAsync(string collectionName, string key, string[] kinds)
		{
			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key", "key cannot be null or empty");

			if (kinds == null || kinds.Length == 0)
				throw new ArgumentNullException("kinds", "kinds cannot be null or empty");

			var url = collectionName + "/" + key + "/relations";

			url = kinds.Aggregate(url, (current, kind) => current + ("/" + kind));
			var result = await _communication.CallWebRequestAsync(url, "GET", null);

			return JsonConvert.DeserializeObject<ListResult>(result.Payload);
		}

		public async Task<Result> PutGraphAsync(string collectionName, string key, string kind, string toCollectionName, string toKey)
		{
			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key", "key cannot be null or empty");

			if (string.IsNullOrEmpty(kind))
				throw new ArgumentNullException("kind", "kind cannot be null or empty");

			if (string.IsNullOrEmpty(toCollectionName))
				throw new ArgumentNullException("toCollectionName", "toCollectionName cannot be null or empty");

			if (string.IsNullOrEmpty(toKey))
				throw new ArgumentNullException("toKey", "toKey cannot be null or empty");

			var url = collectionName + "/" + key + "/relation/" + kind + "/" + toCollectionName + "/" + toKey;

			var baseResult = await _communication.CallWebRequestAsync(url, "PUT", null);

			return new Result
			{
				Path = new OrchestratePath(collectionName, key, baseResult.ETag),
				Score = 1,
				Value = baseResult.Payload
			};
		}

		public async Task<Result> DeleteGraphAsync(string collectionName, string key, string kind, string toCollectionName, string toKey)
		{
			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key", "key cannot be null or empty");

			if (string.IsNullOrEmpty(kind))
				throw new ArgumentNullException("kind", "kind cannot be null or empty");

			if (string.IsNullOrEmpty(toCollectionName))
				throw new ArgumentNullException("toCollectionName", "toCollectionName cannot be null or empty");

			if (string.IsNullOrEmpty(toKey))
				throw new ArgumentNullException("toKey", "toKey cannot be null or empty");

			var url = collectionName + "/" + key + "/relation/" + kind + "/" + toCollectionName + "/" + toKey + "?purge=true";

			var baseResult = await _communication.CallWebRequestAsync(url, "DELETE", null);

			return new Result
			{
				Path = new OrchestratePath(collectionName, key, baseResult.ETag),
				Score = 1,
				Value = baseResult.Payload
			};
		}
	}
}