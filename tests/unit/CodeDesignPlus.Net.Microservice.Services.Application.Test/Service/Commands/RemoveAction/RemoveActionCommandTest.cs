
using System;
using Xunit;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.RemoveAction;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Commands.RemoveAction;

public class RemoveActionCommandTest
{
    private readonly Validator validator;

    public RemoveActionCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_IdService_Is_Empty()
    {
        var command = new RemoveActionCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdService);
    }

    [Fact]
    public void Should_Have_Error_When_IdController_Is_Empty()
    {
        var command = new RemoveActionCommand(Guid.NewGuid(), Guid.Empty, Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdController);
    }

    [Fact]
    public void Should_Have_Error_When_IdAction_Is_Empty()
    {
        var command = new RemoveActionCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdAction);
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Ids_Are_Valid()
    {
        var command = new RemoveActionCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
