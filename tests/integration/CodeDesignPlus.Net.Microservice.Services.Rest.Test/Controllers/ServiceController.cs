using System;

namespace CodeDesignPlus.Net.Microservice.Services.Rest.Test.Controllers;

public class ServiceControllerTest : ServerBase<Program>, IClassFixture<Server<Program>>
{
    public ServiceControllerTest(Server<Program> server) : base(server)
    {        
        server.InMemoryCollection = (x) =>
        {
            x.Add("Vault:Enable", "false");
            x.Add("Vault:Address", "http://localhost:8200");
            x.Add("Vault:Token", "root");
            x.Add("Solution", "CodeDesignPlus");
            x.Add("AppName", "my-test");
            x.Add("RabbitMQ:UserName", "guest");
            x.Add("RabbitMQ:Password", "guest");
            x.Add("Security:ValidAudiences:0", Guid.NewGuid().ToString());
        };
    }

    [Fact]
    public async Task GetServices_ReturnOk()
    {
        var service = await this.CreateServiceAsync();

        var response = await this.RequestAsync("http://localhost/api/Service", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var services = JsonSerializer.Deserialize<IEnumerable<ServiceDto>>(json);

        Assert.NotNull(services);
        Assert.NotEmpty(services);
        Assert.Contains(services, x => x.Id == service.Id);
    }

    [Fact]
    public async Task GetServiceById_ReturnOk()
    {
        var serviceCreated = await this.CreateServiceAsync();

        var response = await this.RequestAsync($"http://localhost/api/Service/{serviceCreated.Id}", null, HttpMethod.Get);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();

        var service = JsonSerializer.Deserialize<ServiceDto>(json);

        Assert.NotNull(service);
        Assert.Equal(serviceCreated.Id, service.Id);
        Assert.Equal(serviceCreated.Name, service.Name);
        Assert.Equal(serviceCreated.Description, service.Description);
    }

    [Fact]
    public async Task CreateService_ReturnNoContent()
    {
        var data = new CreateServiceDto()
        {
            Id = Guid.NewGuid(),
            Name = "Service Test",
            Description = "Service Test",
        };

        var json = JsonSerializer.Serialize(data);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync("http://localhost/api/Service",  content, HttpMethod.Post);

        var service = await this.GetRecordAsync(data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, service.Id);
        Assert.Equal(data.Name, service.Name);
        Assert.Equal(data.Description, service.Description);
    }

    [Fact]
    public async Task UpdateService_ReturnNoContent()
    {
        var serviceCreated = await this.CreateServiceAsync();

        var data = new UpdateServiceDto()
        {
            Id = serviceCreated.Id,
            Name = "Service Test Updated",
            Description = "Service Test Updated",
        };

        var json = JsonSerializer.Serialize(data);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/Service/{serviceCreated.Id}", content, HttpMethod.Put);

        var service = await this.GetRecordAsync(data.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        Assert.Equal(data.Id, service.Id);
        Assert.Equal(data.Name, service.Name);
        Assert.Equal(data.Description, service.Description);
    }

    [Fact]
    public async Task DeleteService_ReturnNoContent()
    {
        var serviceCreated = await this.CreateServiceAsync();

        var response = await this.RequestAsync($"http://localhost/api/Service/{serviceCreated.Id}", null, HttpMethod.Delete);

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task AddController_ReturnNoContent()
    {
        var serviceCreated = await this.CreateServiceAsync();

        var data = new AddControllerDto()
        {
            IdService = serviceCreated.Id,
            IdController = Guid.NewGuid(),
            Name = "Controller Test",
            Description = "Controller Test",
        };

        var json = JsonSerializer.Serialize(data);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/Service/{serviceCreated.Id}/controller", content, HttpMethod.Post);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateController_ReturnNoContent()
    {
        var serviceCreated = await this.CreateServiceAsync();

        var data = new AddControllerDto()
        {
            IdService = serviceCreated.Id,
            IdController = Guid.NewGuid(),
            Name = "Controller Test",
            Description = "Controller Test",
        };

        var json = JsonSerializer.Serialize(data);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/Service/{serviceCreated.Id}/controller", content, HttpMethod.Post);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var dataUpdate = new UpdateControllerDto()
        {
            IdService = serviceCreated.Id,
            IdController = data.IdController,
            Name = "Controller Test Updated",
            Description = "Controller Test Updated",
        };

        var jsonUpdate = JsonSerializer.Serialize(dataUpdate);

        var contentUpdate = new StringContent(jsonUpdate, Encoding.UTF8, "application/json");

        var responseUpdate = await this.RequestAsync($"http://localhost/api/Service/{dataUpdate.IdService}/controller/{dataUpdate.IdController}", contentUpdate, HttpMethod.Put);

        Assert.NotNull(responseUpdate);
        Assert.Equal(HttpStatusCode.NoContent, responseUpdate.StatusCode);
    }

    [Fact]
    public async Task RemoveController_ReturnNoContent()
    {
        var serviceCreated = await this.CreateServiceAsync();

        var data = new AddControllerDto()
        {
            IdService = serviceCreated.Id,
            IdController = Guid.NewGuid(),
            Name = "Controller Test",
            Description = "Controller Test",
        };

        var json = JsonSerializer.Serialize(data);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/Service/{serviceCreated.Id}/controller", content, HttpMethod.Post);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);


        var responseDelete = await this.RequestAsync($"http://localhost/api/Service/{serviceCreated.Id}/controller/{data.IdController}", null, HttpMethod.Delete);

        Assert.NotNull(responseDelete);
        Assert.Equal(HttpStatusCode.NoContent, responseDelete.StatusCode);
    }

    [Fact]
    public async Task AddAction_ReturnNoContent()
    {
        var serviceCreated = await this.CreateServiceAsync();

        var data = new AddControllerDto()
        {
            IdService = serviceCreated.Id,
            IdController = Guid.NewGuid(),
            Name = "Controller Test",
            Description = "Controller Test",
        };

        var json = JsonSerializer.Serialize(data);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/Service/{serviceCreated.Id}/controller", content, HttpMethod.Post);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var dataAction = new AddActionDto()
        {
            IdService = data.IdService,
            IdController = data.IdController,
            IdAction = Guid.NewGuid(),
            Name = "Action Test",
            Description = "Action Test",
        };

        var jsonAction = JsonSerializer.Serialize(dataAction);

        var contentAction = new StringContent(jsonAction, Encoding.UTF8, "application/json");

        var responseAction = await this.RequestAsync($"http://localhost/api/Service/{dataAction.IdService}/controller/{dataAction.IdController}/action", contentAction, HttpMethod.Post);

        Assert.NotNull(responseAction);
        Assert.Equal(HttpStatusCode.NoContent, responseAction.StatusCode);
    }

    [Fact]
    public async Task UpdateAction_ReturnNoContent()
    {
        var serviceCreated = await this.CreateServiceAsync();

        var data = new AddControllerDto()
        {
            IdService = serviceCreated.Id,
            IdController = Guid.NewGuid(),
            Name = "Controller Test",
            Description = "Controller Test",
        };

        var json = JsonSerializer.Serialize(data);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/Service/{serviceCreated.Id}/controller", content, HttpMethod.Post);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var dataAction = new AddActionDto()
        {
            IdService = data.IdService,
            IdController = data.IdController,
            IdAction = Guid.NewGuid(),
            Name = "Action Test",
            Description = "Action Test",
        };

        var jsonAction = JsonSerializer.Serialize(dataAction);

        var contentAction = new StringContent(jsonAction, Encoding.UTF8, "application/json");

        var responseAction = await this.RequestAsync($"http://localhost/api/Service/{dataAction.IdService}/controller/{dataAction.IdController}/action", contentAction, HttpMethod.Post);

        Assert.NotNull(responseAction);
        Assert.Equal(HttpStatusCode.NoContent, responseAction.StatusCode);

        var dataUpdate = new UpdateActionDto()
        {
            IdService = dataAction.IdService,
            IdController = dataAction.IdController,
            IdAction = dataAction.IdAction,
            Name = "Action Test Updated",
            Description = "Action Test Updated",
        };

        var jsonUpdate = JsonSerializer.Serialize(dataUpdate);

        var contentUpdate = new StringContent(jsonUpdate, Encoding.UTF8, "application/json");

        var responseUpdate = await this.RequestAsync($"http://localhost/api/Service/{dataUpdate.IdService}/controller/{dataUpdate.IdController}/action/{dataUpdate.IdAction}", contentUpdate, HttpMethod.Put);

        Assert.NotNull(responseUpdate);
        Assert.Equal(HttpStatusCode.NoContent, responseUpdate.StatusCode);
    }

    [Fact]
    public async Task RemoveAction_ReturnNoContent()
    {
        var serviceCreated = await this.CreateServiceAsync();

        var data = new AddControllerDto()
        {
            IdService = serviceCreated.Id,
            IdController = Guid.NewGuid(),
            Name = "Controller Test",
            Description = "Controller Test",
        };

        var json = JsonSerializer.Serialize(data);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await this.RequestAsync($"http://localhost/api/Service/{serviceCreated.Id}/controller", content, HttpMethod.Post);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var dataAction = new AddActionDto()
        {
            IdService = data.IdService,
            IdController = data.IdController,
            IdAction = Guid.NewGuid(),
            Name = "Action Test",
            Description = "Action Test",
        };

        var jsonAction = JsonSerializer.Serialize(dataAction);

        var contentAction = new StringContent(jsonAction, Encoding.UTF8, "application/json");

        var responseAction = await this.RequestAsync($"http://localhost/api/Service/{dataAction.IdService}/controller/{dataAction.IdController}/action", contentAction, HttpMethod.Post);

        Assert.NotNull(responseAction);
        Assert.Equal(HttpStatusCode.NoContent, responseAction.StatusCode);

        var responseDelete = await this.RequestAsync($"http://localhost/api/Service/{dataAction.IdService}/controller/{dataAction.IdController}/action/{dataAction.IdAction}", null, HttpMethod.Delete);

        Assert.NotNull(responseDelete);
        Assert.Equal(HttpStatusCode.NoContent, responseDelete.StatusCode);
    }

    private static StringContent BuildBody(object data)
    {
        var json = JsonSerializer.Serialize(data);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        return content;
    }

    private async Task<CreateServiceDto> CreateServiceAsync()
    {
        var data = new CreateServiceDto()
        {
            Id = Guid.NewGuid(),
            Name = "Service Test",
            Description = "Service Test",
        };

        var json = JsonSerializer.Serialize(data);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await this.RequestAsync("http://localhost/api/Service", content, HttpMethod.Post);

        return data;
    }

    private async Task<ServiceDto> GetRecordAsync(Guid id)
    {
        var response = await this.RequestAsync($"http://localhost/api/Service/{id}", null, HttpMethod.Get);

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<ServiceDto>(json)!;
    }

    private async Task<HttpResponseMessage> RequestAsync(string uri, HttpContent? content, HttpMethod method)
    {
        var httpRequestMessage = new HttpRequestMessage()
        {
            RequestUri = new Uri(uri),
            Content = content,
            Method = method
        };
        httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("TestAuth");

        var response = await Client.SendAsync(httpRequestMessage);

        return response;
    }

}
