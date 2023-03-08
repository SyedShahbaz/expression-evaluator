using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ULSolutions.Core.Services;

namespace ULSolutions.Core.Tests.Services;

public class ExpressionEvaluatorServiceTests
{
    private readonly ExpressionEvaluatorService _expressionEvaluatorService;
    private readonly Mock<ILogger<ExpressionEvaluatorService>> _logger;

    public ExpressionEvaluatorServiceTests()
    {
        _logger = new Mock<ILogger<ExpressionEvaluatorService>>();
        _expressionEvaluatorService = new ExpressionEvaluatorService(_logger.Object);
    }
    
    [Theory]
    [InlineData("1+2*3-4/2", "5")]
    [InlineData("13+2-22-23-23/23*123", "-153")]
    [InlineData("2+2+2+2+2", "10")]
    public async Task Evaluate_Should_Return_CorrectResult(string expression, string expected)
    {
        var result = await _expressionEvaluatorService.Evaluate(expression);
        
        result.Should().Be(expected);
    }
    
    
    [Theory]
    [InlineData(new[] { 3, 4, 2 }, new[] { "+", "-", }, 5)]
    [InlineData(new[] { 10, 5, 2 }, new[] { "-", "+" }, 7)]
    [InlineData(new[] { 5, 3 }, new[] { "+" }, 8)]
    [InlineData(new[] { 15, 5 }, new[] { "-" }, 10)]
    public void EvaluateAdditionAndSubtraction_Should_Return_CorrectResult(int[] values, string[] operators, int expected)
    {
        var result = _expressionEvaluatorService.EvaluateAdditionAndSubtraction(values, operators);

        result.Should().BeGreaterOrEqualTo(expected);
    }
    
    [Theory]
    [InlineData("4+5-6*3/2", new[] { 4, 5, 6, 3, 2 }, new[] { "+", "-", "*", "/" })]
    [InlineData("2*3/4+5-6", new[] { 2, 3, 4, 5, 6 }, new[] { "*", "/", "+", "-" })]
    public void SplitExpression_Should_Return_CorrectValuesAndOperators(string expression, int[] expectedValues, string[] expectedOperators)
    {
        var (values, operators) = _expressionEvaluatorService.SplitExpression(expression);

        values.Should().BeEquivalentTo(expectedValues);
        operators.Should().BeEquivalentTo(expectedOperators);
    }
    
    
    [Theory]
    [InlineData(new[] { 3, 4, 2 }, new[] { "*", "/" }, new[] { 6 }, new string[] { } )]
    [InlineData(new[] { 6, 2, 3 }, new[] { "/", "*",  }, new[] { 9 }, new string[] { } )]
    [InlineData(new[] { 6, 2, 3, 3 }, new[] { "/", "*", "-"  }, new[] { 9,3 }, new string[] { "-" } )]
    public void EvaluateMultiplicationAndDivision_Should_Return_CorrectResult(int[] values, string[] operators, int[] expectedValues, string[] expectedOperators)
    {
        var (resultValues, resultOperators) = _expressionEvaluatorService.EvaluateMultiplicationAndDivision(values, operators);
        
        resultValues.Should().BeEquivalentTo(expectedValues);
        resultOperators.Should().BeEquivalentTo(expectedOperators);
    }
    
    [Fact]
    public void EvaluateMultiplicationAndDivision_Should_ThrowDivideByZeroException_WhenDividingByZero()
    {
        var values = new[] { 2, 3, 0, 4 };
        var operators = new[] { "*", "/", "/" };
        
        Action action = () => _expressionEvaluatorService.EvaluateMultiplicationAndDivision(values, operators);
        action.Should().Throw<DivideByZeroException>();
    }
    
}