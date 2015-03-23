using System;

namespace Orchestrate.Net
{
    public struct OrchestratePath : IEquatable<OrchestratePath>
    {
	    public OrchestratePath(string collection, string key, string @ref, string ordinal = null) : this()
	    {
		    Ordinal = ordinal;
		    Ref = @ref;
		    Key = key;
		    Collection = collection;
	    }

	    public string Collection { get; private set; }
        public string Key { get; private set; }
        public string Ref { get; private set; }
		public string Ordinal { get; private set; }

	    public override bool Equals(object obj)
	    {
		    return obj is OrchestratePath && Equals((OrchestratePath) obj);
	    }

	    public bool Equals(OrchestratePath other)
	    {
		    return (GetHashCode() == other.GetHashCode());
	    }

	    public override int GetHashCode()
	    {
		    return Collection.GetHashCode() + Key.GetHashCode() + Ref.GetHashCode() + Ordinal.GetHashCode();
	    }
    }
}
