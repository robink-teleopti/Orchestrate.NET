using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Orchestrate.Net
{
	public class CollectionClient
	{
		private readonly ICommunication _communication;

		public CollectionClient(ICommunication communication)
		{
			_communication = communication;
		}

		public Result CreateCollection(string collectionName, string key, string item)
		{
			return AggregateExceptionUnpacker.Unwrap(() => CreateCollectionAsync(collectionName, key, item).Result);
		}

		public Result CreateCollection(string collectionName, string key, object item)
		{
			return AggregateExceptionUnpacker.Unwrap(() => CreateCollectionAsync(collectionName, key, item).Result);
		}

		public Result DeleteCollection(string collectionName)
		{
			return AggregateExceptionUnpacker.Unwrap(() => DeleteCollectionAsync(collectionName).Result);
		}

		public Result Get(string collectionName, string key)
		{
			return AggregateExceptionUnpacker.Unwrap(() => GetAsync(collectionName, key).Result);
		}

		public Result Get(string collectionName, string key, string reference)
		{
			return AggregateExceptionUnpacker.Unwrap(() => GetAsync(collectionName, key, reference).Result);
		}

		public Result Post(string collectionName, string item)
		{
			return AggregateExceptionUnpacker.Unwrap(() => PostAsync(collectionName, item).Result);
		}

		public Result Post(string collectionName, object item)
		{
			return AggregateExceptionUnpacker.Unwrap(() => PostAsync(collectionName, item).Result);
		}

		public Result Put(string collectionName, string key, string item)
		{
			return AggregateExceptionUnpacker.Unwrap(() => PutAsync(collectionName, key, item).Result);
		}

		public Result Put(string collectionName, string key, object item)
		{
			return AggregateExceptionUnpacker.Unwrap(() => PutAsync(collectionName, key, item).Result);
		}

		public Result PutIfMatch(string collectionName, string key, string item, string ifMatch)
		{
			return AggregateExceptionUnpacker.Unwrap(() => PutIfMatchAsync(collectionName, key, item, ifMatch).Result);
		}

		public Result PutIfMatch(string collectionName, string key, object item, string ifMatch)
		{
			return AggregateExceptionUnpacker.Unwrap(() => PutIfMatchAsync(collectionName, key, item, ifMatch).Result);
		}

		public Result PutIfNoneMatch(string collectionName, string key, string item)
		{
			return AggregateExceptionUnpacker.Unwrap(() => PutIfNoneMatchAsync(collectionName, key, item).Result);
		}

		public Result PutIfNoneMatch(string collectionName, string key, object item)
		{
			return AggregateExceptionUnpacker.Unwrap(() => PutIfNoneMatchAsync(collectionName, key, item).Result);
		}

		public Result Delete(string collectionName, string key, bool purge)
		{
			return AggregateExceptionUnpacker.Unwrap(() => DeleteAsync(collectionName, key, purge).Result);
		}

		public ListResult List(string collectionName, int limit, string startKey, string afterKey)
		{
			return AggregateExceptionUnpacker.Unwrap(() => ListAsync(collectionName, limit, startKey,afterKey).Result);
		}

		public Result DeleteIfMatch(string collectionName, string key, string ifMatch, bool purge)
		{
			return AggregateExceptionUnpacker.Unwrap(() => DeleteIfMatchAsync(collectionName, key, ifMatch, purge).Result);
		}

		public async Task<Result> CreateCollectionAsync(string collectionName, string key, object item)
		{
			if (item == null)
				throw new ArgumentNullException("item", "item cannot be null");

			var json = JsonConvert.SerializeObject(item);
			return await CreateCollectionAsync(collectionName, key, json);
		}

		public async Task<Result> CreateCollectionAsync(string collectionName, string key, string item)
		{
			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key", "key cannot be null or empty");

			if (string.IsNullOrEmpty(item))
				throw new ArgumentNullException("item", "item cannot be null or empty");

			var url = collectionName + "/" + key;
			var baseResult = await _communication.CallWebRequestAsync(url, "PUT", item);

			return new Result
			{
				Path = new OrchestratePath(collectionName, key, baseResult.ETag),
				Score = 1,
				Value = baseResult.Payload
			};
		}

		public async Task<Result> DeleteCollectionAsync(string collectionName)
		{
			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

			var url = collectionName + "?force=true";
			var baseResult = await _communication.CallWebRequestAsync(url, "DELETE", null);

			return new Result
			{
				Path = new OrchestratePath(collectionName, string.Empty, baseResult.ETag),
				Score = 1,
				Value = baseResult.Payload
			};
		}

		public async Task<Result> GetAsync(string collectionName, string key)
		{
			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key", "key cannot be null or empty");

			var url = collectionName + "/" + key;
			var baseResult = await _communication.CallWebRequestAsync(url, "GET", null);

			return new Result
			{
				Path = new OrchestratePath(collectionName, key, baseResult.ETag),
				Score = 1,
				Value = baseResult.Payload
			};
		}

		public async Task<Result> GetAsync(string collectionName, string key, string reference)
		{
			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key", "key cannot be null or empty");

			if (string.IsNullOrEmpty(reference))
				throw new ArgumentNullException("reference", "reference cannot be null or empty");

			var url = collectionName + "/" + key + "/refs/" + reference;
			var baseResult = await _communication.CallWebRequestAsync(url, "GET", null);

			return new Result
			{
				Path = new OrchestratePath(collectionName, key, baseResult.ETag),
				Score = 1,
				Value = baseResult.Payload
			};
		}

		public async Task<Result> PostAsync(string collectionName, object item)
		{
			if (item == null)
				throw new ArgumentNullException("item", "item cannot be null");

			var json = JsonConvert.SerializeObject(item);
			return await PostAsync(collectionName, json);
		}

		public async Task<Result> PostAsync(string collectionName, string item)
		{
			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

			if (string.IsNullOrEmpty(item))
				throw new ArgumentNullException("item", "item cannot be empty");

			var url = collectionName;
			var baseResult = await _communication.CallWebRequestAsync(url, "POST", item);

			var key = ExtractKeyFromLocation(baseResult);

			return new Result
			{
				Path = new OrchestratePath(collectionName, key, baseResult.ETag),
				Score = 1,
				Value = baseResult.Payload
			};
		}

		public async Task<Result> PutAsync(string collectionName, string key, object item)
		{
			if (item == null)
				throw new ArgumentNullException("item", "item cannot be null");

			var json = JsonConvert.SerializeObject(item);
			return await PutAsync(collectionName, key, json);
		}

		public async Task<Result> PutAsync(string collectionName, string key, string item)
		{
			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key", "key cannot be null or empty");

			if (string.IsNullOrEmpty(item))
				throw new ArgumentNullException("item", "item cannot be empty");

			var url = collectionName + "/" + key;
			var baseResult = await _communication.CallWebRequestAsync(url, "PUT", item);

			return new Result
			{
				Path = new OrchestratePath(collectionName, key, baseResult.ETag),
				Score = 1,
				Value = baseResult.Payload
			};
		}

		public async Task<Result> PutIfMatchAsync(string collectionName, string key, object item, string ifMatch)
		{
			if (item == null)
				throw new ArgumentNullException("item", "item cannot be null");

			var json = JsonConvert.SerializeObject(item);
			return await PutIfMatchAsync(collectionName, key, json, ifMatch);
		}

		public async Task<Result> PutIfMatchAsync(string collectionName, string key, string item, string ifMatch)
		{
			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key", "key cannot be null or empty");

			if (string.IsNullOrEmpty(item))
				throw new ArgumentNullException("item", "json cannot be empty");

			if (string.IsNullOrEmpty(ifMatch))
				throw new ArgumentNullException("ifMatch", "ifMatch cannot be empty");

			var url = collectionName + "/" + key;
			var baseResult = await _communication.CallWebRequestAsync(url, "PUT", item, ifMatch);

			return new Result
			{
				Path = new OrchestratePath(collectionName, key, baseResult.ETag),
				Score = 1,
				Value = baseResult.Payload
			};
		}

		public async Task<Result> PutIfNoneMatchAsync(string collectionName, string key, object item)
		{
			if (item == null)
				throw new ArgumentNullException("item", "item cannot be null");

			var json = JsonConvert.SerializeObject(item);
			return await PutIfNoneMatchAsync(collectionName, key, json);
		}

		public async Task<Result> PutIfNoneMatchAsync(string collectionName, string key, string item)
		{
			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key", "key cannot be null or empty");

			if (string.IsNullOrEmpty(item))
				throw new ArgumentNullException("item", "item cannot be empty");

			var url = collectionName + "/" + key;
			var baseResult = await _communication.CallWebRequestAsync(url, "PUT", item, null, true);

			return new Result
			{
				Path = new OrchestratePath(collectionName, key, baseResult.ETag),
				Score = 1,
				Value = baseResult.Payload
			};
		}

		public async Task<Result> DeleteAsync(string collectionName, string key, bool purge)
		{
			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key", "key cannot be null or empty");

			var url = collectionName + "/" + key;

			if (purge)
				url += "?purge=true";
			else
				url += "?purge=false";

			var baseResult = await _communication.CallWebRequestAsync(url, "DELETE", null);

			return new Result
			{
				Path = new OrchestratePath(collectionName, key, baseResult.ETag),
				Score = 1,
				Value = baseResult.Payload
			};
		}

		public async Task<Result> DeleteIfMatchAsync(string collectionName, string key, string ifMatch, bool purge)
		{
			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException("key", "key cannot be null or empty");

			if (string.IsNullOrEmpty(ifMatch))
				throw new ArgumentNullException("ifMatch", "ifMatch cannot be null or empty");

			var url = collectionName + "/" + key;

			if (purge)
				url += "?purge=true";
			else
				url += "?purge=false";

			var baseResult = await _communication.CallWebRequestAsync(url, "DELETE", null, ifMatch);

			return new Result
			{
				Path = new OrchestratePath(collectionName, key, baseResult.ETag),
				Score = 1,
				Value = baseResult.Payload
			};
		}

		public async Task<ListResult> ListAsync(string collectionName, int limit, string startKey, string afterKey)
		{
			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

			if (limit < 1 || limit > 100)
				throw new ArgumentOutOfRangeException("limit", "limit must be between 1 and 100");

			if (!string.IsNullOrEmpty(startKey) && !string.IsNullOrEmpty(afterKey))
				throw new ArgumentException("May only specify either a startKey or an afterKey", "startKey");

			var url = collectionName + "?limit=" + limit;

			if (!string.IsNullOrEmpty(startKey))
				url += "&startKey=" + startKey;

			if (!string.IsNullOrEmpty(afterKey))
				url += "&afterKey=" + afterKey;

			var result = await _communication.CallWebRequestAsync(url, "GET", null);

			return JsonConvert.DeserializeObject<ListResult>(result.Payload);
		}

		private static string ExtractKeyFromLocation(BaseResult baseResult)
		{
			// Always in the format /v0/<collection>/<key>/refs/<ref>
			// <ref> is included in BaseResult
			var fragments = baseResult.Location.Split('/');
			int index = Array.IndexOf(fragments, "refs");
			return fragments[index + 1];
		}
	}
}