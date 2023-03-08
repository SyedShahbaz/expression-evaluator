using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ULSolutions.Core.Entities;
using ULSolutions.Core.Interfaces;
using ULSolutions.Core.Services;

namespace ULSolutions.Core.Tests.Services;

public class ExpressionEvaluatorFetcherTests
{
    private readonly Mock<IExpressionEvaluatorService> _expressionEvaluatorService;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<ILogger<ExpressionEvaluatorFetcher>> _logger;
    private readonly ExpressionEvaluatorFetcher _expressionEvaluatorFetcher;

    public ExpressionEvaluatorFetcherTests()
    {
        _mapper = new Mock<IMapper>();
        _logger = new Mock<ILogger<ExpressionEvaluatorFetcher>>();
        _expressionEvaluatorService = new Mock<IExpressionEvaluatorService>();
        _expressionEvaluatorFetcher = new ExpressionEvaluatorFetcher(_expressionEvaluatorService.Object, _mapper.Object, _logger.Object);
    }

    [Fact]
    private void Fetch_Returns_MappedResult_After_Evaluating()
    {
        var expressionRequest = new ExpressionRequest() {Expression = "1+3"};
        var mappedResult = new ExpressionResponse() {Response = "4"};
        const string result = "4";
        _expressionEvaluatorService.Setup(e => e.Evaluate(expressionRequest.Expression)).ReturnsAsync(result);
        _mapper.Setup(m => m.Map<ExpressionResponse>(result)).Returns(mappedResult);
        
        _logger.Setup(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            )
        );
        
        var data = _expressionEvaluatorFetcher.Fetch(expressionRequest);

        data.Result.Response.Should().BeEquivalentTo(result);
        _logger.Verify(
            m => m.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once
        );
        
    }
}