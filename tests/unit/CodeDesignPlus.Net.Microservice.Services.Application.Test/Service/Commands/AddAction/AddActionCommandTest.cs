
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddAction;
using CodeDesignPlus.Net.Microservice.Services.Domain.Enums;
using FluentValidation.TestHelper;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Commands.AddAction;

public class AddActionCommandTest
{
    private readonly Validator validator;

    public AddActionCommandTest()
    {
        validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_IdService_Is_Empty()
    {
        var command = new AddActionCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), "Name", "Description", Domain.Enums.HttpMethod.GET);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdService);
    }

    [Fact]
    public void Should_Have_Error_When_IdController_Is_Empty()
    {
        var command = new AddActionCommand(Guid.NewGuid(), Guid.Empty, Guid.NewGuid(), "Name", "Description", Domain.Enums.HttpMethod.GET);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdController);
    }

    [Fact]
    public void Should_Have_Error_When_IdAction_Is_Empty()
    {
        var command = new AddActionCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty, "Name", "Description", Domain.Enums.HttpMethod.GET);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.IdAction);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new AddActionCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), string.Empty, "Description", Domain.Enums.HttpMethod.GET);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var command = new AddActionCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Name", string.Empty, Domain.Enums.HttpMethod.GET);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_Max_Length()
    {
        var command = new AddActionCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), new string('a', 129), "Description", Domain.Enums.HttpMethod.GET);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Exceeds_Max_Length()
    {
        var command = new AddActionCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Name", new string('a', 513), Domain.Enums.HttpMethod.GET);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new AddActionCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Name", "Description", Domain.Enums.HttpMethod.GET);
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
