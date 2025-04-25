using System;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class SpecificationEvalutor<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query , ISpecification<T> spec)
    {
        if(spec.Criteria is not null)
        {
            query = query.Where(spec.Criteria); // x => x.Brand == brand 
        }

        if(spec.OrderBy is not null){
            query = query.OrderBy(spec.OrderBy);
        }

        if(spec.OrderByDesc is not null)
        {
            query = query.OrderByDescending(spec.OrderByDesc);
        }

        return query;
    }


    public static IQueryable<TResult> GetQuery<Tspec , TResult>(IQueryable<T> query , ISpecification<T , TResult> spec)
    {
        if(spec.Criteria is not null)
        {
            query = query.Where(spec.Criteria);
        }

        if(spec.OrderBy is not null)
        {
            query = query.OrderBy(spec.OrderBy);
        }


        if(spec.OrderByDesc is not null)
        {
            query = query.OrderByDescending(spec.OrderByDesc);
        }

        var selectQuery = query as IQueryable<TResult>;

        if(spec.Select is not null)
        {

               selectQuery = query.Select(spec.Select);

        }


        return selectQuery ?? query.Cast<TResult>() ; 







    }

}
