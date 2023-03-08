using FluentAssertions;
using NetArchTest.Rules;
using ULSolutions.Core.Services;
using ULSolutions.Web.Controllers;

namespace ULSolutions.Architecture.Tests.Infrastructure;

public class InfrastructureTests
{
    [Fact]
    public void CoreLayer_Should_Not_BeDependentOn_WebLayer()
    {
        var result = Types.InAssembly(typeof(ExpressionEvaluatorService).Assembly)
            .That()
            .ResideInNamespace("ULSolutions.Core")
            .ShouldNot()
            .HaveDependencyOn("ULSolutions.Web")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void WebLayer_Should_Be_DependentOn_CoreLayer()
    {
        var result = Types.InAssembly(typeof(EvaluateExpressionController).Assembly)
            .That()
            .ResideInNamespace("ULSolutions.Web")
            .Should()
            .NotBeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}