using System;
using Xunit;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.CreateService;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Commands.CreateService;

public class CreateServiceCommandTest
{
    private readonly Validator validator;

    public CreateServiceCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new CreateServiceCommand(Guid.Empty, "ValidName", "ValidDescription");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new CreateServiceCommand(Guid.NewGuid(), string.Empty, "ValidDescription");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var command = new CreateServiceCommand(Guid.NewGuid(), "ValidName", string.Empty);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_Max_Length()
    {
        var command = new CreateServiceCommand(Guid.NewGuid(), new string('a', 129), "ValidDescription");
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Exceeds_Max_Length()
    {
        var command = new CreateServiceCommand(Guid.NewGuid(), "ValidName", new string('a', 513));
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new CreateServiceCommand(Guid.NewGuid(), "ValidName", "ValidDescription");
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
