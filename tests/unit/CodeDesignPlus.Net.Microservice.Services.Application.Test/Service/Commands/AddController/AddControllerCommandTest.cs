using System;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddController;
using FluentValidation.TestHelper;
using Xunit;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Commands.AddController;

public class AddControllerCommandTest
{
    private readonly Validator validator;

    public AddControllerCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_IdService_Is_Empty()
    {
        var command = new AddControllerCommand(Guid.Empty, Guid.NewGuid(), "Name", "Description");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdService);
    }

    [Fact]
    public void Should_Have_Error_When_IdController_Is_Empty()
    {
        var command = new AddControllerCommand(Guid.NewGuid(), Guid.Empty, "Name", "Description");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdController);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new AddControllerCommand(Guid.NewGuid(), Guid.NewGuid(), string.Empty, "Description");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var command = new AddControllerCommand(Guid.NewGuid(), Guid.NewGuid(), "Name", string.Empty);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        var command = new AddControllerCommand(Guid.NewGuid(), Guid.NewGuid(), new string('a', 129), "Description");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Exceeds_MaxLength()
    {
        var command = new AddControllerCommand(Guid.NewGuid(), Guid.NewGuid(), "Name", new string('a', 513));
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new AddControllerCommand(Guid.NewGuid(), Guid.NewGuid(), "Name", "Description");
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
