namespace iHome.Core.Exceptions;

public abstract class SqlException : Exception
{
    public abstract string SpecialMessage { get; }
}
