namespace Metzo.Core;

using Metzo.Core.Filters;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> WithFilters<TEntity>(this IQueryable<TEntity> source, string filterKey = ".")
    {
        // Define the parameter
        var parameter = Expression.Parameter(typeof(TEntity), filterKey);
        // Create a new WithFiltersPlaceholderExpression that will reduce to `Expression.Constant(true)`
        var placeholderExpression = WithFiltersPlaceholderExpression.GetExpression();

        // lambda will be ignored in query unless replaced by ExpressionVisitor
        var lambda = Expression.Lambda<Func<TEntity, bool>>(placeholderExpression, parameter);
        // Return `source.Where(<filterKey> => true)`
        return source.Where(lambda);
    }

    public static IOrderedQueryable<TSource> OrderBy<TSource>(
       this IQueryable<TSource> query, string propertyName)
    {
        var entityType = typeof(TSource);

        // Build OrderBy selector: r => r.PropName
        var propertyInfo = entityType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        ParameterExpression arg = Expression.Parameter(entityType, "r");
        MemberExpression property = Expression.Property(arg, propertyName);
        var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

        // Get reference to System.Linq.Queryable.OrderBy()
        var enumarableType = typeof(System.Linq.Queryable);
        var method = enumarableType.GetMethods()
             .Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition)
             .Where(m =>
             {
                 var parameters = m.GetParameters().ToList();
                 // Find `OrderBy` overload with 2 parameters            
                 return parameters.Count == 2;
             }).Single();
        // Provide generic types to OrderBy<TSource, TKey>
        MethodInfo genericMethod = method
             .MakeGenericMethod(entityType, propertyInfo.PropertyType);

        // Call query.OrderBy(selector), with query and selector: x=> x.PropName
        var newQuery = (IOrderedQueryable<TSource>)genericMethod
             .Invoke(genericMethod, new object[] { query, selector });
        return newQuery;
    }

    public static IOrderedQueryable<TSource> OrderByDescending<TSource>(
           this IQueryable<TSource> query, string propertyName)
    {
        var entityType = typeof(TSource);

        // Build OrderByDescending selector: r => r.PropName
        var propertyInfo = entityType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        ParameterExpression arg = Expression.Parameter(entityType, "r");
        MemberExpression property = Expression.Property(arg, propertyName);
        var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

        // Get reference to System.Linq.Queryable.OrderByDescending()
        var enumarableType = typeof(System.Linq.Queryable);
        var method = enumarableType.GetMethods()
             .Where(m => m.Name == "OrderByDescending" && m.IsGenericMethodDefinition)
             .Where(m =>
             {
                 var parameters = m.GetParameters().ToList();
                 // Find `OrderByDescending` overload with 2 parameters               
                 return parameters.Count == 2;
             }).Single();
        // Provide generic types to OrderByDescending<TSource, TKey>
        MethodInfo genericMethod = method
             .MakeGenericMethod(entityType, propertyInfo.PropertyType);

        // Call query.OrderByDescending(selector), with query and selector: r => r.PropName
        var newQuery = (IOrderedQueryable<TSource>)genericMethod
             .Invoke(genericMethod, new object[] { query, selector });
        return newQuery;
    }
}
