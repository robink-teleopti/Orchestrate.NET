using System.Threading.Tasks;

namespace Orchestrate.Net
{
	public interface ICommunication
	{
		BaseResult CallWebRequest(string url, string method, string jsonPayload, string ifMatch = null, bool ifNoneMatch = false);
		Task<BaseResult> CallWebRequestAsync(string url, string method, string jsonPayload, string ifMatch = null, bool ifNoneMatch = false);
	}
}