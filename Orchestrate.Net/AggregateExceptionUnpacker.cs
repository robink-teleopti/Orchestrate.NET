using System;

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
}