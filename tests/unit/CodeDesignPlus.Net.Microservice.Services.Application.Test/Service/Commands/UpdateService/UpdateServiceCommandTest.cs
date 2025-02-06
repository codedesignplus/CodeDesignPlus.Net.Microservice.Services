using System;
using Xunit;
using FluentValidation.TestHelper;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateService;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Commands.UpdateService;

public class UpdateServiceCommandTest
{
    private readonly Validator _validator;

    public UpdateServiceCommandTest()
    {
        _validator = new Validator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdateServiceCommand(Guid.Empty, "ValidName", "ValidDescription", true);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new UpdateServiceCommand(Guid.NewGuid(), string.Empty, "ValidDescription", true);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        var command = new UpdateServiceCommand(Guid.NewGuid(), new string('a', 129), "ValidDescription", true);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var command = new UpdateServiceCommand(Guid.NewGuid(), "ValidName", string.Empty, true);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Exceeds_MaxLength()
    {
        var command = new UpdateServiceCommand(Guid.NewGuid(), "ValidName", new string('a', 513), true);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new UpdateServiceCommand(Guid.NewGuid(), "ValidName", "ValidDescription", true);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
