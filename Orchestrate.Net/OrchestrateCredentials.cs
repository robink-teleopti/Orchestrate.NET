namespace Orchestrate.Net
{
	public class OrchestrateCredentials : IOrchestrateCredentials
	{
		public OrchestrateCredentials(string apiKey, string host = "https://api.orchestrate.io/")
		{
			ApiKey = apiKey;
			Host = host + "v0/";
		}

		public string ApiKey { get; private set; }
		public string Host { get; private set; }
	}
}