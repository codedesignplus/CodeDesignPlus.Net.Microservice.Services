namespace CodeDesignPlus.Net.Microservice.Services.Application;

public class Errors: IErrorCodes
{    
    public const string UnknownError = "200 : UnknownError";
    public const string InvalidRequest = "201 : The request is invalid";
    public const string ServiceAlreadyExists = "202 : The service already exists";
    public const string ServiceNotFound = "203 : The service was not found"; 
}
