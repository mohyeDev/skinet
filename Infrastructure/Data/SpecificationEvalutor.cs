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

        return query;
    }

}
