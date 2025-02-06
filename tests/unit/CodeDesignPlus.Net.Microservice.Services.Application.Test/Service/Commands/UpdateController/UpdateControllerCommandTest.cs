using System;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateController;
using FluentValidation.TestHelper;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Commands.UpdateController;

public class UpdateControllerCommandTest
{
    private readonly Validator _validator;

    public UpdateControllerCommandTest()
    {
        _validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_IdService_Is_Empty()
    {
        var command = new UpdateControllerCommand(Guid.Empty, Guid.NewGuid(), "ValidName", "ValidDescription");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdService);
    }

    [Fact]
    public void Should_Have_Error_When_IdController_Is_Empty()
    {
        var command = new UpdateControllerCommand(Guid.NewGuid(), Guid.Empty, "ValidName", "ValidDescription");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdController);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new UpdateControllerCommand(Guid.NewGuid(), Guid.NewGuid(), string.Empty, "ValidDescription");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        var command = new UpdateControllerCommand(Guid.NewGuid(), Guid.NewGuid(), new string('a', 129), "ValidDescription");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var command = new UpdateControllerCommand(Guid.NewGuid(), Guid.NewGuid(), "ValidName", string.Empty);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Exceeds_MaxLength()
    {
        var command = new UpdateControllerCommand(Guid.NewGuid(), Guid.NewGuid(), "ValidName", new string('a', 513));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new UpdateControllerCommand(Guid.NewGuid(), Guid.NewGuid(), "ValidName", "ValidDescription");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
