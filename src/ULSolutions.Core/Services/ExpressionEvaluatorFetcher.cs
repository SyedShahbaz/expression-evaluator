using AutoMapper;
using Microsoft.Extensions.Logging;
using ULSolutions.Core.Entities;
using ULSolutions.Core.Interfaces;

namespace ULSolutions.Core.Services;

public class ExpressionEvaluatorFetcher : IExpressionEvaluatorFetcher
{
    private readonly IExpressionEvaluatorService _evaluatorService;
    private readonly IMapper _mapper;
    private readonly ILogger<ExpressionEvaluatorFetcher> _logger;

    public ExpressionEvaluatorFetcher(IExpressionEvaluatorService evaluatorService, IMapper mapper, ILogger<ExpressionEvaluatorFetcher> logger)
    {
        _evaluatorService = evaluatorService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ExpressionResponse> Fetch(ExpressionRequest expressionRequest)
    {
        _logger.LogInformation("Evaluation of expression started!");
        return _mapper.Map<ExpressionResponse>(await _evaluatorService.Evaluate(expressionRequest.Expression));
    }
}