namespace CodeDesignPlus.Net.Microservice.Services.Domain;

public class Errors: IErrorCodes
{    
    public const string UnknownError = "100 : UnknownError";
    public const string InvalidId = "101 : Invalid Id";
    public const string InvalidName = "102 : Invalid Name"; 
    public const string InvalidDescription = "103 : Invalid Description"; 
    public const string InvalidControllerName = "104 : Invalid Controller Name"; 
    public const string InvalidControllerDescription = "105 : Invalid Controller Description"; 
    public const string InvalidControllerId = "106 : Invalid Controller Id"; 
    public const string ControllerNotFound = "107 : Controller Not Found"; 
    public const string InvalidUpdatedBy = "108 : Invalid Updated By"; 
    public const string InvalidActionName = "109 : Invalid Action Name"; 
    public const string InvalidActionDescription = "110 : Invalid Action Description"; 
    public const string InvalidActionId = "111 : Invalid Action Id"; 
    public const string ActionNotFound = "112 : Action Not Found"; 
}
