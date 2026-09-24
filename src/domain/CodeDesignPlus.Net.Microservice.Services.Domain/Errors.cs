using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Services.Domain;

public class Errors: IErrorCodes
{    
    public static readonly Error UnknownError = new("100", "UnknownError");
    public static readonly Error InvalidId = new("101", "Invalid Id");
    public static readonly Error InvalidName = new("102", "Invalid Name"); 
    public static readonly Error InvalidDescription = new("103", "Invalid Description"); 
    public static readonly Error InvalidControllerName = new("104", "Invalid Controller Name"); 
    public static readonly Error InvalidControllerDescription = new("105", "Invalid Controller Description"); 
    public static readonly Error InvalidControllerId = new("106", "Invalid Controller Id"); 
    public static readonly Error ControllerNotFound = new("107", "Controller Not Found"); 
    public static readonly Error InvalidUpdatedBy = new("108", "Invalid Updated By"); 
    public static readonly Error InvalidActionName = new("109", "Invalid Action Name"); 
    public static readonly Error InvalidActionDescription = new("110", "Invalid Action Description"); 
    public static readonly Error InvalidActionId = new("111", "Invalid Action Id"); 
    public static readonly Error ActionNotFound = new("112", "Action Not Found"); 
}
