using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Orchestrate.Net
{
	public class Communication : ICommunication
	{
		private readonly IOrchestrateCredentials _orchestrateCredentials;

		public Communication(IOrchestrateCredentials orchestrateCredentials)
		{
			_orchestrateCredentials = orchestrateCredentials;
		}

		public BaseResult CallWebRequest(string url, string method, string jsonPayload, string ifMatch = null, bool ifNoneMatch = false)
		{
			return CallWebRequestAsync(url, method, jsonPayload, ifMatch, ifNoneMatch).Result;
		}

		public Task<BaseResult> CallWebRequestAsync(string url, string method, string jsonPayload, string ifMatch = null, bool ifNoneMatch = false)
		{
			var httpMethod = method.ToHttpMethod();
			var httpClient = new HttpClient();
			var request = new HttpRequestMessage(httpMethod, _orchestrateCredentials.PrependHost(url));

			if (jsonPayload != null && httpMethod.CanHaveContent())
			{
				request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
			}
			if (!string.IsNullOrEmpty(ifMatch))
			{
				request.Headers.Add(HttpRequestHeader.IfMatch.ToString(), ifMatch);
			}
			else if (ifNoneMatch)
			{
				request.Headers.Add(HttpRequestHeader.IfNoneMatch.ToString(), "\"*\"");
			}

			var authorization =
						Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:", _orchestrateCredentials.ApiKey)));

			request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authorization);
			return httpClient.SendAsync(request).ContinueWith<BaseResult>(BuildResult);
		}

		private static BaseResult BuildResult(Task<HttpResponseMessage> responseMessageTask)
		{
			var response = responseMessageTask.Result;
			response.EnsureSuccessStatusCode();

			var payload = response.Content.ReadAsStringAsync().Result;
			var location = (response.Headers.Location != null) ? response.Headers.Location.ToString() : string.Empty;

			var eTag = (response.Headers.ETag != null) ? response.Headers.ETag.Tag : string.Empty;

			var toReturn = new BaseResult
			{
				Location = location,
				ETag = eTag,
				Payload = payload
			};
			return toReturn;
		}
	}
}