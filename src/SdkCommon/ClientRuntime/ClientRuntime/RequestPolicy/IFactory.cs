namespace Microsoft.Rest.ClientRuntime.RequestPolicy
{
    // The Factory interface represents an object that can create its Policy object. Each HTTP request sent
    // requires that this Factory create a new instance of its Policy object.
    public interface IFactory
    {
        IPolicy Create(PolicyNode node);
    }
}
