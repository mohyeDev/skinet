using System;
using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specification;

public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
{

    protected BaseSpecification() : this(null) { }
    public Expression<Func<T, bool>> Criteria => criteria;

    public Expression<Func<T, object>>? OrderBy { get; private set; }

    public Expression<Func<T, object>>? OrderByDesc { get; private set; }

    public bool IsDistnict { get; private set; }

    public int Take {get ; private set ;}
    public int Skip {get ; private set ;}

    public bool IsPagingEnabled {get ; private set ;}
    public List<Expression<Func<T, object>>> Includes {get;} = [];

    public List<string> IncludeStrings {get;} = [];


    protected void AddInclude(Expression<Func<T,object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void AddInclude(string includeString ){
        IncludeStrings.Add(includeString);
    }

    public IQueryable<T> ApplyCriteria(IQueryable<T> query)
    {
        if(criteria is not null)
        {
            query = query.Where(criteria);
        }

        return query ; 
    }

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {

        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByDesc = orderByDescendingExpression;
    }

    protected void ApplyDistinct()
    {
        IsDistnict = true;
    }

    protected void ApplyPaging(int skip , int take)
    {
            Skip = skip ; 
            Take = take ; 
            IsPagingEnabled = true  ; 
    }
}


public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria)
: BaseSpecification<T>(criteria), ISpecification<T, TResult>
{

    protected BaseSpecification() : this(null) { }
    public Expression<Func<T, TResult>>? Select { get; private set; }

    protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
    {
        Select = selectExpression;
    }
}