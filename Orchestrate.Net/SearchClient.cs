using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Orchestrate.Net
{
	public class SearchClient
	{
		private readonly ICommunication _communication;

		public SearchClient(ICommunication communication)
		{
			_communication = communication;
		}

		public SearchResult Search(string collectionName, string query, int limit = 10, int offset = 0)
		{
			try
			{
				return SearchAsync(collectionName, query, limit, offset).Result;
			}
			catch (AggregateException aggregateException)
			{
				throw aggregateException.InnerException;
			}
		}

		public async Task<SearchResult> SearchAsync(string collectionName, string query, int limit = 10, int offset = 0)
		{
			if (string.IsNullOrEmpty(collectionName))
				throw new ArgumentNullException("collectionName", "collectionName cannot be null or empty");

			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException("query", "query cannot be null or empty");

			if (limit < 1 || limit > 100)
				throw new ArgumentOutOfRangeException("limit", "limit must be between 1 and 100");

			if (offset < 0)
				throw new ArgumentOutOfRangeException("offset", "offset must be at least 0");

			var url = collectionName + "?query=" + query + "&limit=" + limit + "&offset=" + offset;
			var result = await _communication.CallWebRequestAsync(url, "GET", null);

			return JsonConvert.DeserializeObject<SearchResult>(result.Payload);
		}
	}
}