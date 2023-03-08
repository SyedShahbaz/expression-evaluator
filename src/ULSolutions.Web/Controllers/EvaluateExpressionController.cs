using Microsoft.AspNetCore.Mvc;
using ULSolutions.Core.Entities;
using ULSolutions.Core.Interfaces;

namespace ULSolutions.Web.Controllers;

public class EvaluateExpressionController : BaseApiController
{
    private readonly IExpressionEvaluatorFetcher _expressionEvaluatorFetcher;

    public EvaluateExpressionController(IExpressionEvaluatorFetcher expressionEvaluatorFetcher)
    {
        _expressionEvaluatorFetcher = expressionEvaluatorFetcher;
    }

    [HttpPost(Name = "EvaluateExpression")]
    public async Task<ExpressionResponse> Post(ExpressionRequest expressionRequest)
    {
        return await _expressionEvaluatorFetcher.Fetch(expressionRequest);
    }
}