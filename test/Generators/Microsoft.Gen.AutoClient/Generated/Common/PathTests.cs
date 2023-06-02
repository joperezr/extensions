// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using TestClasses;
using Xunit;

namespace Microsoft.Gen.AutoClient.Test;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "IDisposable inside mock setups")]
public class PathTests
{
    private readonly Mock<HttpMessageHandler> _handlerMock = new(MockBehavior.Strict);
    private readonly Mock<IHttpClientFactory> _factoryMock = new(MockBehavior.Strict);

    private readonly IPathTestClient _sut;

    public PathTests()
    {
        _factoryMock.Setup(m => m.CreateClient("MyClient")).Returns(new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new Uri("https://example.com/")
        });

        var services = new ServiceCollection();
        services.AddSingleton(_ => _factoryMock.Object);
        services.AddPathTestClient();
        var provider = services.BuildServiceProvider();

        _sut = provider.GetRequiredService<IPathTestClient>();
    }

    [Fact]
    public async Task SimplePath()
    {
        _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(message =>
                    message.Method == HttpMethod.Get &&
                    message.RequestUri != null &&
                    message.RequestUri.PathAndQuery == "/api/users/myUser"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("Success!")
                });

        var response = await _sut.GetUser("myUser");

        Assert.Equal("Success!", response);
    }

    [Fact]
    public async Task MultiplePathParameters()
    {
        _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(message =>
                    message.Method == HttpMethod.Get &&
                    message.RequestUri != null &&
                    message.RequestUri.PathAndQuery == "/api/users/myTenant/myUser"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("Success!")
                });

        var response = await _sut.GetUserFromTenant("myTenant", "myUser");

        Assert.Equal("Success!", response);
    }
}