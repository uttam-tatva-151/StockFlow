namespace StockFlow.Common.Exceptions;
public class ModelValidationException : Exception
{
    public List<string>? Errors { get; set; }

    public ModelValidationException() : base("One or more validation failures have been occurred.")
    { }

    public ModelValidationException(params string[] errorList) : this()
    {
        Errors = [.. errorList];
    }

    public ModelValidationException(List<string> errors) : this()
    {
        Errors = errors;
    }
}