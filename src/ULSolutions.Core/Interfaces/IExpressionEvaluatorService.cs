namespace ULSolutions.Core.Interfaces;

public interface IExpressionEvaluatorService
{
    Task<string> Evaluate(string expression);
}