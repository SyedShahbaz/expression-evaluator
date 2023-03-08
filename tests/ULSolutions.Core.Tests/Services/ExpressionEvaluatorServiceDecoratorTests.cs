using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using ULSolutions.Core.Interfaces;
using ULSolutions.Core.Services;

namespace ULSolutions.Core.Tests.Services;

public class ExpressionEvaluatorServiceDecoratorTests
{
    private readonly Mock<IExpressionEvaluatorService> _mockInnerService;
    private readonly IMemoryCache _memoryCache;
    private readonly ExpressionEvaluatorServiceDecorator _decorator;

    public ExpressionEvaluatorServiceDecoratorTests()
    { 
        _mockInnerService = new Mock<IExpressionEvaluatorService>();
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _decorator = new ExpressionEvaluatorServiceDecorator(_mockInnerService.Object, _memoryCache);
    }

    [Fact]
    public async Task Evaluate_Should_Return_Result_From_Cache_If_Available()
    {
        const string expression = "1+2";
        const string expectedResult = "3";
        _memoryCache.Set(expression, expectedResult);
        
        var result = await _decorator.Evaluate(expression);
        
        result.Should().Be(expectedResult);
        _mockInnerService.Verify(service => service.Evaluate(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Evaluate_Should_Evaluate_Expression_And_Add_Result_To_Cache_If_Not_Available()
    {
        const string expression = "1+2";
        const string expectedResult = "3";
        _mockInnerService.Setup(service => service.Evaluate(expression)).ReturnsAsync(expectedResult);
        
        var result = await _decorator.Evaluate(expression);
        
        result.Should().Be(expectedResult);
        _memoryCache.Get(expression).Should().Be(expectedResult);
        _mockInnerService.Verify(service => service.Evaluate(expression), Times.Once);
    }
}