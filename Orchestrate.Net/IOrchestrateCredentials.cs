namespace Orchestrate.Net
{
	public interface IOrchestrateCredentials
	{
		string ApiKey { get; }
		string PrependHost(string url);
	}
}