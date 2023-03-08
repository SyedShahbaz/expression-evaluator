using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Moq;
using ULSolutions.Core.Entities;
using ULSolutions.Core.Interfaces;
using ULSolutions.Web.Controllers;

namespace ULSolutions.Web.Tests.Controllers;

public class EvaluateExpressionControllerTests
{
    private readonly Mock<IExpressionEvaluatorFetcher> _expressionEvaluator;
    private readonly EvaluateExpressionController _evaluateExpressionController;

    public EvaluateExpressionControllerTests()
    {
        _expressionEvaluator = new Mock<IExpressionEvaluatorFetcher>();
        _evaluateExpressionController = new EvaluateExpressionController(_expressionEvaluator.Object);
    }

    [Fact]
    private void Post_ExpressionRequest_WithValidValues_ShouldReturnCorrectResult()
    {
        var expressionRequest = new ExpressionRequest() {Expression = "1+3"};
        var expected = new ExpressionResponse() {Response = "4"};
        _expressionEvaluator.Setup(f 
            => f.Fetch(It.IsAny<ExpressionRequest>())).ReturnsAsync(expected);

        var result = _evaluateExpressionController.Post(expressionRequest);

        result.Result.Response.Should().BeEquivalentTo(expected.Response);
    }
    
    
    [Fact]
    public void Post_ExpressionRequest_WithValidValues_ShouldPassValidation()
    {
        var expressionRequest = new ExpressionRequest
        {
            Expression = "5+3*4"
        };
        var context = new ValidationContext(expressionRequest);
        var result = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(expressionRequest, context, result, true);

        isValid.Should().BeTrue();
        result.Should().BeEmpty();
    }
    
    [Fact]
    public void Post_ExpressionRequest_WithInvalidValues_ShouldFailValidation()
    {
        var expressionRequest = new ExpressionRequest
        {
            Expression = "invalid expression"
        };
        var context = new ValidationContext(expressionRequest);
        var result = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(expressionRequest, context, result, true);

        isValid.Should().BeFalse();
        result.Should().NotBeEmpty().And.HaveCount(1);
        result[0].ErrorMessage.Should().Be("Invalid request expression.");
    }
    
    [Fact]
    public void Post_ExpressionRequest_WithInvalidMathematicalValues_ShouldFailValidation()
    {
        var expressionRequest = new ExpressionRequest
        {
            Expression = "-2+1/0"
        };
        var context = new ValidationContext(expressionRequest);
        var result = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(expressionRequest, context, result, true);

        isValid.Should().BeFalse();
        result.Should().NotBeEmpty().And.HaveCount(1);
        result[0].ErrorMessage.Should().Be("Invalid request expression.");
    }
}