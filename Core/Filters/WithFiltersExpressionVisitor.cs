namespace Metzo.Core.Filters;

using System;
using System.Linq;
using System.Linq.Expressions;

public class WithFiltersExpressionVisitor : ExpressionVisitor
{
    public Dictionary<string, FilterGroup>? Filters { get; set; }
    public WithFiltersExpressionVisitor(Dictionary<string, FilterGroup>? filters = null)
    {
        Filters = filters;
    }

    public IQueryable<TEntity> GetFilteredQuery<TEntity>(IQueryable<TEntity> inputQuery)
    {
        if (Filters is null || Filters.Keys.Count == 0)
            return inputQuery;

        var outputQueryExpression = Visit(inputQuery.Expression);
        var newQuery = inputQuery.Provider.CreateQuery<TEntity>(outputQueryExpression);
        return newQuery;
    }
    protected override Expression VisitLambda<T>(Expression<T> node)
    {
        if (node.Body.GetType() == typeof(WithFiltersPlaceholderExpression))
        {
            var filterKey = node.Parameters.FirstOrDefault()?.Name ?? ".";
            if (Filters is null || Filters.Keys.Count == 0 || !Filters.Keys.Contains(filterKey))
                return base.VisitLambda(node);
            var filterGroup = Filters[filterKey];
            var parameter = ((LambdaExpression)node).Parameters.FirstOrDefault();
            if (parameter is null)
                return base.VisitLambda(node);
            var filterExpression = filterGroup.GetFilterExpression<T>(parameter);
            var newNode = node.Update(filterExpression.Body, filterExpression.Parameters);
            return base.VisitLambda<T>(newNode);
        }

        return base.VisitLambda(node);
    }
}