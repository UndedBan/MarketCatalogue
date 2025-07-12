namespace MarketCatalogue.Presentation.Exceptions;

public class UserWasNotFoundException : Exception
{
    public UserWasNotFoundException(string message) : base(message) { }
}
