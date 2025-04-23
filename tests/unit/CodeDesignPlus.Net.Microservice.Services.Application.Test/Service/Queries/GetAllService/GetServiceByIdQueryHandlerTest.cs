using System;
using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;
using CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetAllService;

namespace CodeDesignPlus.Net.Microservice.Services.Application.Test.Service.Queries.GetAllService;


public class GetAllServiceQueryHandlerTest
{
    private readonly Mock<IServiceRepository> repositoryMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly GetAllServiceQueryHandler handler;

    public GetAllServiceQueryHandlerTest()
    {
        repositoryMock = new Mock<IServiceRepository>();
        mapperMock = new Mock<IMapper>();
        handler = new GetAllServiceQueryHandler(repositoryMock.Object, mapperMock.Object);
    }

    [Fact]
    public async Task Handle_RequestIsNull_ThrowsInvalidRequestException()
    {
        // Arrange
        GetAllServiceQuery request = null!;
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, cancellationToken));

        Assert.Equal(Errors.InvalidRequest.GetCode(), exception.Code);
        Assert.Equal(Errors.InvalidRequest.GetMessage(), exception.Message);
        Assert.Equal(Layer.Application, exception.Layer);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsServiceDtoList()
    {
        // Arrange
        var idUser = Guid.NewGuid();
        var criteria = new C.Criteria();
        var request = new GetAllServiceQuery(criteria);
        var cancellationToken = CancellationToken.None;

        var service1 = ServiceAggregate.Create(Guid.NewGuid(), "TestService", "Test Description", idUser);
        var service2 = ServiceAggregate.Create(Guid.NewGuid(), "TestService", "Test Description", idUser);

        var serviceDto1 = new ServiceDto()
        {
            Id = service1.Id,
            Name = service1.Name,
            Description = service1.Description
        };
        var serviceDto2 = new ServiceDto()
        {
            Id = service2.Id,
            Name = service2.Name,
            Description = service2.Description
        };

        var serviceAggregates = new Pagination<ServiceAggregate>([service1, service2], 2, 10, 0);
        var serviceDtos = new Pagination<ServiceDto>([serviceDto1, serviceDto2], 2, 10, 0);

        repositoryMock.Setup(repo => repo.MatchingAsync<ServiceAggregate>(request.Criteria, cancellationToken))
            .ReturnsAsync(serviceAggregates);

        mapperMock.Setup(mapper => mapper.Map<Pagination<ServiceDto>>(serviceAggregates))
            .Returns(serviceDtos);

        // Act
        var result = await handler.Handle(request, cancellationToken);

        // Assert
        foreach (var serviceDto in result.Data)
        {
            var itemAggregate = serviceAggregates.Data.FirstOrDefault(x => x.Id == serviceDto.Id);

            Assert.NotNull(itemAggregate);
            Assert.Equal(serviceDto.Name, itemAggregate.Name);
            Assert.Equal(serviceDto.Description, itemAggregate.Description);
        }
    }
}
