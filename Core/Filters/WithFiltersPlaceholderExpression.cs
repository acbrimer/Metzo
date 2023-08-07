namespace Metzo.Core.Filters;

using System;
using System.Linq.Expressions;

public class WithFiltersPlaceholderExpression : Expression
{
    private static readonly WithFiltersPlaceholderExpression _placeholder = new WithFiltersPlaceholderExpression();
    public static WithFiltersPlaceholderExpression GetExpression()
    {
        return _placeholder;
    }
    // NodeType is overridden to return ExpressionType.Extension.
    public override ExpressionType NodeType => ExpressionType.Extension;

    // Type is overridden to return a specific type, in this case bool.
    public override Type Type => typeof(bool);

    // The Reduce method can be overridden to provide a simple reduction for the expression.
    // This is used by LINQ methods to simplify the expression before processing it.
    // In this case, we simply reduce it to an Equal expression.
    public override bool CanReduce => true;
    public override Expression Reduce()
    {
        return Expression.Constant(true);
    }

}
