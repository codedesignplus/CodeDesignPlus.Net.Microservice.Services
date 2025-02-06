using System;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.RemoveController;
using FluentValidation.TestHelper;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Commands.RemoveController;

public class RemoveControllerCommandTest
{
    private readonly Validator validator;

    public RemoveControllerCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_IdService_Is_Empty()
    {
        var command = new RemoveControllerCommand(Guid.Empty, Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdService);
    }

    [Fact]
    public void Should_Have_Error_When_IdController_Is_Empty()
    {
        var command = new RemoveControllerCommand(Guid.NewGuid(), Guid.Empty);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdController);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Ids_Are_Valid()
    {
        var command = new RemoveControllerCommand(Guid.NewGuid(), Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.IdService);
        result.ShouldNotHaveValidationErrorFor(x => x.IdController);
    }
}
