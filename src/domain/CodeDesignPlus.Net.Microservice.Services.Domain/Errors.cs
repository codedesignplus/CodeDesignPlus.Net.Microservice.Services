using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Services.Domain;

public class Errors: IErrorCodes
{    
    public static readonly Error UnknownError = new("100");
    public static readonly Error InvalidId = new("101");
    public static readonly Error InvalidName = new("102"); 
    public static readonly Error InvalidDescription = new("103"); 
    public static readonly Error InvalidControllerName = new("104"); 
    public static readonly Error InvalidControllerDescription = new("105"); 
    public static readonly Error InvalidControllerId = new("106"); 
    public static readonly Error ControllerNotFound = new("107"); 
    public static readonly Error InvalidUpdatedBy = new("108"); 
    public static readonly Error InvalidActionName = new("109"); 
    public static readonly Error InvalidActionDescription = new("110"); 
    public static readonly Error InvalidActionId = new("111"); 
    public static readonly Error ActionNotFound = new("112"); 
}
