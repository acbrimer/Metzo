namespace Metzo.Core.Filters;

using System.Collections.Generic;
using System.Linq.Expressions;
public enum FilterGroupOperator
{
    AND,
    OR
}

public class FilterGroup : IFilterItem
{
    public FilterGroupOperator Operator { get; set; } = FilterGroupOperator.AND;
    public List<IFilterItem> Filters { get; set; } = new List<IFilterItem>();
    public string Error { get; set; }

    public FilterGroup(FilterGroupOperator @operator, params IFilterItem[] filters)
    {
        Operator = @operator;
        Filters = filters is not null && filters.Count() > 0 ? filters.ToList() : new List<IFilterItem>() { };
    }
    public FilterGroup(params IFilterItem[] filters)
    {
        Operator = FilterGroupOperator.AND;
        Filters = filters is not null && filters.Count() > 0 ? filters.ToList() : new List<IFilterItem>() { };
    }

    public Expression<T> GetFilterExpression<T>(ParameterExpression parameter)
    {

        var lambdaType = typeof(T);
        var entityType = lambdaType.GenericTypeArguments.FirstOrDefault() ?? typeof(object);

        Expression combinedExpression = null;

        foreach (var filter in Filters)
        {
            var filterExpression = filter.GetFilterExpression<T>(parameter)?.Body;
            // Don't add filterExpressions that returned null
            if (filterExpression is not null)
            {
                combinedExpression = combinedExpression == null ? filterExpression :
                    Operator == FilterGroupOperator.AND ? Expression.AndAlso(combinedExpression, filterExpression) :
                    Operator == FilterGroupOperator.OR ? Expression.OrElse(combinedExpression, filterExpression) :
                    Expression.AndAlso(combinedExpression, filterExpression);
            }
        }
        if (combinedExpression is null)
        {
            Error = $"FilterGroupOperator {Operator} is not supported.";
            throw new NotSupportedException(Error);
        }
        return Expression.Lambda<T>(combinedExpression, parameter);
    }
}