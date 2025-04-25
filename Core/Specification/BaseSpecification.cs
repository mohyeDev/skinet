using System;
using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specification;

public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
{

    protected BaseSpecification() : this(null) { }
    public Expression<Func<T, bool>> Criteria => criteria;

    public Expression<Func<T, object>>? OrderBy { get; private set; }

    public Expression<Func<T, object>>? OrderByDesc { get; private set ;}

    protected void AddOrderBy(Expression<Func<T,object>> orderByExpression){

        OrderBy = orderByExpression ; 
    }

    protected void AddOrderByDescending(Expression<Func<T,object>> orderByDescendingExpression)
    {
        OrderByDesc = orderByDescendingExpression  ;
    }
}


public class BaseSpecification<T, TResult>(Expression<Func<T, bool>> criteria)
: BaseSpecification<T>(criteria), ISpecification<T, TResult>
{
    public Expression<Func<T, TResult>>? Select {get; private set;}
}