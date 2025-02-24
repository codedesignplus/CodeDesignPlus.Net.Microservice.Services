global using CodeDesignPlus.Microservice.Api.Dtos;
global using CodeDesignPlus.Net.Logger.Extensions;
global using CodeDesignPlus.Net.Mongo.Extensions;
global using CodeDesignPlus.Net.Observability.Extensions;
global using CodeDesignPlus.Net.RabbitMQ.Extensions;
global using CodeDesignPlus.Net.Redis.Extensions;
global using CodeDesignPlus.Net.Security.Extensions;
global using Mapster;
global using MapsterMapper;
global using MediatR;
global using Microsoft.AspNetCore.Mvc;
global using C = CodeDesignPlus.Net.Core.Abstractions.Models.Criteria;
global using CodeDesignPlus.Net.Serializers;
global using NodaTime;









global using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.CreateService;
global using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateService;
global using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.DeleteService;
global using CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetServiceById;
global using CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetAllService;
global using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddController;
global using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateController;
global using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.RemoveController;
global using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddAction;
global using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.UpdateAction;
global using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.RemoveAction;
global using CodeDesignPlus.Net.Microservice.Services.Application.Service.Queries.GetServiceByName;
global using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddControllers;
global using CodeDesignPlus.Net.Microservice.Services.Application.Service.Commands.AddActions;