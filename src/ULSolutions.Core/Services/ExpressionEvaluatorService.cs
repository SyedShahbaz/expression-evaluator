using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ULSolutions.Core.Interfaces;

namespace ULSolutions.Core.Services;

public class ExpressionEvaluatorService : IExpressionEvaluatorService
{
    private readonly ILogger<ExpressionEvaluatorService> _logger;

    public ExpressionEvaluatorService(ILogger<ExpressionEvaluatorService> logger)
    {
        _logger = logger;
    }

    public Task<string> Evaluate(string expression)
    {
        try
        {
            var (values, operators) = SplitExpression(expression);

            var (newValues, newOperators) = EvaluateMultiplicationAndDivision(values, operators);
        
            return Task.FromResult(EvaluateAdditionAndSubtraction(newValues, newOperators).ToString(CultureInfo.InvariantCulture));
        }
        catch(Exception exception)
        {
            _logger.LogError("{ExceptionStackTrace}", exception.ToString());
            throw;
        }
        
    }
    
    public  (int[] values, string[] operators) SplitExpression(string expression)
    {
        // Split the expression into operands and operators.
        var operands = Regex.Split(expression, "[\\+\\-\\*\\/]");
        var operators = Regex.Matches(expression, "[\\+\\-\\*\\/]").Cast<Match>().Select(m => m.Value).ToArray();

        var values = Array.ConvertAll(operands, int.Parse);

        return (values, operators);
    }
    
    public  (int[] values, string[] operators) EvaluateMultiplicationAndDivision(int[] values, string[] operators)
    {
        var result = 0;

        for (var i = 0; i < operators.Length; i++)
        {
            if (operators[i] == "*" || operators[i] == "/")
            {
                var leftOperand = values[i];
                var rightOperand = values[i + 1];

                result = operators[i] switch
                {
                    "*" => leftOperand * rightOperand,
                    "/" => leftOperand / rightOperand,
                    _ => 0
                };

                // Replace the two operands and the operator with the result.
                var newValues = new List<int>();
                var newOperators = new List<string>();
                for (var j = 0; j < i; j++)
                {
                    newValues.Add(values[j]);
                    newOperators.Add(operators[j]);
                }
                newValues.Add(result);
                for (var j = i + 2; j < values.Length; j++)
                {
                    newValues.Add(values[j]);
                }
                for (var j = i + 1; j < operators.Length; j++)
                {
                    newOperators.Add(operators[j]);
                }

                // Update the values and operators arrays for the next iteration.
                values = newValues.ToArray();
                operators = newOperators.ToArray();
                i--;
            }
        }

        return (values, operators);
    }
    
    public  int EvaluateAdditionAndSubtraction(int[] values, string[] operators)
    {
        var result = values[0];
        for (var i = 0; i < operators.Length; i++)
        {
            var nextValue = values[i + 1];

            result = operators[i] switch
            {
                "+" => result + nextValue,
                "-" => result - nextValue,
                _ => 0
            };
        }

        return result;
    }
}