namespace StockFlow.Common.Exceptions;
public class ResourceNotFoundException(string message = "Resource") :
    Exception($"{message} Not Found")
{
}