using Microsoft.Extensions.Caching.Memory;
using ULSolutions.Core.Interfaces;

namespace ULSolutions.Core.Services;

public class ExpressionEvaluatorServiceDecorator : IExpressionEvaluatorService
{
    private readonly IExpressionEvaluatorService _expressionEvaluatorService;
    private readonly IMemoryCache _memoryCache;

    public ExpressionEvaluatorServiceDecorator(IExpressionEvaluatorService expressionEvaluatorService, IMemoryCache memoryCache)
    {
        _expressionEvaluatorService = expressionEvaluatorService;
        _memoryCache = memoryCache;
    }

    public async Task<string> Evaluate(string expression)
    {
        if (_memoryCache.TryGetValue(expression, out string? result))
        {
            return (await Task.FromResult(result))!;
        }
        
        result = await _expressionEvaluatorService.Evaluate(expression);

        _memoryCache.Set(expression, result);

        return result;
    }
}