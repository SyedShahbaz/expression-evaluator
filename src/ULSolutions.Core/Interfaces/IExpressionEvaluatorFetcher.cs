using ULSolutions.Core.Entities;

namespace ULSolutions.Core.Interfaces;

public interface IExpressionEvaluatorFetcher
{
    Task<ExpressionResponse> Fetch(ExpressionRequest expressionRequest);
}