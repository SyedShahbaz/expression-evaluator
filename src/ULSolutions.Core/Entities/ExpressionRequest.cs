using System.ComponentModel.DataAnnotations;

namespace ULSolutions.Core.Entities;

public class ExpressionRequest
{
    [Required]
    [RegularExpression(@"^[0-9]+[\+\-\*\/][0-9]+([\+\-\*\/][0-9]+)*$", ErrorMessage = "Invalid request expression.")]
    public string Expression { get; set; } = string.Empty;
}