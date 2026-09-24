using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Services.Application;

public class Errors: IErrorCodes
{    
    public static readonly Error UnknownError = new("200", "UnknownError");
    public static readonly Error InvalidRequest = new("201", "The request is invalid");
    public static readonly Error ServiceAlreadyExists = new("202", "The service already exists");
    public static readonly Error ServiceNotFound = new("203", "The service was not found"); 
}
