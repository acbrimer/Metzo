namespace Metzo.Core.Filters;

using System.Linq.Expressions;

public interface IFilterItem
{
    public Expression<T> GetFilterExpression<T>(ParameterExpression parameter);

}
