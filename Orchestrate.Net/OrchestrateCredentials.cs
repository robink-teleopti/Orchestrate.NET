namespace Orchestrate.Net
{
	public class OrchestrateCredentials : IOrchestrateCredentials
	{
		private readonly string _host;

		public OrchestrateCredentials(string apiKey, string host = "https://api.orchestrate.io/")
		{
			ApiKey = apiKey;
			_host = host + "v0/";
		}

		public string ApiKey { get; private set; }

		public string PrependHost(string url)
		{
			return _host + url;
		}
	}
}