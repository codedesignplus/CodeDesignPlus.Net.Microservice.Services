using CodeDesignPlus.Net.Microservice.Services.Domain.Entities;
using CodeDesignPlus.Net.Microservice.Services.Domain.Enums;

namespace CodeDesignPlus.Net.Microservice.Services.Domain;

public class ServiceAggregate(Guid id) : AggregateRootBase(id)
{
    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public List<ControllerEntity> Controllers { get; private set; } = [];

    private ServiceAggregate(Guid id, string name, string description, Guid createdBy) : this(id)
    {
        Name = name;
        Description = description;
        IsActive = true;
        CreatedBy = createdBy;
        CreatedAt = SystemClock.Instance.GetCurrentInstant();

        AddEvent(ServiceCreatedDomainEvent.Create(Id, Name, Description, IsActive));
    }

    public static ServiceAggregate Create(Guid id, string name, string description, Guid createdBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.InvalidId);
        DomainGuard.IsNullOrEmpty(name, Errors.InvalidName);
        DomainGuard.IsNullOrEmpty(description, Errors.InvalidDescription);

        return new ServiceAggregate(id, name, description, createdBy);
    }

    public void Update(string name, string description, bool isActive, Guid updatedBy)
    {
        DomainGuard.IsNullOrEmpty(name, Errors.InvalidName);
        DomainGuard.IsNullOrEmpty(description, Errors.InvalidDescription);

        Name = name;
        Description = description;
        IsActive = isActive;
        UpdatedBy = updatedBy;
        UpdatedAt = SystemClock.Instance.GetCurrentInstant();

        AddEvent(ServiceUpdatedDomainEvent.Create(Id, Name, Description, IsActive));
    }

    public void AddController(Guid id, string name, string description, Guid updatedBy)
    {
        DomainGuard.GuidIsEmpty(id, Errors.InvalidControllerId);
        DomainGuard.IsNullOrEmpty(name, Errors.InvalidControllerName);
        DomainGuard.IsNullOrEmpty(description, Errors.InvalidControllerDescription);
        DomainGuard.GuidIsEmpty(updatedBy, Errors.InvalidUpdatedBy);

        UpdatedBy = updatedBy;
        UpdatedAt = SystemClock.Instance.GetCurrentInstant();

        var controller = new ControllerEntity()
        {
            Id = id,
            Name = name,
            Description = description
        };

        Controllers.Add(controller);

        AddEvent(ControllerAddedDomainEvent.Create(Id, controller.Id, controller.Name, controller.Description));
    }

    public void UpdateController(Guid idController, string name, string description, Guid updatedBy)
    {
        DomainGuard.GuidIsEmpty(idController, Errors.InvalidControllerId);
        DomainGuard.IsNullOrEmpty(name, Errors.InvalidControllerName);
        DomainGuard.IsNullOrEmpty(description, Errors.InvalidControllerDescription);
        DomainGuard.GuidIsEmpty(updatedBy, Errors.InvalidUpdatedBy);

        var controller = Controllers.FirstOrDefault(x => x.Id == idController);

        DomainGuard.IsNull(controller, Errors.ControllerNotFound);

        UpdatedBy = updatedBy;
        UpdatedAt = SystemClock.Instance.GetCurrentInstant();

        controller.Name = name;
        controller.Description = description;

        AddEvent(ControllerUpdatedDomainEvent.Create(Id, controller.Id, controller.Name, controller.Description));
    }

    public void RemoveController(Guid idController, Guid removedBy)
    {
        DomainGuard.GuidIsEmpty(idController, Errors.InvalidControllerId);

        var controller = Controllers.FirstOrDefault(x => x.Id == idController);

        DomainGuard.IsNull(controller, Errors.ControllerNotFound);

        UpdatedBy = removedBy;
        UpdatedAt = SystemClock.Instance.GetCurrentInstant();

        Controllers.Remove(controller);

        AddEvent(ControllerRemovedDomainEvent.Create(Id, controller.Id, controller.Name, controller.Description));
    }

    public void AddAction(Guid idController, Guid idAction, string name, string description, HttpMethodEnum httpMethod, Guid updatedBy)
    {
        DomainGuard.GuidIsEmpty(idController, Errors.InvalidControllerId);
        DomainGuard.GuidIsEmpty(idAction, Errors.InvalidActionId);
        DomainGuard.IsNullOrEmpty(Name, Errors.InvalidActionName);
        DomainGuard.IsNullOrEmpty(Description, Errors.InvalidActionDescription);

        var controller = Controllers.FirstOrDefault(x => x.Id == idController);

        DomainGuard.IsNull(controller, Errors.ControllerNotFound);

        UpdatedBy = updatedBy;
        UpdatedAt = SystemClock.Instance.GetCurrentInstant();

        var action = new ActionEntity()
        {
            Id = idAction,
            Name = name,
            Description = description,
            HttpMethod = httpMethod
        };

        controller.Actions.Add(action);

        AddEvent(ActionAddedDomainEvent.Create(Id, controller.Id, action.Id, action.Name, action.Description, action.HttpMethod));
    }

    public void UpdateAction(Guid controllerId, Guid actionId, string name, string description, HttpMethodEnum httpMethod, Guid updatedBy)
    {
        DomainGuard.GuidIsEmpty(controllerId, Errors.InvalidControllerId);
        DomainGuard.GuidIsEmpty(actionId, Errors.InvalidActionId);
        DomainGuard.IsNullOrEmpty(name, Errors.InvalidActionName);
        DomainGuard.IsNullOrEmpty(description, Errors.InvalidActionDescription);

        var controller = Controllers.FirstOrDefault(x => x.Id == controllerId);

        DomainGuard.IsNull(controller, Errors.ControllerNotFound);

        var action = controller.Actions.FirstOrDefault(x => x.Id == actionId);

        DomainGuard.IsNull(action, Errors.ActionNotFound);
        
        UpdatedBy = updatedBy;
        UpdatedAt = SystemClock.Instance.GetCurrentInstant();

        action.Name = name;
        action.Description = description;
        action.HttpMethod = httpMethod;

        AddEvent(ActionUpdatedDomainEvent.Create(Id, controller.Id, action.Id, action.Name, action.Description, action.HttpMethod));
    }

    public void RemoveAction(Guid controllerId, Guid actionId, Guid removedBy)
    {
        DomainGuard.GuidIsEmpty(controllerId, Errors.InvalidControllerId);
        DomainGuard.GuidIsEmpty(actionId, Errors.InvalidActionId);

        var controller = Controllers.FirstOrDefault(x => x.Id == controllerId);

        DomainGuard.IsNull(controller, Errors.ControllerNotFound);

        var action = controller.Actions.FirstOrDefault(x => x.Id == actionId);

        DomainGuard.IsNull(action, Errors.ActionNotFound);
        
        UpdatedBy = removedBy;
        UpdatedAt = SystemClock.Instance.GetCurrentInstant();

        controller.Actions.Remove(action);

        AddEvent(ActionRemovedDomainEvent.Create(Id, controller.Id, action.Id, action.Name, action.Description, action.HttpMethod));
    }

    public void Remove(Guid removedBy)
    {
        IsActive = false;
        UpdatedBy = removedBy;
        UpdatedAt = SystemClock.Instance.GetCurrentInstant();

        AddEvent(ServiceDeletedDomainEvent.Create(Id, Name, Description, IsActive));
    }
}
