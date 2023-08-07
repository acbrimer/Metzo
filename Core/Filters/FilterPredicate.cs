namespace Metzo.Core.Filters;

using System.Linq.Expressions;
using System.Reflection;

public class FilterPredicate : IFilterItem
{
    public string Field { get; set; }
    public FilterOperator Operator { get; set; } = FilterOperator.eq;
    public object Value { get; set; }
    public Type DataType { get => GetDataType(Value); }
    public bool Applied { get; set; } = false;
    public string? Error { get; set; } = null;
    public FilterPredicate(string field, object value)
    {
        Field = field;
        Value = value;
    }
    public FilterPredicate(string field, FilterOperator @operator, object value)
    {
        Field = field;
        Operator = @operator;
        Value = value;
    }
    public Expression<T> GetFilterExpression<T>(ParameterExpression parameter)
    {
        var lambdaType = typeof(T);
        var entityType = lambdaType.GenericTypeArguments.FirstOrDefault() ?? typeof(object);

        var propertyInfo = entityType.GetProperty(Field, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        
        var property = Expression.Property(parameter, propertyInfo);

        var constant = Expression.Constant(Value);
        // Build binary expression with the right Operator
        Expression expression = Operator switch
        {
            FilterOperator.eq => Expression.Equal(property, constant),
            FilterOperator.ne => Expression.NotEqual(property, constant),
            FilterOperator.gt => Expression.GreaterThan(property, constant),
            FilterOperator.lt => Expression.LessThan(property, constant),
            FilterOperator.gte => Expression.GreaterThanOrEqual(property, constant),
            FilterOperator.lte => Expression.LessThanOrEqual(property, constant),
            FilterOperator.@in => Expression.Call(typeof(Enumerable), nameof(Enumerable.Contains), new[] { DataType }, constant, property),
            FilterOperator.nin => Expression.Not(Expression.Call(typeof(Enumerable), nameof(Enumerable.Contains), new[] { DataType }, constant, property)),
            FilterOperator.like => Expression.Call(property, "Contains", null, constant),
            FilterOperator.nlike => Expression.Not(Expression.Call(property, "Contains", null, constant)),
            _ => Expression.Equal(property, constant),
        };

        var newExpression = Expression.Lambda<T>(expression, parameter);
        return newExpression;
    }

    public static Type GetDataType(object value)
    {
        return value.GetType();
    }
}

public enum FilterOperator
{
    // Equal to
    eq,
    // Not equal to
    ne,
    // Greater than
    gt,
    // Less than
    lt,
    // Greater than/equal to
    gte,
    // Less than/not equal to
    lte,
    // Included in array
    @in,
    // Not included in array
    nin,
    // String like
    like,
    // String not like
    nlike
}
