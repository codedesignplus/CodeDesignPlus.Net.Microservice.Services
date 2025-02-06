using System;
using System.Collections.Generic;
using Xunit;
using CodeDesignPlus.Net.Microservice.Services.Domain;
using CodeDesignPlus.Net.Microservice.Services.Domain.Entities;
using CodeDesignPlus.Net.Microservice.Services.Domain.Enums;

namespace CodeDesignPlus.Net.Microservice.Services.Domain.Test;

public class ServiceAggregateTest
{
    [Fact]
    public void Create_ValidParameters_ReturnsServiceAggregate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Service";
        var description = "Test Description";
        var createdBy = Guid.NewGuid();

        // Act
        var serviceAggregate = ServiceAggregate.Create(id, name, description, createdBy);

        // Assert
        Assert.NotNull(serviceAggregate);
        Assert.Equal(id, serviceAggregate.Id);
        Assert.Equal(name, serviceAggregate.Name);
        Assert.Equal(description, serviceAggregate.Description);
        Assert.True(serviceAggregate.IsActive);
        Assert.Equal(createdBy, serviceAggregate.CreatedBy);
    }

    [Fact]
    public void Update_ValidParameters_UpdatesServiceAggregate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Service";
        var description = "Test Description";
        var createdBy = Guid.NewGuid();
        var serviceAggregate = ServiceAggregate.Create(id, name, description, createdBy);

        var newName = "Updated Service";
        var newDescription = "Updated Description";
        var isActive = false;
        var updatedBy = Guid.NewGuid();

        // Act
        serviceAggregate.Update(newName, newDescription, isActive, updatedBy);

        // Assert
        Assert.Equal(newName, serviceAggregate.Name);
        Assert.Equal(newDescription, serviceAggregate.Description);
        Assert.False(serviceAggregate.IsActive);
        Assert.Equal(updatedBy, serviceAggregate.UpdatedBy);
    }

    [Fact]
    public void AddController_ValidParameters_AddsController()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Service";
        var description = "Test Description";
        var createdBy = Guid.NewGuid();
        var serviceAggregate = ServiceAggregate.Create(id, name, description, createdBy);

        var controllerId = Guid.NewGuid();
        var controllerName = "Test Controller";
        var controllerDescription = "Test Controller Description";
        var updatedBy = Guid.NewGuid();

        // Act
        serviceAggregate.AddController(controllerId, controllerName, controllerDescription, updatedBy);

        // Assert
        Assert.Single(serviceAggregate.Controllers);
        var controller = serviceAggregate.Controllers[0];
        Assert.Equal(controllerId, controller.Id);
        Assert.Equal(controllerName, controller.Name);
        Assert.Equal(controllerDescription, controller.Description);
    }

    [Fact]
    public void UpdateController_ValidParameters_UpdatesController()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Service";
        var description = "Test Description";
        var createdBy = Guid.NewGuid();
        var serviceAggregate = ServiceAggregate.Create(id, name, description, createdBy);

        var controllerId = Guid.NewGuid();
        var controllerName = "Test Controller";
        var controllerDescription = "Test Controller Description";
        var updatedBy = Guid.NewGuid();
        serviceAggregate.AddController(controllerId, controllerName, controllerDescription, updatedBy);

        var newControllerName = "Updated Controller";
        var newControllerDescription = "Updated Controller Description";

        // Act
        serviceAggregate.UpdateController(controllerId, newControllerName, newControllerDescription, updatedBy);

        // Assert
        var controller = serviceAggregate.Controllers[0];
        Assert.Equal(newControllerName, controller.Name);
        Assert.Equal(newControllerDescription, controller.Description);
    }

    [Fact]
    public void RemoveController_ValidParameters_RemovesController()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Service";
        var description = "Test Description";
        var createdBy = Guid.NewGuid();
        var serviceAggregate = ServiceAggregate.Create(id, name, description, createdBy);

        var controllerId = Guid.NewGuid();
        var controllerName = "Test Controller";
        var controllerDescription = "Test Controller Description";
        var updatedBy = Guid.NewGuid();
        serviceAggregate.AddController(controllerId, controllerName, controllerDescription, updatedBy);

        // Act
        serviceAggregate.RemoveController(controllerId, updatedBy);

        // Assert
        Assert.Empty(serviceAggregate.Controllers);
    }

    [Fact]
    public void AddAction_ValidParameters_AddsAction()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Service";
        var description = "Test Description";
        var createdBy = Guid.NewGuid();
        var serviceAggregate = ServiceAggregate.Create(id, name, description, createdBy);

        var controllerId = Guid.NewGuid();
        var controllerName = "Test Controller";
        var controllerDescription = "Test Controller Description";
        var updatedBy = Guid.NewGuid();
        serviceAggregate.AddController(controllerId, controllerName, controllerDescription, updatedBy);

        var actionId = Guid.NewGuid();
        var actionName = "Test Action";
        var actionDescription = "Test Action Description";
        var httpMethod = Enums.HttpMethod.GET;

        // Act
        serviceAggregate.AddAction(controllerId, actionId, actionName, actionDescription, httpMethod, updatedBy);

        // Assert
        var controller = serviceAggregate.Controllers[0];
        Assert.Single(controller.Actions);
        var action = controller.Actions[0];
        Assert.Equal(actionId, action.Id);
        Assert.Equal(actionName, action.Name);
        Assert.Equal(actionDescription, action.Description);
        Assert.Equal(httpMethod, action.HttpMethod);
    }

    [Fact]
    public void UpdateAction_ValidParameters_UpdatesAction()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Service";
        var description = "Test Description";
        var createdBy = Guid.NewGuid();
        var serviceAggregate = ServiceAggregate.Create(id, name, description, createdBy);

        var controllerId = Guid.NewGuid();
        var controllerName = "Test Controller";
        var controllerDescription = "Test Controller Description";
        var updatedBy = Guid.NewGuid();
        serviceAggregate.AddController(controllerId, controllerName, controllerDescription, updatedBy);

        var actionId = Guid.NewGuid();
        var actionName = "Test Action";
        var actionDescription = "Test Action Description";
        var httpMethod = Enums.HttpMethod.GET;
        serviceAggregate.AddAction(controllerId, actionId, actionName, actionDescription, httpMethod, updatedBy);

        var newActionName = "Updated Action";
        var newActionDescription = "Updated Action Description";
        var newHttpMethod = Enums.HttpMethod.POST;

        // Act
        serviceAggregate.UpdateAction(controllerId, actionId, newActionName, newActionDescription, newHttpMethod, updatedBy);

        // Assert
        var controller = serviceAggregate.Controllers[0];
        var action = controller.Actions[0];
        Assert.Equal(newActionName, action.Name);
        Assert.Equal(newActionDescription, action.Description);
        Assert.Equal(newHttpMethod, action.HttpMethod);
    }

    [Fact]
    public void RemoveAction_ValidParameters_RemovesAction()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Service";
        var description = "Test Description";
        var createdBy = Guid.NewGuid();
        var serviceAggregate = ServiceAggregate.Create(id, name, description, createdBy);

        var controllerId = Guid.NewGuid();
        var controllerName = "Test Controller";
        var controllerDescription = "Test Controller Description";
        var updatedBy = Guid.NewGuid();
        serviceAggregate.AddController(controllerId, controllerName, controllerDescription, updatedBy);

        var actionId = Guid.NewGuid();
        var actionName = "Test Action";
        var actionDescription = "Test Action Description";
        var httpMethod = Enums.HttpMethod.GET;
        serviceAggregate.AddAction(controllerId, actionId, actionName, actionDescription, httpMethod, updatedBy);

        // Act
        serviceAggregate.RemoveAction(controllerId, actionId, updatedBy);

        // Assert
        var controller = serviceAggregate.Controllers[0];
        Assert.Empty(controller.Actions);
    }

    [Fact]
    public void Delete_ValidParameters_DeletesService()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Service";
        var description = "Test Description";
        var createdBy = Guid.NewGuid();
        var serviceAggregate = ServiceAggregate.Create(id, name, description, createdBy);

        var removedBy = Guid.NewGuid();

        // Act
        serviceAggregate.Delete(removedBy);

        // Assert
        Assert.False(serviceAggregate.IsActive);
        Assert.Equal(removedBy, serviceAggregate.UpdatedBy);
    }
}
