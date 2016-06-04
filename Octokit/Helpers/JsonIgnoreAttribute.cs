using System;

namespace Octokit.Internal
{
    /// <summary>
    /// Indicate to the serializer that this property or value 
    /// should not be serialized to JSON
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class JsonIgnoreAttribute : Attribute
    {

    }
}